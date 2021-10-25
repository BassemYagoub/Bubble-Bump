using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlatformFactory : MonoBehaviour {

    public GameObject platformPrefab;
    public GameObject movingPlatformPrefab;
    public GameObject breakablePlatformPrefab;
    public GameObject springPrefab;
    public float distanceBetweenPlateforms;

    private float difficulty = 5f; //chances (0-100%) of generating a non-simple platform
    private int platformDensity = 35; //nb of platforms to instantiate
    private List<GameObject> platforms;

    private GameObject player;
    private GameObject mainCamera;
    private float lastCameraYPos = 0;
    private int nextUpdate = 1; // Next update in second

    void Start() {
        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
        platforms = new List<GameObject>();
        platforms.Add(GameObject.FindGameObjectWithTag("Platform"));

        GeneratePlatforms();
    }

    void Update() {

        // If the next update is reached
        if (Time.time >= nextUpdate) {
            // Change the next update (current second+1)
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }

        //Generate new platforms if player reaches border of camera height
        if(mainCamera.transform.position.y > lastCameraYPos + (mainCamera.GetComponent<Camera>().orthographicSize*2))
            GeneratePlatforms(mainCamera.transform.position.y, mainCamera.GetComponent<Camera>().orthographicSize+.5f);
    }


    void UpdateEverySecond() {
        RemoveUnseeableObjects();
    }

    void GeneratePlatforms(float pos = 0f, float offset = 0f) {
        Vector2 newPos;
        bool overlaps;
        lastCameraYPos = mainCamera.transform.position.y;

        //50% chance of having one platform less than before
        if (Random.Range(0, 2) == 1 && platformDensity > 5) {
            platformDensity--;
        }

        for (int i = 0; i < platformDensity; i++) {
            newPos = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(1f+pos+offset, (mainCamera.GetComponent<Camera>().orthographicSize * 4) +pos));
            overlaps = false;

            foreach(GameObject platform in platforms){
                //if position overlaps existing platform
                if(Vector2.Distance(newPos, platform.transform.position) <= distanceBetweenPlateforms || Mathf.Abs(newPos.y - platform.transform.position.y) <= distanceBetweenPlateforms) {
                    overlaps = true;
                    break;
                }
            }
            if(!overlaps){
                CreatePlatform(newPos);
            }
            else {
                if(Random.Range(0,2) == 1)
                    i--; //Retry
            }
        }

        bool spacesToFill = false; //value by default
        platforms.Sort((v1, v2) => v1.transform.position.y.CompareTo(v2.transform.position.y)); //values will have to be sorted

        //if there is too much space between two platforms (in height), fill it with a platform
        do {
            spacesToFill = false;
            for (int i = 0; i < platforms.Count - 1; i++) {
                if (platforms[i].transform.position.y + 3f < platforms[i + 1].transform.position.y || (i<platforms.Count-2 && platforms[i + 1].CompareTag("BreakablePlatform") && platforms[i].transform.position.y + 3f < platforms[i + 2].transform.position.y)) {
                    newPos = new Vector2(Random.Range(-2.5f, 2.5f), platforms[i].transform.position.y + (platforms[i + 1].transform.position.y - platforms[i].transform.position.y) / 2);

                    CreatePlatform(newPos, false);
                    //Debug.Log("i:"+i+", "+platforms[i].transform.position.y+" i+1:"+ platforms[i+1].transform.position.y+" = new platform " + newPos);
                    spacesToFill = true;
                    platforms.Sort((v1, v2) => v1.transform.position.y.CompareTo(v2.transform.position.y));
                }
            }
        } while (spacesToFill);

        if(difficulty < 90f)
            difficulty += 2;
    }

    void RemoveUnseeableObjects() {
        float triggerDistance = mainCamera.transform.position.y - mainCamera.GetComponent<Camera>().orthographicSize;
        GameObject platformToRemove = null;

        for(int i=platforms.Count-1; i>=0; i--) {
            if (platforms[i].transform.position.y<triggerDistance) {
                platformToRemove = platforms[i];
                platforms.RemoveAt(i);
                Destroy(platformToRemove);
            }
        }
    }

    void CreatePlatform(Vector3 pos, bool breakable = true) {
        GameObject newPlatform;
        float rand = Random.Range(0, 100);

        if (rand <= difficulty*1){ //% chance to instantiate moving platform
            newPlatform = Instantiate(movingPlatformPrefab, pos, platformPrefab.transform.rotation);
        }
        else if (rand <= difficulty*3 && breakable && !platforms[platforms.Count-1].CompareTag("BreakablePlatform")) { //% chance to instantiate breakable platform
            newPlatform = Instantiate(breakablePlatformPrefab, pos, platformPrefab.transform.rotation);
        }
        else{ //default
            newPlatform = Instantiate(platformPrefab, pos, platformPrefab.transform.rotation);

            //spring on top of platform
            if (Random.Range(0, 10) <= 1) {
                GameObject newSpring = Instantiate(springPrefab, new Vector3(pos.x, pos.y + 0.19f, pos.z), springPrefab.transform.rotation);
                newSpring.transform.parent = newPlatform.gameObject.transform;
            }
        }

        newPlatform.transform.parent = this.gameObject.transform;
        platforms.Add(newPlatform);
    }

}

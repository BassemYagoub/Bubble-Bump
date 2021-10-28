using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlatformFactory : MonoBehaviour {

    public GameObject platformPrefab;
    public GameObject movingPlatformPrefab;
    public GameObject breakablePlatformPrefab;
    public GameObject springPrefab;
    public GameObject propellerPrefab;
    public GameObject jetPackPrefab;
    public float distanceBetweenPlatforms;

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
                if(Vector2.Distance(newPos, platform.transform.position) <= distanceBetweenPlatforms || Mathf.Abs(newPos.y - platform.transform.position.y) <= distanceBetweenPlatforms) {
                    overlaps = true;
                    break;
                }
            }
            if(!overlaps) {
                CreatePlatform(newPos);
            }
            else {
                if(Random.Range(0,2) == 1)
                    i--; //Retry
            }
        }

        bool spacesToFill = false; //value by default
        platforms.Sort((v1, v2) => v1.transform.position.y.CompareTo(v2.transform.position.y)); //values will have to be sorted

        //to avoid having 2 breakable platforms in a row
        for (int i = 0; i < platforms.Count-1; i++) {
            if (platforms[i].CompareTag("BreakablePlatform") && platforms[i+1].CompareTag("BreakablePlatform")) {
                //Debug.Log("replace breakable at" + platforms[i + 1].transform.position);
                GameObject platformToRemove = platforms[i + 1];
                Destroy(platformToRemove);
                platforms[i + 1] = Instantiate(platformPrefab, platforms[i + 1].transform.position, platformPrefab.transform.rotation);
                platforms[i+1].transform.parent = this.gameObject.transform;
            }
        }

        //if there is too much space between two platforms (in height), fill it with a platform
        do {
            spacesToFill = false;
            for (int i = 0; i < platforms.Count - 1; i++) {
                if (platforms[i].transform.position.y + 3f < platforms[i + 1].transform.position.y || (i < platforms.Count - 2 && platforms[i + 1].CompareTag("BreakablePlatform") && platforms[i].transform.position.y + 3f < platforms[i + 2].transform.position.y)) {

                    newPos = new Vector2(Random.Range(-2.5f, 2.5f), platforms[i].transform.position.y + (platforms[i + 1].transform.position.y - platforms[i].transform.position.y) / 2);

                    //2nd case
                    if (i < platforms.Count - 2 && platforms[i + 1].CompareTag("BreakablePlatform") && platforms[i].transform.position.y + 3f < platforms[i + 2].transform.position.y) {
                        if((platforms[i + 2].transform.position.y - platforms[i + 1].transform.position.y) > (platforms[i + 1].transform.position.y - platforms[i].transform.position.y))
                            newPos = new Vector2(Random.Range(-2.5f, 2.5f), platforms[i+1].transform.position.y + (platforms[i + 2].transform.position.y - platforms[i+1].transform.position.y) / 2);
                    }

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
            if (platforms[i].transform.position.y<triggerDistance-0.3f) {
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
        else if (rand <= difficulty*3 && breakable) { //% chance to instantiate breakable platform
            newPlatform = Instantiate(breakablePlatformPrefab, pos, platformPrefab.transform.rotation);
        }
        else{ //default
            newPlatform = Instantiate(platformPrefab, pos, platformPrefab.transform.rotation);
        }

        //objects on top of unbreakable platform
        if (Random.Range(0, 10) <= 1 && !newPlatform.CompareTag("BreakablePlatform")) {
            float xPosOffset = Random.Range(-0.35f, 0.35f);
            float yPosOffset = newPlatform.GetComponent<Renderer>().bounds.size.y;
            float randPercentage = Random.Range(0, 100);
            GameObject newObject;

            if (randPercentage < 70) // 70% chances of getting spring
                newObject = Instantiate(springPrefab, new Vector3(pos.x + xPosOffset, pos.y + yPosOffset-0.1f, pos.z), springPrefab.transform.rotation);
            else if (randPercentage >= 70 && randPercentage < 90) //20% chance of getting propeller
                newObject = Instantiate(propellerPrefab, new Vector3(pos.x + xPosOffset, pos.y + yPosOffset - 0.05f, pos.z), propellerPrefab.transform.rotation);
            else {//10% chance of getting jetpack
                newObject = Instantiate(jetPackPrefab, new Vector3(pos.x + xPosOffset, pos.y + yPosOffset + 0.2f, pos.z), jetPackPrefab.transform.rotation);
            }

            newObject.transform.parent = newPlatform.gameObject.transform;
        }

        newPlatform.transform.parent = this.gameObject.transform;
        platforms.Add(newPlatform);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlatformFactory : MonoBehaviour {

    public GameObject platformPrefab;
    public float distanceBetweenPlateforms;
    private int platformDensity = 30; //nb of platforms to instantiate
    private List<GameObject> platforms;

    private GameObject player;
    private GameObject mainCamera;
    private float lastCameraYPos = 0;
    private int nextUpdate = 1; // Next update in second

    void Start() {
        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
        platforms = new List<GameObject>();

        GeneratePlatforms();
    }

    void Update() {

        // If the next update is reached
        if (Time.time >= nextUpdate) {
            // Change the next update (current second+1)
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }

        if(mainCamera.transform.position.y > lastCameraYPos + (mainCamera.GetComponent<Camera>().orthographicSize*2))
            GeneratePlatforms(mainCamera.transform.position.y, mainCamera.GetComponent<Camera>().orthographicSize+.5f);
    }


    void UpdateEverySecond() {
        RemoveUnseeableObjects();
    }

    void GeneratePlatforms(float pos = 0f, float offset = 0f) {
        Debug.Log(mainCamera.GetComponent<Camera>().orthographicSize);
        Vector2 newPos;
        bool overlaps;
        lastCameraYPos = mainCamera.transform.position.y;

        //33% chance of having one platform
        if (Random.Range(0, 2) == 1 && platformDensity > 5) {
            platformDensity--;
        }

        for (int i = 0; i < platformDensity; i++) {
            newPos = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(1f+pos+offset, (mainCamera.GetComponent<Camera>().orthographicSize * 4) +pos));
            overlaps = false;

            foreach(GameObject platform in platforms){
                //if position overlaps existing platform
                if(Vector2.Distance(newPos, platform.transform.position) <= distanceBetweenPlateforms){
                    overlaps = true;
                    break;
                }
            }
            if(!overlaps){
                GameObject newPlatform = Instantiate(platformPrefab, newPos, platformPrefab.transform.rotation);
                newPlatform.transform.parent = this.gameObject.transform;
                platforms.Add(newPlatform);
            }
            else {
                //Try again
                //if(Random.Range(0, 2) == 1)
                  //  platformDensity++;
            }
                
        }
        Debug.Log(platformDensity);
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

}

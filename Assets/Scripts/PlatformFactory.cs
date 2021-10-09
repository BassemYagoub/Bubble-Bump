using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlatformFactory : MonoBehaviour {

    public GameObject platformPrefab;
    public float distanceBetweenPlateforms;
    private int platformDensity = 30; //nb of platforms to instantiate
    private GameObject player;
    private List<GameObject> platforms;

    void Start() {
        player = GameObject.Find("Player");
        platforms = new List<GameObject>();

        //float jumpForce = player.GetComponent<PlayerController>().jumpForce;

        GeneratePlatforms();
    }

    // Update is called once per frame
    void Update() {
    }

    void GeneratePlatforms() {
        Vector2 newPos;
        bool overlaps;

        for (int i = 0; i < platformDensity; i++) {
            newPos = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(-4f, 5f));
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
                platforms.Add(newPlatform);
            }
            else {
                //Try again ?
                if(Random.Range(0, 2) == 1)
                    platformDensity++;
            }
                
        }

    }

}

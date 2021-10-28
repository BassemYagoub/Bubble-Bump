using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public GameObject redEnemyPrefab;
    public GameObject pinkEnemyPrefab;
    public List<GameObject> ennemies;
    private GameObject mainCamera;
    private GameObject player;
    private int nextUpdate = 1; // Next update in second
    private float lastCameraYPos = 0;
    private float difficulty = 5f;
    private int nbEnnemies = 2; //nb of ennemies to instantiate
    private GameObject platforms;
    public float distanceBetweenPlatforms; //distance between ennemy and platforms

    // Start is called before the first frame update
    void Start(){
        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
        platforms = GameObject.Find("Platforms");
        ennemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update(){
        // If the next update is reached
        if (Time.time >= nextUpdate) {
            // Change the next update (current second+1)
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            //updates every second
            RemoveUnseeableObjects();
        }

        //Generate new platforms if player reaches border of camera height
        if (mainCamera.transform.position.y > lastCameraYPos + (mainCamera.GetComponent<Camera>().orthographicSize * 2))
            GenerateEnnemies(mainCamera.transform.position.y, mainCamera.GetComponent<Camera>().orthographicSize + .5f);
    }

    void CreateEnemy(Vector3 pos){
        GameObject newEnemy;
        float rand = Random.Range(0, 100);

        if (rand <= difficulty * 1){ //% chance to instantiate moving platform
            newEnemy = Instantiate(pinkEnemyPrefab, pos, pinkEnemyPrefab.transform.rotation);
        }
        else{ //% chance to instantiate breakable platform
            newEnemy = Instantiate(redEnemyPrefab, pos, redEnemyPrefab.transform.rotation);
        }

        newEnemy.transform.parent = this.gameObject.transform;
        ennemies.Add(newEnemy);
    }

    void RemoveUnseeableObjects() {
        float triggerDistance = mainCamera.transform.position.y - mainCamera.GetComponent<Camera>().orthographicSize;
        GameObject ennemyToRemove = null;

        for (int i = ennemies.Count - 1; i >= 0; i--) {
            if (ennemies[i].transform.position.y < triggerDistance - 0.5f) {
                ennemyToRemove = ennemies[i];
                ennemies.RemoveAt(i);
                Destroy(ennemyToRemove);
            }
        }
    }

    void GenerateEnnemies(float pos = 0f, float offset = 0f) {
        Vector2 newPos = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(1f + pos + offset, (mainCamera.GetComponent<Camera>().orthographicSize * 4) + pos));
        bool overlaps;
        lastCameraYPos = mainCamera.transform.position.y;

        for (int i = 0; i < nbEnnemies; i++) {
            newPos = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(1f + pos + offset, (mainCamera.GetComponent<Camera>().orthographicSize * 4) + pos));
            overlaps = false;

            foreach (Transform platform in platforms.transform) {
                //if position overlaps existing platform
                if (Vector2.Distance(newPos, platform.transform.position) <= distanceBetweenPlatforms || Mathf.Abs(newPos.y - platform.transform.position.y) <= distanceBetweenPlatforms) {
                    overlaps = true;
                    break;
                }
            }
            //create an enemy at newPos if it doesn't overlap a platform
            if (!overlaps) {
                CreateEnemy(newPos);
            }
            else {
                if (Random.Range(0, 2) == 1)
                    i--; //Retry
            }
        }

        if (difficulty < 90f)
            difficulty += 2;
    }

}

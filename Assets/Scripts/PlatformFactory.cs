using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlatformFactory : MonoBehaviour {

    public GameObject platformPrefab;
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
        RaycastHit2D hitUp, hitDown, hitLeft, hitRight;

        float yPos = 0f, radius = 1f, distance = .5f;

        for (int i = 0; i < platformDensity; i++) {
            yPos = Random.Range(-4f, 5f);
            GameObject newPlatform = Instantiate(platformPrefab, new Vector3(Random.Range(-2.5f, 2.5f), yPos), platformPrefab.transform.rotation);
            newPlatform.transform.parent = this.gameObject.transform;

            //check if there is another platform nearby, and delete this platform if yes
            hitUp = Physics2D.CircleCast(newPlatform.transform.position, radius, Vector2.up, distance);
            hitDown = Physics2D.CircleCast(newPlatform.transform.position, radius, Vector2.down, distance);
            hitLeft = Physics2D.CircleCast(newPlatform.transform.position, radius, Vector2.left, distance);
            hitRight = Physics2D.CircleCast(newPlatform.transform.position, radius, Vector2.right, distance);


            if (hitUp.collider != null || hitDown.collider != null || hitLeft.collider != null || hitRight.collider != null) {
                //Debug.Log("destroy "+ newPlatform.transform.position);
                Destroy(newPlatform);
            }
            else {
                platforms.Add(newPlatform);
            }
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlatformFactory : MonoBehaviour {

    public GameObject platformPrefab;
    public int platformDensity = 50;
    private GameObject player;

    void Start() {
        player = GameObject.Find("Player");
        float jumpForce = player.GetComponent<PlayerController>().jumpForce;

        float yPos = 0f;
        Debug.Log(jumpForce);
        for(int i=0; i<platformDensity; i++) {
            while (yPos+5f < jumpForce/2) {
                yPos = Random.Range(-5f, 5f);
            }
            Instantiate(platformPrefab, new Vector3(Random.Range(-2.5f, 2.5f), yPos), platformPrefab.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update() {
    }
}

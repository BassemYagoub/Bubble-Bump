using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CameraController : MonoBehaviour {

    private GameObject player;
    private float playerYpos;
    private GameObject background;

    void Start() {
        player = GameObject.Find("Player");
        playerYpos = player.transform.position.y;
        background = GameObject.Find("Background");
    }

    // Update is called once per frame
    void Update() {
        if (player.transform.position.y > (background.transform.position.y/2)) {
            transform.position = new Vector3(transform.position.x, transform.position.y +.1f, transform.position.z);
        }
    }
}

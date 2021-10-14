using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CameraController : MonoBehaviour {

    private GameObject player;

    void Start() {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update() {
        if (player != null && player.transform.position.y > transform.position.y) {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
    }
}

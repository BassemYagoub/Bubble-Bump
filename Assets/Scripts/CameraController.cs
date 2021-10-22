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
        if (player != null){
            if(player.transform.position.y > transform.position.y)
                transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
        //Camera moves down when game is over
        else{
            Vector3 target = new Vector3(transform.position.x, 0, transform.position.z);
            float step = 5f * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }
    }
}

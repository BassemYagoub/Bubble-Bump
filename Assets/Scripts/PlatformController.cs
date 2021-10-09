using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    //if player touches platform from below => he can go through
    private void OnCollisionEnter2D(Collision2D collision) {
        //Debug.Log("collision with "+ collision.transform.position.y+" | platform = "+transform.position.y);
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player") && collision.transform.position.y < transform.position.y) {
            //Debug.Log("go through");
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    //if player touches platform from above => he can go on top
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player") && collision.transform.position.y >= transform.position.y) {
            GetComponent<BoxCollider2D>().isTrigger = false;
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            //give him some jump force back because he might lose it sometimes
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Rigidbody2D>().AddForce(Vector3.up * player.jumpForce, ForceMode2D.Impulse);
        }
    }
}

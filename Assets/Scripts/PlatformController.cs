using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {
    private bool movingRight = true; //direction for moving platforms
    public float speed = 2f;

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (gameObject.CompareTag("MovingPlatform"))
            MovePlatform();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && other.transform.position.y >= transform.position.y) {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            
            if(other.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0f){
                other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * player.jumpForce, ForceMode2D.Impulse);
            }
        }
    }


    void MovePlatform() {
        Vector3 target;

        //careful with x value written in "raw"
        if (movingRight)
            target = new Vector3(2.5f, transform.position.y, transform.position.z);
        else
            target = new Vector3(-2.5f, transform.position.y, transform.position.z);

        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        if (transform.position.x == -2.5f || transform.position.x == 2.5f)
            movingRight = !movingRight;
    }
}

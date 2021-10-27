using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {
    public float speed = 2f;
    public Sprite springActivatedSprite;

    void Start() {

    }

    void Update() {

    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Rigidbody2D>().velocity.y <= -2f) {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (gameObject.CompareTag("Spring")) {
                gameObject.GetComponent<AudioSource>().Play();
                gameObject.GetComponent<SpriteRenderer>().sprite = springActivatedSprite;

                gameObject.transform.position = new Vector3(transform.position.x, transform.position.y+0.2f, transform.position.z);
                other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * player.jumpForce*2, ForceMode2D.Impulse);
            }
        }

    }


}

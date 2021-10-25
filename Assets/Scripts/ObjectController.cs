using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {
    public float speed = 2f;


    void Start() {

    }

    void Update() {

    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Rigidbody2D>().velocity.y <= -3.5f) {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            if (gameObject.CompareTag("Spring")) {
                Debug.Log(gameObject.GetComponent<SpriteRenderer>().sprite);
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("spring_activated@2x");
                Debug.Log(gameObject.GetComponent<SpriteRenderer>().sprite);
                other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * player.jumpForce*2, ForceMode2D.Impulse);
            }
        }

    }


}

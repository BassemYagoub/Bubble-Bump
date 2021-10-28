using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {
    public float speed = 2f;
    public Sprite objectActivatedSprite;

    void Start() {

    }

    void Update() {

    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Rigidbody2D>().velocity.y <= -3f && other.gameObject.transform.position.y > transform.position.y + 0.1f) {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            if (gameObject.CompareTag("Spring") ) {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * player.jumpForce * 1.5f, ForceMode2D.Impulse);
                gameObject.GetComponent<AudioSource>().Play();
                gameObject.GetComponent<SpriteRenderer>().sprite = objectActivatedSprite;
                gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z); 
            }
            else if (gameObject.CompareTag("Propeller")) {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * player.jumpForce * 2.5f, ForceMode2D.Impulse);
                other.gameObject.GetComponent<Animator>().SetBool("PropellerActivated", true);
                //StartCoroutine(player.PropellerAnimation());
                Destroy(gameObject);
            }
            else if (gameObject.CompareTag("Jetpack")) {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * player.jumpForce * 4f, ForceMode2D.Impulse);
                other.gameObject.GetComponent<Animator>().SetBool("JetpackActivated", true);
                Destroy(gameObject);
            }
        }

    }


}

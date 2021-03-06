using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {
    public float speed = 2f;
    public Sprite objectActivatedSprite;
    private EnemyFactory enemyFactory;

    void Start() {
        enemyFactory = GameObject.Find("Enemies").GetComponent<EnemyFactory>();
    }

    void Update() {

    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            if (gameObject.CompareTag("Spring") && other.gameObject.GetComponent<Rigidbody2D>().velocity.y <= -1f && other.gameObject.transform.position.y > transform.position.y + 0.1f) {
                other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * player.jumpForce * 1.5f, ForceMode2D.Impulse);
                gameObject.GetComponent<AudioSource>().Play();
                gameObject.GetComponent<SpriteRenderer>().sprite = objectActivatedSprite;
                gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z); 
            }
            else if (gameObject.CompareTag("Propeller")) {
                other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * player.jumpForce * 2.5f, ForceMode2D.Impulse);
                other.gameObject.GetComponent<AudioSource>().PlayOneShot(player.audioClips[0]);
                other.gameObject.GetComponent<Animator>().SetBool("PropellerActivated", true);
                enemyFactory.setBonusIsActive(true); //deactivate enemy factory
                //StartCoroutine(player.PropellerAnimation());
                Destroy(gameObject);
            }
            else if (gameObject.CompareTag("Jetpack")) {
                other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * player.jumpForce * 4f, ForceMode2D.Impulse);
                other.gameObject.GetComponent<AudioSource>().PlayOneShot(player.audioClips[1]);
                other.gameObject.GetComponent<Animator>().SetBool("JetpackActivated", true);
                enemyFactory.setBonusIsActive(true); //deactivate enemy factory
                Destroy(gameObject);
            }
        }

    }


}

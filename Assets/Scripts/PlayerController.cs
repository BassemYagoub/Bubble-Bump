
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour {
    public float speed = 2.5f;
    public float jumpForce = 6.5f;

    private Rigidbody2D playerRb;
    private bool isJumping = false;

    void Start() {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        CheckIfTouchesSide();

        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow)) {
            //if (!gameObject.GetComponent<SpriteRenderer>().sprite.name.Contains("lik-left"))
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("lik-left@2x");
            movement += Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            //if(!gameObject.GetComponent<SpriteRenderer>().sprite.name.Contains("lik-right"))
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("lik-right@2x");
            movement += Vector3.right;
        }

        transform.position += movement * speed * Time.deltaTime;
    }


    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Platform") && collision.transform.position.y > transform.position.y+gameObject.GetComponent<BoxCollider2D>().bounds.size.y) {
            playerRb.velocity = Vector2.zero;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            if (!isJumping) {
                isJumping = true;
                StartCoroutine(JumpAnimation());
            }
            //transform.position += Vector3.up * jumpForce * Time.deltaTime;
        }
    }

    private void CheckIfTouchesSide() {
        if(transform.position.x > 3f) {
            transform.position = new Vector3(-3f, transform.position.y, transform.position.z);
        }

        else if(transform.position.x < -3f) {
            transform.position = new Vector3(3f, transform.position.y, transform.position.z);
        }
    }

    private IEnumerator JumpAnimation() {
        string previousSprite = gameObject.GetComponent<SpriteRenderer>().sprite.name;
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(previousSprite.Substring(0, previousSprite.Length-3) +"-odskok@2x");
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(previousSprite);
        isJumping = false;
    }

}

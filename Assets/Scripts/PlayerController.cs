
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour {
    public float speed = 2.5f;
    public float jumpForce = 6.5f;

    private Rigidbody2D playerRb;

    void Start() {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        CheckIfTouchesSide();

        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
            movement += Vector3.left;
        if (Input.GetKey(KeyCode.RightArrow))
            movement += Vector3.right;

        transform.position += movement * speed * Time.deltaTime;
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Platform")) {
            Debug.Log("Jump");

            playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            //transform.position += Vector3.up * jumpForce * Time.deltaTime;
        }
    }

    private void CheckIfTouchesSide() {
        if(transform.position.x > 3.1f) {
            transform.position = new Vector3(-3.1f, transform.position.y, transform.position.z);
        }

        else if(transform.position.x < -3.1f) {
            transform.position = new Vector3(3.1f, transform.position.y, transform.position.z);
        }
    }

}

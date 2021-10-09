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

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && other.transform.position.y >= transform.position.y)
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            
            if(other.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0f){
                other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * player.jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool movingRight = true; //direction for moving enemies
    private bool tiltingRight = true;
    public float speed = 2f;
    private bool tiltingUp = true;
    private float rightTarget;
    private float leftTarget;
    private float upTarget;
    private float downTarget;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("TiltingEnemy")) {
            rightTarget = transform.position.x + 1;
            leftTarget = transform.position.x - 1;
            upTarget = transform.position.y + 1;
            downTarget = transform.position.y - 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("TiltingEnemy")){
            tiltEnemy();
        }
        else if (gameObject.CompareTag("MovingEnemy")) {
            moveEnemy();
        }
    }

    private void moveEnemy(){
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

    private void tiltEnemy(){
        Vector3 target;
        float tiltDirection = (tiltingUp ? upTarget : downTarget);

        //careful with x value written in "raw"
        if (tiltingRight) {
            target = new Vector3(rightTarget, tiltDirection, transform.position.z);
            tiltingUp = !tiltingUp;
        }

        else {
            target = new Vector3(leftTarget, tiltDirection, transform.position.z);
            tiltingUp = !tiltingUp;
        }

        float step = speed * 2 * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        if (transform.position.x == leftTarget || transform.position.x == rightTarget)
            tiltingRight = !tiltingRight;
    }
}

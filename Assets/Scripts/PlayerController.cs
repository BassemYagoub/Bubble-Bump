
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour {
    public float speed = 2.5f;
    public float projectileSpeed = 10f;
    public float jumpForce = 6.5f;

    private Rigidbody2D playerRb;
    private bool isJumping = false;
    private bool isShooting = false;
    private string previousSprite;

    private GameObject mainCamera;
    private GameObject enemies;
    private GameObject projectiles;
    private Transform jetpackTransform;

    public GameObject projectilePrefab;
    private List<GameObject> projectilesList;
    private int nextUpdate = 1; // Next update in second

    void Start() {
        playerRb = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.Find("Main Camera");
        enemies = GameObject.Find("Enemies");
        projectiles = GameObject.Find("Projectiles");
        jetpackTransform = gameObject.transform.GetChild(1).transform;
        projectilesList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update() {
        CheckIfTouchesSide();

        Vector3 movement = Vector3.zero;
        // If the next update is reached
        if (Time.time >= nextUpdate) {
            // Change the next update (current second+1)
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            //updates every second
            RemoveUnseeableProjectiles();
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            if (!isJumping) {
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("lik-left@2x");
            }
            else {
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("lik-left-odskok@2x");
                previousSprite = "lik-left@2x";
            }

            //reverse jetpack sprite if needed
            if (jetpackTransform.localPosition.x < 0) {
                MirrorJetPack();
            }

            movement += Vector3.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            if (!isJumping) {
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("lik-right@2x");
            }
            else {
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("lik-right-odskok@2x");
                previousSprite = "lik-right@2x";
            }

            //reverse jetpack sprite if needed
            if (jetpackTransform.localPosition.x > 0) {
                MirrorJetPack();
            }

            movement += Vector3.right;
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("piou piou");
            if (!isShooting) {
                isShooting = true;
                StartCoroutine(ShootAnimation());
            }
        }

        transform.position += movement * speed * Time.deltaTime;

        if (gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0f) {
            gameObject.GetComponent<Animator>().SetBool("PropellerActivated", false);
            gameObject.GetComponent<Animator>().SetBool("JetpackActivated", false);
            enemies.GetComponent<EnemyFactory>().setBonusIsActive(false); //reactivate enemy factory
        }

        Vector3 target;

        foreach (GameObject projectile in projectilesList) {
            Debug.Log("projectile");
            target = projectile.transform.position;
            target.y += 50;
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, target, projectileSpeed * Time.deltaTime);
        }
    }


    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Platform") && collision.transform.position.y <= transform.position.y + gameObject.GetComponent<BoxCollider2D>().bounds.size.y) {
            //playerRb.velocity = Vector2.zero;
            //playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            if (!isJumping) {
                isJumping = true;
                StartCoroutine(JumpAnimation());
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<EnemyController>() != null) {
            //player jumping on enemy with tag
            if (!collision.gameObject.CompareTag("Untagged") && collision.transform.position.y <= transform.position.y+0.2f) {
                enemies.GetComponent<EnemyFactory>().enemies.Remove(collision.gameObject);

                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                Destroy(collision.gameObject);
            }
            //game over when player touches enemy
            else {
                mainCamera.GetComponent<GameController>().EndGame();
            }
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
        previousSprite = gameObject.GetComponent<SpriteRenderer>().sprite.name;
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(previousSprite.Substring(0, previousSprite.Length-3) +"-odskok@2x");
        yield return new WaitForSeconds(.45f);
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(previousSprite);
        isJumping = false;
    }

    private IEnumerator ShootAnimation() {
        //previousSprite = gameObject.GetComponent<SpriteRenderer>().sprite.name;
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("lik-puca@2x");
        GameObject shootingMouth = gameObject.transform.Find("ShootingMouth").gameObject;
        shootingMouth.SetActive(true);
        //projectilePrefab
        Vector3 projectilePos = gameObject.transform.position;
        projectilePos.y += 0.5f;
        GameObject projectile = Instantiate(projectilePrefab, projectilePos, projectilePrefab.transform.rotation);
        projectile.transform.SetParent(projectiles.transform);
        projectilesList.Add(projectile);
        yield return new WaitForSeconds(.45f);
        //gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(previousSprite);
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("lik-right@2x");
        shootingMouth.SetActive(false);
        isShooting = false;
    }


    private void MirrorJetPack() {
        Vector3 jetpackPos = jetpackTransform.localPosition;
        jetpackTransform.transform.localPosition = new Vector3((-1) * jetpackPos.x, jetpackPos.y, jetpackPos.z);
        Quaternion jetpackRot = jetpackTransform.rotation;
        jetpackTransform.rotation = new Quaternion(jetpackRot.x, (-1) * jetpackRot.y, jetpackRot.z, jetpackRot.w);
    }

    void RemoveUnseeableProjectiles() {
        float triggerDistance = mainCamera.transform.position.y + (mainCamera.GetComponent<Camera>().orthographicSize * 2);
        GameObject projectileToRemove = null;

        for (int i = projectilesList.Count - 1; i >= 0; i--) {
            if (projectilesList[i].transform.position.y < triggerDistance - 0.5f) {
                projectileToRemove = projectilesList[i];
                projectilesList.RemoveAt(i);
                Destroy(projectileToRemove);
            }
        }
    }
}

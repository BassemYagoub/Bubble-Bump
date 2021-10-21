using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


[DisallowMultipleComponent]
public class GameController : MonoBehaviour {
    public TextMeshProUGUI scoreText;
    private int score;
    private GameObject player;
    private GameObject mainCamera;
    private bool gameOver = false;
    private GameObject GameOverCanvas;
    
    void Start() {
        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
        GameOverCanvas = GameObject.Find("GameOverCanvas");
        GameOverCanvas.SetActive(false);
        score = 0;
    }

    void Update() {
        if (!gameOver) {
            //score management
            int tmpScore = (int)player.transform.position.y * 100;
            if (tmpScore > score) {
                score = tmpScore;
                //scoreText.GetComponent<TextMeshProUGUI>().text = tmpScore.ToString();
            }
            int visibleScore = int.Parse(scoreText.GetComponent<TextMeshProUGUI>().text);
            if (visibleScore < score)
                scoreText.GetComponent<TextMeshProUGUI>().text = (visibleScore + (score - visibleScore) / 5).ToString();

            //game state management
            if (player.transform.position.y < mainCamera.transform.position.y - 5.9f) {
                gameOver = true;
                Destroy(player);
                //Debug.Log("Game Over");
            }
        }
        else {
            GameOverCanvas.SetActive(true);
        }
    }
}

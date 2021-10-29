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
    private GameObject GameOver; 
    private GameObject EndScore;
    
    void Start() {
        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
        GameOver = GameObject.Find("GameOver");
        EndScore = GameOver.transform.Find("EndScore").gameObject;
        GameOver.SetActive(false);
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
                player.GetComponent<AudioSource>().Play();
            }
        }
        else {
            Destroy(player);
            ShowGameOver();
        }
    }

    void ShowGameOver() {
        GameOver.SetActive(true);
        EndScore.GetComponent<TextMeshProUGUI>().text = "your score: " + scoreText.GetComponent<TextMeshProUGUI>().text;
    }

    public void EndGame() {
        gameOver = true;
    }
}

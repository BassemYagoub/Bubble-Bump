using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public class GameController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        int tmpScore = (int)player.transform.position.y * 100;
        if (tmpScore > score) {
            score = tmpScore;
            scoreText.GetComponent<TextMeshProUGUI>().text = tmpScore.ToString();
        }
            
    }
}

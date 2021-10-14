using UnityEngine;
using UnityEngine.SceneManagement;



public class UIController : MonoBehaviour
{
    public void launchGame() {
        SceneManager.LoadScene("MainScene");
    }

    public void quitGame() {
        Application.Quit();   
    }
}

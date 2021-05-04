using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
 
    public void loadLevel()
    {
        StartCoroutine(GameManager.Instance.loadingScreen.LoadSceneWithLoadScreen(SceneManager.GetActiveScene().buildIndex + 1));
    }


    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}




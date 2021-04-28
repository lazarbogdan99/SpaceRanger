using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // public Animator transition;
    // public float transitionTime;

    /** public void Update()
     {
         if (Input.GetMouseButtonDown(0))
         {
             loadLevel();
         }
     } 

     IEnumerator LoadLevel()
     {
         transition.SetTrigger("Start");
         yield return new WaitForSeconds(transitionTime);
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    } **/

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




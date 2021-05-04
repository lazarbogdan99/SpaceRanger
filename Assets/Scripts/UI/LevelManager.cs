using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// Level manager is reponsible for quick scene switching and displaying the in-game UI in case of winning or losing so the player
// can go to next level
public class LevelManager : MonoBehaviour
{

    [SerializeField] private string onPlayerDeathText, onPlayerWinText, onGameFinishText;
    [SerializeField] private string finalLevelName = "Level3";

    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private Button _nextLevelButton;

    public void NextLevel()
    {
        // If this is not the last level
        // I load the next level using the Loading Screen system, which does the fade in fade out effects and shows the background image and text
        if (SceneManager.GetActiveScene().buildIndex < GameManager.Instance.sceneCount - 1)
        {
            StartCoroutine(GameManager.Instance.loadingScreen.LoadSceneWithLoadScreen(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    public void RestartLevel()
    {
        // retarting a level doesn't require redoing the background and text story, I just restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // This is the function to call when wanting to enable the UI menu
    public void Transition(bool lost = true)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        // I do the audio lowering effect while active
        StartCoroutine(GameManager.Instance.audioManager.AudioLowerEffect());

        // In case I lost the level, I disable he next level button since I don't want the player to go to next level if he
        // Didn't beat the current one
        if (lost)
        {
            _titleText.text = onPlayerDeathText;
            _nextLevelButton.interactable = false;
        }
        // If this is the final level, meaning there's no (next level) I disable the button as well since I don't want to load a non-existing level
        else if (SceneManager.GetActiveScene().name == finalLevelName)
        {
            _nextLevelButton.interactable = false;
            _titleText.text = onGameFinishText;
        }
        else
            _titleText.text = onPlayerWinText;
    }
}

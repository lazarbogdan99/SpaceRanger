using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region Variables

    [SerializeField] private int mainMenuSceneIndex;
    [SerializeField] [Range(0.0f, 1.0f)] private float freezeRate = 0.0f;

    private ScaleUpAnimation scaleUpAnimation;

    #endregion

    private void OnEnable()
    {
        scaleUpAnimation = GetComponent<ScaleUpAnimation>();
        scaleUpAnimation.OnAnimationFinishedEvent += OnAnimationFinished;
    }

    private void OnAnimationFinished()
    {
        Time.timeScale = freezeRate;
    }

    private void OnDisable()
    {
        scaleUpAnimation.OnAnimationFinishedEvent -= OnAnimationFinished;
        Time.timeScale = 1.0f;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(GameManager.Instance.loadingScreen.LoadSceneWithLoadScreen(mainMenuSceneIndex));
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

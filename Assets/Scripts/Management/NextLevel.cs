using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private ScoreSystem _scoreSystem;
    [SerializeField] private bool skipUI = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("won");
            _scoreSystem.SaveScore(SceneManager.GetActiveScene());
            if (skipUI)
            {
                int scene = SceneManager.GetActiveScene().buildIndex;
                StartCoroutine(GameManager.Instance.loadingScreen.LoadSceneWithLoadScreen(scene + 1));
            }
            else
                _levelManager.Transition(false);
        }
    }
}

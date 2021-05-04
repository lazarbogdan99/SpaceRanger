using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// This is the system used to load the levels in the main menu so the player can pick a level of his choice and play it
// instead of replaying the whole game
// It simply gets,data from the save data system, all the levels the player played,
//then from these I can guess what was the last level the player
// Unlocked, and I spawn buttons based on that, while using the loading screen effect to load the level
public class LoadLevel : MonoBehaviour
{
    [SerializeField] private GameObject buttonTemplate;
    [SerializeField] private GameObject panel;

    private void OnEnable()
    {
        var currentUser = GameManager.Instance.CurrentUser;
        int highestValue = 0;
        foreach (var level in currentUser.levelScores)
        {
            var levelButtonObject = Instantiate(buttonTemplate, panel.transform);
            levelButtonObject.transform.SetParent(panel.transform);
            var levelButton = levelButtonObject.GetComponent<Button>();
            levelButtonObject.GetComponentInChildren<TMP_Text>().text = level.levelName;
            string n = string.Empty;
            n += level.levelName[level.levelName.Length - 1];
            int levelNumber = int.Parse(n);
            levelButton.onClick.AddListener(delegate
            {
                StartCoroutine(GameManager.Instance.loadingScreen.LoadSceneWithLoadScreen(levelNumber + 1));
            });
            highestValue = levelNumber + 1;
        }
        if (highestValue == 4) return;
        if (highestValue == 0) highestValue = 1;
        LevelData ld = new LevelData();
        ld.levelName = $"Level{highestValue}";
        AddLevel(ld);
    }

    private void AddLevel(LevelData level)
    {
        var levelButtonObject = Instantiate(buttonTemplate, panel.transform);
        levelButtonObject.transform.SetParent(panel.transform);
        var levelButton = levelButtonObject.GetComponent<Button>();
        levelButtonObject.GetComponentInChildren<TMP_Text>().text = level.levelName;
        string n = string.Empty;
        n += level.levelName[level.levelName.Length - 1];
        int levelNumber = int.Parse(n);
        levelButton.onClick.AddListener(delegate
        {
            SceneManager.LoadScene(levelNumber + 1);
        });
    }


    private void OnDisable()
    {
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            Destroy(panel.transform.GetChild(i).gameObject);
        }
    }
}

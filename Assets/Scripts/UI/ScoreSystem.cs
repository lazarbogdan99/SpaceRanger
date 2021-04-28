using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private Timer _timer;
    private int score;

    private int totalScore;

    // Properties
    public int Score
    {
        get { return score; }

        set
        {
            // We make sure we don't set negative score by accident
            if (value >= 0)
            {
                score = value;
                // We update the text after updating the score's value
                // This way we don't have to update the text in the Update() method
                // Which is heavy on CPU
                UpdateScoreText();
            }
        }
    }

    private void Start()
    {
        // We init the text to "Score: 0"
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.SetText($"Score: {score}");
    }

    // This is the function to be called whenever we want to update the score in the file
    public void SaveScore(Scene scene)
    {
        // we have a local boolean to check if the level is new or we already played it
        var levelExists = false;
        // We calculate the score based on the provided formula
        // We cast it to int since we don't want score to be something like 16.523 for example
        totalScore = (int)(long)(_timer.CurrentTime * score + 100);

        // We loop through all of the levels the player has unlocked
        foreach (var i in GameManager.Instance.CurrentUser.levelScores)
        {
            // if name matches, we hit the nail
            // we already played this level, check if we got higher score
            if (i.levelName == scene.name)
            {
                // We had a better play this time, update the score on the file
                if (i.levelScore < totalScore)
                    i.levelScore = totalScore;
                levelExists = true;
                break;
            }
        }
        if (!levelExists)
        {
            // If it's a new level, we create a new one
            // and add it
            var levelData = new LevelData();
            levelData.levelName = scene.name;
            levelData.levelScore = totalScore;
            var currentUser = GameManager.Instance.CurrentUser;
            currentUser.levelScores.Add(levelData);
        }
        // We have to save our changes
        GameManager.Instance.Save();
    }
}

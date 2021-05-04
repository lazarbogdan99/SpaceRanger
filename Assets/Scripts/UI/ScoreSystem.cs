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
            // I make sure I don't set negative score by accident
            if (value >= 0)
            {
                score = value;
                // I update the text after updating the score's value
                // This way I don't have to update the text in the Update() method
                // Which is heavy on CPU
                UpdateScoreText();
            }
        }
    }

    private void Start()
    {
        // I init the text to "Score: 0"
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.SetText($"Score: {score}");
    }

    // This is the function to be called whenever I want to update the score in the file
    public void SaveScore(Scene scene)
    {
        // I have a local boolean to check if the level is new or I already played it
        var levelExists = false;
        // I calculate the score based on the provided formula
        // I cast it to int since I don't want score to be something like 16.523 for example
        totalScore = (int)(long)(_timer.CurrentTime * score + 100);

        // I loop through all of the levels the player has unlocked
        foreach (var i in GameManager.Instance.CurrentUser.levelScores)
        {
            // if name matches, I hit the nail
            // I already played this level, check if I got higher score
            if (i.levelName == scene.name)
            {
                // I had a better play this time, update the score on the file
                if (i.levelScore < totalScore)
                    i.levelScore = totalScore;
                levelExists = true;
                break;
            }
        }
        if (!levelExists)
        {
            // If it's a new level, I create a new one
            // and add it
            var levelData = new LevelData();
            levelData.levelName = scene.name;
            levelData.levelScore = totalScore;
            var currentUser = GameManager.Instance.CurrentUser;
            currentUser.levelScores.Add(levelData);
        }
        // I have to save our changes
        GameManager.Instance.Save();
    }
}

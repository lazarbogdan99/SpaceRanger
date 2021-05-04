using System;
using System.Collections.Generic;

// I mark the class as Serializable
// So that I can write it into a file
[Serializable]
public class User
{
    private long _totalScore = 0;

    public int id;

    // The username of the player
    public string username;


    //public long totalScore = ;
    public long TotalScore
    {
        get
        {
            _totalScore = 0;
            foreach (var level in levelScores)
                _totalScore += level.levelScore;
            return _totalScore;
        }
    }

    // I will store all player scores here, and map them accordingly
    public List<LevelData> levelScores;

    // Constructor    
    public User(int id = 0, string username = "default", int numberOfLevels = 1)
    {
        this.id = id;
        this.username = username;

        // here I make sure the Array has the same size as the number of levels I have
        // I made it into a variable in case I wanted change the number of levels in the future
        // which should be easy
        // Plus I're zeroing all of the values inside it since it's a new player
        levelScores = new List<LevelData>(numberOfLevels);
    }
}
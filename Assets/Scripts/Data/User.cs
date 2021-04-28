using System;
using System.Collections.Generic;

// We mark the class as Serializable
// So that we can write it into a file
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

    // We will store all player scores here, and map them accordingly
    public List<LevelData> levelScores;

    // Constructor    
    public User(int id = 0, string username = "default", int numberOfLevels = 1)
    {
        this.id = id;
        this.username = username;

        // here we make sure the Array has the same size as the number of levels we have
        // we made it into a variable in case we wanted change the number of levels in the future
        // which should be easy
        // Plus we're zeroing all of the values inside it since it's a new player
        levelScores = new List<LevelData>(numberOfLevels);
    }
}
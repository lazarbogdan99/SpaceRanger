using System.IO;
using UnityEngine;
using System.Linq;

/**
 * How does it work?
 *      It first loads data from a file (save.json) if it's empty, the system creates a new users variable for holding the upcoming users
 *      Else it loads the data from the JSON file (Unity's system) and stores it into the users variable
 */
public class UserManager
{

    // The path to the file we're saving
    public static string filePath = Application.dataPath + "/save.json";
    public Users users;


    public void Save()
    {
        // Unity does the magic of transforming the class into a JSON text file
        var jsonString = JsonUtility.ToJson(users, true);

        // We write to the file now
        File.WriteAllText(filePath, jsonString);
    }

    // We load the data from the file
    public Users Load()
    {
        // If the file doesn't exist / hasn't been created yet
        // we return nothing
        if (!File.Exists(filePath))
            return null;
        // We load the JSON text from the file
        var jsonString = File.ReadAllText(filePath);

        // Check if the users variable is null (just started the game) and create a new object
        if (users is null)
            users = new Users();
        // Unity then does the magic of transforming from JSON to our actual class
        JsonUtility.FromJsonOverwrite(jsonString, users);
        return users;
    }

    public User AddUser(User u)
    {
        if (users != null && users.users.Count > 0)
        {
            for (int i = 0; i < users.users.Count; i++)
            {
                // user already exists, we exit
                if (users.users[i].username.CompareTo(u.username) == 0)
                {
                    return users.users[i];
                }
            }
            users.users.Add(u);
            users.users.OrderBy(user => user.id);
            Save();
        }
        else
        {
            users = new Users();
            var defaultUser = new User(0);
            users.users.Add(defaultUser);
            users.users.Add(u);
            users.users.OrderBy(user => user.id);
            Save();
        }
        return u;

    }
}

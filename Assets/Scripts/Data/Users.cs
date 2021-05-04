using System.Collections.Generic;

// Since Unity cannot directly write / Serialize
// Arrays of objects, I need to write a wrapper class
// That has a field with all necessary data to write
[System.Serializable]
public class Users
{
    public List<User> users;

    public Users(int size = 1)
    {
        this.users = new List<User>(size);
    }
}

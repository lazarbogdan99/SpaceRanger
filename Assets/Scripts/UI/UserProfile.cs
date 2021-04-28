using UnityEngine;


public class UserProfile : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField userInputField;
    [SerializeField] private int mainMenuBuildIndex = 1;

    public void Save()
    {
        if (userInputField.text.CompareTo("") == 0)
        {
            userInputField.placeholder.GetComponent<TMPro.TMP_Text>().text = "Username cannot be empty";
            return;
        }
        GameManager.Instance.AddUser(userInputField.text);
        StartCoroutine(GameManager.Instance.loadingScreen.LoadSceneWithLoadScreen(mainMenuBuildIndex));
    }
}

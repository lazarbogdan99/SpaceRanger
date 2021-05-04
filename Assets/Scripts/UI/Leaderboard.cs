using System.Linq;
using UnityEngine;
using TMPro;


/**
 * Leaderboard:
 *      How does it work:
 *          The System first waits for the Scale Up Animation script to finish, 
 *          then gets the list of all players from the game manager and orderes them
 *          by total score in a decending order, 
 *          next it loops through that list of players (skipping the player named default since it's for debugging) and creates
 *          a UI object, based on the provided template, 
 */

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject _userTemplate;
    [SerializeField] private GameObject _contentObject;

    private ScaleUpAnimation _animation;

    private TMP_Text usernameText, level1Text, level2Text, level3Text, totalText;
    private RectTransform _userContainerObject;

    private void Start()
    {
        _animation = GetComponent<ScaleUpAnimation>();
        _animation.OnAnimationFinishedEvent += EnableObjectsAnimation;
    }

    private void SetTexts(GameObject template)
    {
        // Retrieving the objects from the game object to use for filling the each player's data
        usernameText = template.transform.Find("UsernameText").GetComponent<TMP_Text>();
        _userContainerObject = template.transform.Find("Container").GetComponent<RectTransform>();
        level1Text = _userContainerObject.transform.Find("Level1Text").GetComponent<TMP_Text>();
        level2Text = _userContainerObject.transform.Find("Level2Text").GetComponent<TMP_Text>();
        level3Text = _userContainerObject.transform.Find("Level3Text").GetComponent<TMP_Text>();
        totalText = _userContainerObject.transform.Find("TotalText").GetComponent<TMP_Text>();
    }

    private void EnableObjectsAnimation()
    {
        // Getting the list of players from the Game Manager + ordering them by total score
        var players = GameManager.Instance.Players.OrderByDescending(player => player.TotalScore).ToList();
        foreach (var user in players)
        {
            // if the player is default, I skip since I're not interested
            if (user.username.CompareTo("default") == 0) continue;

            // I check if by any chance the player in the leaderboard already exists and I destroy it,
            //this is to make sure data is updated
            var @obj = _contentObject.transform.Find(user.username);
            if (@obj) Destroy(@obj.gameObject);

            // I instantiate an object based on a template
            var player = Instantiate(_userTemplate);

            // Filling data
            player.name = user.username;
            player.SetActive(true);
            player.transform.SetParent(_contentObject.transform);

            // making sure scale is 1 so I don't face size issues
            player.GetComponent<RectTransform>().localScale = Vector3.one;
            SetTexts(player);
            usernameText.text = user.username;

            // Checks, seeing levels completed by the player
            if (user.levelScores.Count > 0)
                level1Text.text = user.levelScores[0].levelScore.ToString();
            else
            {
                // There's no point in continuing if the user hasn't won even the first level
                level1Text.text = "0";
                level2Text.text = "0";
                level3Text.text = "0";
                totalText.text = "0";
                continue;
            }

            if (user.levelScores.Count > 1)
            {
                level2Text.text = user.levelScores[1].levelScore.ToString();
            }
            else
                level2Text.text = "0";

            if (user.levelScores.Count > 2)
            {
                level3Text.text = user.levelScores[2].levelScore.ToString();
            }
            else
                level3Text.text = "0";
            totalText.text = user.TotalScore.ToString();
        }
    }
}

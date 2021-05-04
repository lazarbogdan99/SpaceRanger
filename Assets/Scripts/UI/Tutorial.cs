using System.Collections;
using UnityEngine;
using TMPro;

/**
 * How does it work?
 *      The system enables a GUI text object when the player enters the trigger, 
 *      it waits for the scale up animation then does the type writing effect (similar to loading
 *      screen one) while having the player disabled
 */

[RequireComponent(typeof(BoxCollider2D))]
public class Tutorial : MonoBehaviour
{
    #region Variables

    [SerializeField] private TMP_Text UI_Text;
    [SerializeField] private GameObject parentObject;
    [SerializeField] private float textAnimationSpeed;
    [SerializeField] private bool playOnStart;


    private string text;
    private int index;
    private ScaleUpAnimation _animation;
    private bool finishedAnimation;

    #endregion

    private void Start()
    {
        text = UI_Text.text;
        UI_Text.text = "";
        parentObject.SetActive(false);

        // Observer pattern for loading screen, I notify when the fade in effect is done,
        //so I don't trigger UI or any other effects while the screen is white (or black)
        LoadingScreen.OnLoadScreenFinishEvent += OnLoadScreenFinish;
    }

    private void OnEnable()
    {
        _animation = parentObject.GetComponent<ScaleUpAnimation>();
        // The Scale up animation, another observer pattern to notify us when the scale up animation is done playing
        _animation.OnAnimationFinishedEvent += OnScaleUpAnimationFinish;
    }

    private void OnDestroy()
    {
        // make sure to unsubscribe from both events when destroyed to prevent null exceptions
        LoadingScreen.OnLoadScreenFinishEvent -= OnLoadScreenFinish;
        _animation.OnAnimationFinishedEvent -= OnScaleUpAnimationFinish;
    }

    private void OnScaleUpAnimationFinish()
    {
        finishedAnimation = true;
        Time.timeScale = 0;
    }

    private void OnLoadScreenFinish()
    {
        if (playOnStart) StartCoroutine(DisplayUIText());

    }

    private IEnumerator DisplayUIText()
    {
        parentObject.SetActive(true);
        while (!finishedAnimation)
            yield return null;
        Time.timeScale = 0;
        while (index < text.Length)
        {
            UI_Text.text += text[index];
            index++;
            yield return new WaitForSecondsRealtime(textAnimationSpeed / text.Length);
        }

        do
        {
            yield return null;
        } while (!Input.anyKey);

        Time.timeScale = 1;
        finishedAnimation = false;
        Destroy(parentObject);
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playOnStart) return;
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(DisplayUIText());
        }
    }
}

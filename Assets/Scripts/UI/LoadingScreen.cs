using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * LoadingScreen:
 *      This system is made for showing load screens between levels, and for some fade screen effects
 *      How does it work:
 *          The system first plays a fade in effect at the start of each level.
 *          Checks if the level (a scriptable object called LevelData) shows a background and story text in between levels,
 *          if it does then before the level loads
 *          It does a typing text animation on the story text using a simple coroutine while showing the background
 *          
 */
public class LoadingScreen : MonoBehaviour
{
    #region Variables

    private static LoadingScreen _instance;

    public delegate void OnLoadScreenFinish();
    public static event OnLoadScreenFinish OnLoadScreenFinishEvent;

    public LoadScreenData currentScene;

    [SerializeField] private LoadScreenData[] loadingScreens;
    [SerializeField] private GameObject textObject, backgroundImageObject;
    [SerializeField] private bool isDebugginSkipLoadScreen = true;
    [SerializeField] private bool warpAtFinalScene = false;

    private Image backgroundImage;
    private TMP_Text storyText;
    private CanvasGroup _canvasGroup;


    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);

        _instance = this;

        storyText = textObject.GetComponent<TMP_Text>();
        backgroundImage = backgroundImageObject.GetComponent<Image>();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        _canvasGroup = GetComponentInChildren<CanvasGroup>();
        loadingScreens.OrderBy(loadingScreen => loadingScreen.buildIndex);


        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            // Each time a level loads, I fade in and set the current scene object (it refers to the loading screen scriptable data object)
            OnSceneLoaded();
            if (warpAtFinalScene && scene.buildIndex == GameManager.Instance.sceneCount - 1)
                // If I want to warp and I're at the last level, I go back to the main menu
                StartCoroutine(LoadSceneWithLoadScreen(1));
        };
        OnSceneLoaded();
    }

    private void OnSceneLoaded()
    {
        // I get the current loading screen object when the scene loads
        var u = (from loadingScreen in loadingScreens
                 where loadingScreen.buildIndex == SceneManager.GetActiveScene().buildIndex
                 select loadingScreen)?.ToArray();
        if (u.Length > 0)
            currentScene = u[0];
        if (!currentScene)
            currentScene = ScriptableObject.CreateInstance<LoadScreenData>();

        backgroundImageObject.SetActive(false);
        textObject.SetActive(false);
        StartCoroutine(TriggerFadeIn(true));
    }

    private IEnumerator TriggerFadeIn(bool triggerEvent = false)
    {
        StopCoroutine(TriggerFadeOut());
        var time = 0.0f;
        while (time < currentScene.fadeInDuration)
        {
            _canvasGroup.alpha = 1 - (time / currentScene.fadeInDuration);
            time += Time.deltaTime;
            yield return null;
        }
        _canvasGroup.alpha = 0;
        if (triggerEvent && OnLoadScreenFinishEvent != null)
            OnLoadScreenFinishEvent();
    }

    private IEnumerator TriggerFadeOut()
    {
        StopCoroutine(TriggerFadeIn());
        var time = 0.0f;
        while (time < currentScene.fadeOutDuration)
        {
            _canvasGroup.alpha = time / currentScene.fadeOutDuration;
            time += Time.deltaTime;
            yield return null;
        }
        _canvasGroup.alpha = 1;
    }

    public IEnumerator LoadSceneWithLoadScreen(int buildIndex)
    {
        // stop any fade in / fade out effect already running
        StopCoroutine(TriggerFadeIn());
        StopCoroutine(TriggerFadeOut());


        // I wait for a fade out to finish
        yield return TriggerFadeOut();

        // I get the current loading screen object from the list provided in the editor
        var @list = (from loadScreen in loadingScreens where loadScreen.buildIndex == buildIndex select loadScreen).ToArray();
        currentScene = @list?[0];

        // if the level data wants to show background and story... and I're not debugging
        // It does the animation and effect
        if (currentScene.showLoadScreenWithFade && !isDebugginSkipLoadScreen)
        {
            backgroundImageObject.SetActive(true);
            textObject.SetActive(true);
            backgroundImage.sprite = currentScene.backgroundImage;
            storyText.text = "";

            // I fake loading by triggering a fade in effect while having the background enabled
            yield return TriggerFadeIn();
            int index = 0;
            // The type writing effect, I add one character at a time to the UI text over a periode of time (provided in the editor)

            // NEW: Story effect can be skipped by pressing any key during the animation

            bool hasPressedKey = false;
            while (index < currentScene.storyDescription.Length)
            {
                storyText.text += currentScene.storyDescription[index];
                index++;
                yield return new WaitForSeconds(currentScene.loadScreenWaitTime / currentScene.storyDescription.Length);
                if (Input.anyKey)
                {
                    storyText.text = currentScene.storyDescription;
                    hasPressedKey = true;
                    break;
                }
            }
            if (!hasPressedKey)
            {
                storyText.text += "\n\n- Pres Any Key -\n\n";

                // here I wait for the player to input a key
                do
                {
                    hasPressedKey = Input.anyKeyDown;
                    yield return null;
                } while (hasPressedKey == false);

            }

            yield return TriggerFadeOut();
            backgroundImageObject.SetActive(false);
            textObject.SetActive(false);
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);
        // Here, in case the level had some time overload while loading or anything strange happened
        // I make sure the screen stays white until the level is loaded, which then will trigger a fade in
        while (!operation.isDone)
        {
            yield return null;
        }

    }
}

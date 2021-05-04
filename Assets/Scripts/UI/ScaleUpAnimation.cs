using UnityEngine;
using TMPro;

public class ScaleUpAnimation : MonoBehaviour
{

    // Variables to control the UI animation
    public delegate void OnAnimationFinished();
    public event OnAnimationFinished OnAnimationFinishedEvent;

    [SerializeField] private bool _verticalAnimation, _horizontalAnimation;
    [SerializeField] private float speed = 30.0f;
    [SerializeField] private bool doSlowMotion = false;
    [SerializeField] private bool permanentSlowMotion = false;
    [SerializeField] private bool disablePlayer = true;


    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private GameObject _backgroundImage;
    [SerializeField] private RectTransform _animatedObject;
    [SerializeField] private GameObject _postAnimationElements;


    private bool finishedAnimation = false;
    private GameObject _player;

    private void OnDisable()
    {
        if (_player != null)
            _player.SetActive(true);
        Time.timeScale = 1.0f;
        finishedAnimation = false;
    }

    private void OnEnable()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
            _player.SetActive(!disablePlayer);
        if (doSlowMotion)
            Time.timeScale = 0.1f;
        finishedAnimation = false;

        // I first make sure that all the buttons are turned off / aren't active so I can
        // Do a nice animation
        _postAnimationElements.SetActive(false);

        var localScale = _animatedObject.localScale;
        if (_horizontalAnimation) localScale.x = 0;
        if (_verticalAnimation) localScale.y = 0;

        _animatedObject.localScale = localScale;

        if (_titleText != null)
            _titleText.gameObject.SetActive(false);
        if (_backgroundImage != null)
            _backgroundImage.gameObject.SetActive(false);
    }

    public void UIAnimation()
    {
        var localScale = _animatedObject.localScale;

        if (localScale.x < 1)
            localScale.x += speed * Time.unscaledDeltaTime;

        if (localScale.y < 1)
            localScale.y += speed * Time.unscaledDeltaTime;

        _animatedObject.localScale = localScale;

        // I only finish if the object has reached the desired size
        if (_animatedObject.localScale.x >= 1 && _animatedObject.localScale.y >= 1)
        {
            Time.timeScale = (doSlowMotion && permanentSlowMotion) ? 0.1f : 1.0f;
            finishedAnimation = true;
            _animatedObject.localScale = Vector3.one;
            _postAnimationElements.SetActive(true);

            if (_titleText != null)
                _titleText.gameObject.SetActive(true);
            if (_backgroundImage != null)
                _backgroundImage.gameObject.SetActive(true);
        }
    }

    public void EnableUI()
    {
        // The animation is done, there's nothing to do
        if (finishedAnimation) return;
        // I ensure the gameObject holding the script is enabled
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        UIAnimation();

        OnAnimationFinishedEvent?.Invoke();
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;
        EnableUI();
    }
}

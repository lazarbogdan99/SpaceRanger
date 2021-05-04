using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _audioManager;

    [SerializeField]
    private AudioClip menuMusic, backgroundMusic, bonusAudio, onDeathSound, spawnSound;
    [SerializeField]
    private GameObject backgroundSource, effectsSource;

    [SerializeField] private float timeToReachMaxAudio, timeToReachZeroAudio;
    [SerializeField] [Range(0.0f, 1.0f)] private float minAudio, maxAudio;

    private AudioSource _backgroundAudioSource, _effectsAudioSource;

    private void OnEnable()
    {
        if (_audioManager != null && _audioManager != this)
        {
            Destroy(gameObject);
            return;
        }

        _audioManager = this;
        DontDestroyOnLoad(gameObject);

        _effectsAudioSource = effectsSource.GetComponent<AudioSource>();
        _backgroundAudioSource = backgroundSource.GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
        _backgroundAudioSource.volume = 0;

        transform.SetParent(GameManager.Instance.transform);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (_audioManager == this)
            _audioManager = null;
        Destroy(gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (_audioManager != this) return;

        // I play the audio source based on the level I're at, background music for the levels and menu music for menu levels
        if (scene.buildIndex < 2)
        {
            if (_backgroundAudioSource.clip != menuMusic)
            {
                _backgroundAudioSource.clip = menuMusic;
                _backgroundAudioSource.Play();

                // Do that cool audio effect
                StartCoroutine(AudioRaisingEffect());
            }
        }
        else
        {
            _backgroundAudioSource.clip = backgroundMusic;
            _backgroundAudioSource.Play();
            StartCoroutine(AudioRaisingEffect());
        }
    }

    private IEnumerator AudioRaisingEffect()
    {
        float time = 0;
        // If the total time of the effect is small, I skip the effect, meaning I don't want it
        if (timeToReachMaxAudio <= 0.005f) _backgroundAudioSource.volume = 1;
        else
        {
            // While I still didn't reach total time
            while (time <= timeToReachMaxAudio)
            {
                // A simple formula
                // maxAudio         <====> maxTime
                // x(currentAudio)  <====> time
                // x(currentAudio) = (time * maxAudio) / maxTime(timeToReachMaxAudio)
                _backgroundAudioSource.volume = time * maxAudio / timeToReachMaxAudio;

                // I increament the timer
                time += Time.deltaTime;
                // this is put so I don't overload the CPU and make Unity crash
                yield return null;
            }
            // I clamp the audio to the max audio
            _backgroundAudioSource.volume = maxAudio;
        }
    }
    public IEnumerator AudioLowerEffect()
    {
        float time = 0;
        // Same as raising effect but in reverse
        if (timeToReachZeroAudio <= 0.005f) _backgroundAudioSource.volume = minAudio;
        else
        {
            while (_backgroundAudioSource.volume > minAudio)
            {
                _backgroundAudioSource.volume = maxAudio - (time / timeToReachZeroAudio);
                time += Time.deltaTime;
                yield return null;
            }
            _backgroundAudioSource.volume = minAudio;
        }
    }

    public void PlayBonusSound()
    {
        _effectsAudioSource.clip = bonusAudio;
        _effectsAudioSource.Play();
    }
    public void PlayDeathSound()
    {
        _effectsAudioSource.clip = onDeathSound;
        _effectsAudioSource.Play();
    }
    public void PlaySpawnSound()
    {
        _effectsAudioSource.clip = spawnSound;
        _effectsAudioSource.Play();
    }
}

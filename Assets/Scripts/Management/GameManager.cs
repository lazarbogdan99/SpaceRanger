using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

// Singleton class for game management
public class GameManager : MonoBehaviour
{
    // Singleton pattern
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    // a reference to keep the id unique for each new player
    private int nextId = 0;
    private UserManager _userManager;

    // who is the current player?
    // So I don't have to search for the player each time I need to do an update
    private User _currentUser;

    private PauseMenu _pauseMenu;

    [SerializeField]
    private GameObject audioManagerPrefab;

    [SerializeField]
    private GameObject loadingScreenPrefab;

    [SerializeField]
    private GameObject settingsManagerPrefab;

    [SerializeField]
    private GameObject pauseMenuPrefab;

    [SerializeField]
    private KeyCode pauseKey;

    public System.Collections.Generic.List<User> Players => _userManager.users.users;

    // A property for the current player
    public User CurrentUser
    {
        get
        {
            if (_currentUser != null)
            {
                return _currentUser;
            }
            else
            {
                _currentUser = GetUserByName("default");
                return _currentUser;
            }
        }
        set { _currentUser = value; }
    }

    [HideInInspector] public AudioManager audioManager;
    [HideInInspector] public LoadingScreen loadingScreen;
    [HideInInspector] public SettingsManager settingsManager;

    [HideInInspector] public int sceneCount;

    private void InitData()
    {
        // I initialize the variables here
        _userManager = new UserManager();
        _userManager.users = _userManager.Load();
        // I make sure if the list isn't empty, I grab the biggest id
        // else I starting new
        if (_userManager.users != null && _userManager.users.users.Count > 0)
            nextId = _userManager.users.users[_userManager.users.users.Count - 1].id + 1;
        else
        {
            _userManager.users = new Users();
            _userManager.Save();
            nextId = 1;
        }

        sceneCount = SceneManager.sceneCountInBuildSettings;
        _pauseMenu = FindObjectOfType<PauseMenu>();
    }

    private void Awake()
    {
        // making sure there is only one instance of the game manager
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => { InitAudioManager(); };

        InitData();
        InitAudioManager();
        InitLoadScreenManager();
        InitSettingsManager();
        OrderUsers();

        DontDestroyOnLoad(gameObject);
    }
    private void InitAudioManager()
    {
        // I already got a reference to our AudioManager, go back
        if (audioManager != null) return;

        // Find out if I, by any chance already have an AudioManager in the scene and get a reference to it
        audioManager = FindObjectOfType<AudioManager>();

        // Seems like I have to do some manual labour
        if (!audioManager)
        {
            var @object = Instantiate(audioManagerPrefab, Vector3.zero, Quaternion.identity);
            @object.name = "AudioManager";
            audioManager = @object.GetComponent<AudioManager>();
        }
    }

    private void InitLoadScreenManager()
    {
        if (loadingScreen != null) return;
        loadingScreen = FindObjectOfType<LoadingScreen>();

        if (!loadingScreen)
        {
            var @object = Instantiate(loadingScreenPrefab, Vector3.zero, Quaternion.identity);
            @object.name = "LoadingScreen";
            loadingScreen = @object.GetComponent<LoadingScreen>();
        }
    }
    private void InitSettingsManager()
    {
        // I already got a reference to our SettingsManager, go back
        if (settingsManager != null) return;

        // Find out if I, by any chance already have an SettingsManager in the scene and get a reference to it
        settingsManager = FindObjectOfType<SettingsManager>();

        // Seems like I have to do some manual labour
        if (!settingsManager)
        {
            var @object = Instantiate(settingsManagerPrefab, Vector3.zero, Quaternion.identity);
            @object.name = "SettingsManager";
            settingsManager = @object.GetComponent<SettingsManager>();
        }
    }

    private void OrderUsers()
    {
        // I order the users list based on their id
        if (_userManager.users != null)
            _userManager.users.users = (from user in _userManager.users.users
                                        orderby user.id
                                        select user).ToList();
    }

    public void AddUser(string username)
    {
        User u = GetUserByName(username);
        if (u == null)
        {
            u = new User(username: username)
            {
                id = nextId++
            };
        }
        _currentUser = _userManager.AddUser(u);
    }

    public User GetUserByName(string username)
    {
        foreach (var u in _userManager.users.users)
        {
            if (username.CompareTo(u.username) == 0)
                return u;
        }
        return null;
    }
    public void Save()
    {
        _userManager.Save();
    }

    public void Update()
    {
        if (Input.GetKeyDown(pauseKey) && loadingScreen.currentScene.buildIndex > 1)
        {
            if (_pauseMenu == null)
            {
                var go = Instantiate(pauseMenuPrefab, Vector3.zero, Quaternion.identity);
                go.name = "PauseMenuCanvas";
                _pauseMenu = go.GetComponent<PauseMenu>();
                _pauseMenu.gameObject.SetActive(true);
            }
            else
            {
                _pauseMenu.gameObject.SetActive(!_pauseMenu.gameObject.activeSelf);
            }
        }
    }
}
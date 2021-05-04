using UnityEngine;
using System.IO;


/**
 * Settings manager, a script that controls and holds global values for game settings, currently holds volume and resolution
 * How does it work: I hold a reference to a settings data object, 
 * I save / load data into that object and manipulate its variables at runtime
 */
public class SettingsManager : MonoBehaviour
{

    private static SettingsManager _settingsManager;

    [SerializeField]
    private string fileName = "options.json";
    private string filePath;


    private float volume;
    private int resolutionIndex;


    private Resolution[] resolutions;

    private SettingsData _settingsData;

    public float Volume
    {
        get => volume;

        set
        {
            if (value < 0 || value > 1.0f) return;
            AudioListener.volume = _settingsData.volume = volume = value;
            // Auto save each time settings change
            Save();
        }
    }

    public int ResolutionIndex
    {
        get => resolutionIndex;
        set
        {
            if (value >= resolutions.Length || value < 0) return;
            Screen.SetResolution(resolutions[value].width, resolutions[value].height, Screen.fullScreen);
            _settingsData.resolution = resolutionIndex = value;
            // Auto save each time settings change
            Save();
        }
    }

    private void Awake()
    {
        if (_settingsManager != null && _settingsManager != this)
            Destroy(gameObject);

        _settingsManager = this;
        DontDestroyOnLoad(gameObject);

        _settingsData = new SettingsData();

        resolutions = Screen.resolutions;

        filePath = $"{Application.dataPath}/{fileName}";

        Load();
    }

    public void Save()
    {
        var json = JsonUtility.ToJson(_settingsData, true);
        File.WriteAllText(filePath, json);
    }

    public void Load()
    {
        if (!File.Exists(filePath))
            Save();

        var str = File.ReadAllText(filePath);
        JsonUtility.FromJsonOverwrite(str, _settingsData);
        Volume = _settingsData.volume;
        ResolutionIndex = _settingsData.resolution;
    }

}

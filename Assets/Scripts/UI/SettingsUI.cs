using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 *  SettingsUI, a script that uses the SettingsManager as a backend and binds it to a Unity UI interface, currently supports only Volume and resolutions
 */
public class SettingsUI : MonoBehaviour
{
    #region Variables


    [SerializeField] private TMP_Dropdown resolutionsDropdown;
    [SerializeField] private Slider volumeControl;



    #endregion

    private void OnEnable()
    {
        resolutionsDropdown.ClearOptions();
        var resolutions = Screen.resolutions;
        foreach (var res in resolutions)
        {
            // We add resolutions at runtime, when user enables the options menu
            resolutionsDropdown.options.Add(new TMP_Dropdown.OptionData(res.ToString()));
        }

        // We add a tracking function, each time the value of the slider / dropdown changes, we update the settings manager data and save
        resolutionsDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        volumeControl.onValueChanged.AddListener(delegate { OnVolumeChange(); });

        // We load the data from the settings manager each time we enable the options menu
        resolutionsDropdown.value = GameManager.Instance.settingsManager.ResolutionIndex;
        volumeControl.value = GameManager.Instance.settingsManager.Volume;
    }

    private void OnResolutionChange()
    {
        GameManager.Instance.settingsManager.ResolutionIndex = resolutionsDropdown.value;
    }

    private void OnVolumeChange()
    {
        GameManager.Instance.settingsManager.Volume = volumeControl.value;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "Load Screen Object", menuName = "LoadScreen/LoadScreenObject")]
public class LoadScreenData : ScriptableObject
{
    public string sceneName = "";
    public int buildIndex = 0;
    public Sprite backgroundImage = null;
    [Multiline(12)] public string storyDescription = "";

    public float fadeInDuration = 0.01f, fadeOutDuration = 0.01f, loadScreenWaitTime = 1.0f;
    public bool showLoadScreenWithFade = false;
}
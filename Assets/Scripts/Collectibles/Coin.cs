using UnityEngine;


// Unity function that let's us create the object from right mouse click menu
// This is used to hold coin data for the type object pattern
[CreateAssetMenu(fileName = "CoinData", menuName = "TypeObject/Coins")]
public class Coin : ScriptableObject
{
    // Graphics
    public Sprite coinGraphics;

    // Score
    public int scoreAdded;

    // Animation related
    public float rotationSpeed;
    public float upDownSpeed;
    public float upDownHeight;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeTankData", menuName = "TypeObject/TimeTank")]
public class TimerTank : ScriptableObject
{
    // Grraphics
    public Sprite objectSprite;

    // Timer
    public float timeAdded;

    // Animation related
    public float rotationSpeed;
    public float upDownSpeed;
    public float upDownHeight;
}

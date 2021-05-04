using UnityEngine;

// I've created a new collectible, a coin
public class PickableCoin : Collectible
{
    [SerializeField] private Coin coin;

    // I need a reference to the score system to update the score on the screen
    private ScoreSystem scoreSystem;

    void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();
        // here comes the use of type object, where I can have the same code running but the coins have different sprites
        GetComponent<SpriteRenderer>().sprite = coin.coinGraphics;
    }

    protected override void Animate()
    {
        // This part handles the animation of the object
        // here comes the use of type object, where I can have the same code running but the coins have different animation values
        var position = transform.position;
        transform.Rotate((Vector3.up * coin.rotationSpeed + position.x * Vector3.up) * Time.deltaTime, Space.World);
        position.y += coin.upDownHeight * Mathf.Cos(Time.time * coin.upDownSpeed - position.x) * Time.deltaTime;
        transform.position = position;
    }

    protected override void OnCollectibleEnter()
    {
        // a coin adds score, so I add it in this function
        // here comes the use of type object, where I can have the same code running but the coins can add multiple score values
        scoreSystem.Score += coin.scoreAdded;
    }
}

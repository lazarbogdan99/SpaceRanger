using UnityEngine;

// Same thing as a pickable coin, except here we use timer tank scriptable objects for holding our data
public class PickableTimerTank : Collectible
{
    [SerializeField]
    private TimerTank timerTank;

    private Timer _timer;

    private void Start()
    {
        _timer = FindObjectOfType<Timer>();
        GetComponent<SpriteRenderer>().sprite = timerTank.objectSprite;
    }

    protected override void Animate()
    {
        // This part handles the animation of the object
        var position = transform.position;
        transform.Rotate((Vector3.up * timerTank.rotationSpeed + position.x * Vector3.up) * Time.deltaTime, Space.World);
        position.y += timerTank.upDownHeight * Mathf.Cos(Time.time * timerTank.upDownSpeed - position.x) * Time.deltaTime;
        transform.position = position;
    }

    protected override void OnCollectibleEnter()
    {
        // When player enters, we add to timer instead of adding to score
        _timer.CurrentTime += timerTank.timeAdded;
    }
}

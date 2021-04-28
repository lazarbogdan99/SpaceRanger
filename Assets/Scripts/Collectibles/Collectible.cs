using UnityEngine;

// A mother class for all collectibles, it is meant for inheritance and not direct use
public abstract class Collectible : MonoBehaviour
{
    [Tooltip("If set to true, the object will be destroyed when picked up, else it will be disabled to be respawned easily")]
    [SerializeField] private bool destroy;

    [Tooltip("Time to elapse before destroying the game object")]
    [SerializeField] private float destroyDelay = 0.1f;

    // A function that must be implemented by child class, what happens when the player enters the Collider?
    protected abstract void OnCollectibleEnter();
    // How do we animate the collectible? if we animate it
    protected abstract void Animate();

    private void OnEnable()
    {
        // We reenable the object when player loses a life (Observer pattern)
        PlayerHealthSystem.OnPlayerDeathEvent += ReEnable;
    }

    private void Update()
    {
        // we Call the animate function, no matter how the child class implemented it, just use it
        Animate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Since every single collectibe should play a sound, we play it here
            GameManager.Instance.audioManager.PlayBonusSound();

            // we call the child class's implementation, what happens when player collects this object? add score? Time? energy boost?
            OnCollectibleEnter();
            // If this object should be destroyed, we destroy it here
            if (destroy)
                Destroy(gameObject, destroyDelay);
            else
                gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        // unsubscribe from the observer pattern, null error
        PlayerHealthSystem.OnPlayerDeathEvent -= ReEnable;
    }
    private void ReEnable()
    {
        if (!destroy)
            gameObject.SetActive(true);
    }
}

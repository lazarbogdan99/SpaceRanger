using UnityEngine;

// A mother class for all collectibles, it is meant for inheritance and not direct use
public abstract class Collectible : MonoBehaviour
{
    [Tooltip("If set to true, the object will be destroyed when picked up, else it will be disabled to be respawned easily")]
    [SerializeField] private bool destroy;

    [Tooltip("Time to elapse before destroying the game object")]
    [SerializeField] private float destroyDelay = 0.1f;

    // A function that must be implemented by child class
    protected abstract void OnCollectibleEnter();
    // I animate the collectible
    protected abstract void Animate();

    private void OnEnable()
    {
        // I reenable the object when player loses a life (Observer pattern)
        PlayerHealthSystem.OnPlayerDeathEvent += ReEnable;
    }

    private void Update()
    {
        // I Call the animate function, no matter how the child class implemented it
        Animate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Since every single collectibe should play a sound, I play it here
            GameManager.Instance.audioManager.PlayBonusSound();

            // I call the child class's implementation, when player collects this object or add score or Time
            OnCollectibleEnter();
            // If this object should be destroyed, I destroy it here
            if (destroy)
                Destroy(gameObject, destroyDelay);
            else
                gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        // unsubscribe from the observer pattern, 
        PlayerHealthSystem.OnPlayerDeathEvent -= ReEnable;
    }
    private void ReEnable()
    {
        if (!destroy)
            gameObject.SetActive(true);
    }
} 

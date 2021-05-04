using System.Collections;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    // Observer Pattern: Player Death
    public delegate void OnPlayerDeath();
    public static event OnPlayerDeath OnPlayerDeathEvent;

    [SerializeField] private bool godMod;
    [SerializeField] private float countLimit = 2.0f;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private float damageCooldownTime = 2.0f;
    [SerializeField] private Transform respawnPoint;

    // Level Manager for restarting game
    [SerializeField] private LevelManager _levelManager;


    private float count;
    private int life = 0;
    private bool canTakeDamage = true;
    private WaitForSeconds damageCooldown;

    public bool GodMod => godMod;

    private PlayerAnimation _playerAnimation;

    private void OnEnable()
    {
        Timer.OnTimeUpEvent += TimerUpDeath;
    }

    private void OnDisable()
    {
        Timer.OnTimeUpEvent -= TimerUpDeath;
    }

    private void Start()
    {
        _playerAnimation = GetComponent<PlayerAnimation>();
        damageCooldown = new WaitForSeconds(damageCooldownTime);
        life = hearts.Length;
    }
    public void TakeDamage()
    {
        if (godMod) return;
        if (canTakeDamage)
        {
            StartCoroutine(DamageCooldown());
            GameManager.Instance.audioManager.PlayDeathSound();
            Destroy(hearts[life].gameObject);
            StartCoroutine(PlayerDeath());
            if (life <= 0)
                _levelManager.Transition();
        }
    }

    public void StartCounter()
    {
        count += Time.deltaTime;
        if (count >= countLimit)
        {
            TakeDamage();
            count = 0;
        }
    }

    public void ResetCounter()
    {
        count = 0;
    }

    // I have to wait for player's death coroutine to finish before we
    // Can reset the timer
    private void TimerUpDeath()
    {
        TakeDamage();
    }
    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        if (life > 0)
            life -= 1;
        yield return damageCooldown;
        canTakeDamage = true;
    }
    private IEnumerator PlayerDeath()
    {
        if (godMod) yield break;
        yield return _playerAnimation.PlayDeathAnimation();
        PlayerRespawn();
        OnPlayerDeathEvent?.Invoke();
    }

    private void PlayerRespawn()
    {
        transform.position = respawnPoint.position;
        GameManager.Instance.audioManager.PlaySpawnSound();
    }
}

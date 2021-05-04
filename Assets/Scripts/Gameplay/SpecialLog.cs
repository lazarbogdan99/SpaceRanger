using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialLog : MonoBehaviour
{
    #region Added variables

    public GameObject particleEffectsPrefab;
    public float selfDestructTimeout = 1.2f;

    private float _timer;
    private bool isTriggered;

    // Needed to modify the isDead variable to end the current game
    private PlayerHealthSystem _playerHealthSystem;


    // Reference to the global SpecialLogSpawner
    private SpecialLogSpawner _logSpawner;

    #endregion

    private void Start()
    {
        _playerHealthSystem = FindObjectOfType<PlayerHealthSystem>();
        _logSpawner = FindObjectOfType<SpecialLogSpawner>();
    }

    void Update()
    {
        // If there's a player, start counting
        if (isTriggered)
        {
            _timer += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        // If I reach the time limit I self destruct
        if (_timer >= selfDestructTimeout)
        {
            _timer = 0;
            SelfDestruct();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If player exists before I hit the mark, I reset the counter
        if (other.CompareTag("Player"))
        {
            _timer = 0;
            isTriggered = false;
        }
    }

    private void SelfDestruct()
    {
        var obj = Instantiate(particleEffectsPrefab, transform.position, Quaternion.identity);
        // I destroy the particle effects after n sec from creating it
        Destroy(obj, 1.0f);

        foreach (var o in _logSpawner.spawnedLogs)
        {
            if (o == gameObject)
            {
                _logSpawner.spawnedLogs.Remove(o);
                break;
            }
        }

        // I self destruct
        Destroy(gameObject, 0.1f);

        // Player lost
        _playerHealthSystem.TakeDamage();
    }
}
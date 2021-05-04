using System.Collections;
using UnityEngine;


[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerHealthSystem))]
public class Player : MonoBehaviour
{
    [SerializeField] private float xBorderLeft;
    [SerializeField] private float xBorderRight;

    private PlayerAnimation _playerAnimation;
    private PlayerInput _playerInput;
    private PlayerMovement _playerMovement;
    private PlayerHealthSystem _playerHealthSystem;

    private Vector2 input;

    private void Start()
    {
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerInput = GetComponent<PlayerInput>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerHealthSystem = GetComponent<PlayerHealthSystem>();
    }

    private void Update()
    {
        if (_playerAnimation.DeathAnimation)
        {
            _playerMovement.onLog = false;
            input = Vector2.zero;
        }
        else
            input = _playerInput.HandleInput(input);
        _playerAnimation.HandleAnimation(input);
        _playerMovement.ClampPosition();
    }
    private void FixedUpdate()
    {
        _playerMovement.Move(input);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            if (!_playerMovement.onLog)
            {
                _playerHealthSystem.StartCounter();
            }
        }
        else if (collision.tag == "Danger")
        {
            _playerHealthSystem.TakeDamage();
        }
        else if (collision.tag == "Border")
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, xBorderLeft, xBorderRight), transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            _playerHealthSystem.ResetCounter();
        }
    }
}

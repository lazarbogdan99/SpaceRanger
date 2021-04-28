using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/**
 * Dashing, how does it work?
 *      We first recieve input from the player, if he presses the dash key (defined in editor) and a direction key, the player will
 *      Move in the direction (dashDistance) units, then spawns a particle effect and plays it
 */
public class PlayerDash : MonoBehaviour
{
    #region Variables

    [SerializeField] private float dashDistance = 1.0f;
    [SerializeField] private KeyCode dashKey;
    [SerializeField] private float dashCooldownTime = 1.0f;
    [SerializeField] private Image cooldownGraphics;
    [SerializeField] private GameObject dashEffectPrefab;
    [SerializeField] private bool invertEffect;
    [SerializeField] private float addedWidth = 1.2f;

    private Player _player;
    private Vector2 input;
    private bool pressedDashCombo = false;
    private bool canDash = true;

    #endregion


    private void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        //StartCoroutine(Dash());
    }

    private void Update()
    {
        // we use axis raw instead of regular GetAxis because we just want the input from the player without any smoothing or interpolations
        // (GetAxis does some math to make the transition smooth instead of directly giving a 1 or -1 as a value)
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        // We can only dash if the player presesd dash key + a direction
        pressedDashCombo = Input.GetKeyDown(dashKey) && input.sqrMagnitude != 0;
        // We spawn a cooldown graphics sprite to show how much the player should wait before the next dash, the scale is because if the player turns sideway
        // while the image is still there, it will flip which is awkward
        cooldownGraphics.transform.localScale = new Vector3(-_player.transform.lossyScale.x, 1, 1);

        if (pressedDashCombo && canDash)
        {
            // If player pressed the combination, we create the cooldown
            StartCoroutine(DashCooldown());
            // We get the dash direction
            Vector3 dash = input.normalized * dashDistance;
            // We spawn the particle effect 
            SpawnDashEffect();
            // Then we move the player
            _player.transform.position += dash;
        }
    }

    // We spawn the particle effect here and we rotate it / Scale it to match our players's height and match the distance covered by the dash
    private void SpawnDashEffect()
    {
        var playerPosition = _player.transform.position;
        playerPosition += (Vector3)((dashDistance / 2 + addedWidth) * input.normalized);
        var playerSpriteRenderer = _player.GetComponentInChildren<SpriteRenderer>();
        var playerYSize = playerSpriteRenderer.bounds.extents.y;
        playerPosition.y += playerYSize / 2;
        var startAngle = Mathf.Acos(input.x);
        var @obj = Instantiate(dashEffectPrefab, playerPosition, Quaternion.identity);
        @obj.name = "DashEffect";
        var effect = @obj.GetComponent<ParticleSystem>();
        var main = effect.main;
        main.startRotation = startAngle;
        main.startSizeX = dashDistance;
        main.startSizeY = playerSpriteRenderer.transform.lossyScale.y;

        // After having the dash object spawned and configured / rotated correctly we play it, then destroy the game object to keep the hierarchy clean
        effect.Play();
        Destroy(obj, 0.5f);
    }

    private IEnumerator DashCooldown()
    {
        canDash = false;
        var timer = 0.0f;
        while (timer < dashCooldownTime)
        {
            cooldownGraphics.fillAmount = 1 - (timer / dashCooldownTime);
            timer += Time.deltaTime;
            yield return null;
        }
        cooldownGraphics.fillAmount = 0;
        canDash = true;
    }
}

using System.Collections;
using UnityEngine;

internal class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float playerDeathWaitTime = 2.0f;

    private WaitForSeconds playerDeathWait;

    private bool deathAnimation = false;
    public bool DeathAnimation => deathAnimation;

    private void Start()
    {
        playerDeathWait = new WaitForSeconds(playerDeathWaitTime);
    }

    public void HandleAnimation(Vector2 input)
    {
        animator.SetFloat("Horizontal", input.x);
        animator.SetFloat("Vertical", input.y);
        animator.SetFloat("Velocity", input.sqrMagnitude);
    }

    public IEnumerator PlayDeathAnimation()
    {
        deathAnimation = true;
        animator.SetBool("Dead", true);
        yield return playerDeathWait;
        animator.SetBool("Dead", false);
        deathAnimation = false;
    }
}
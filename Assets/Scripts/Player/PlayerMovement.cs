using UnityEngine;

internal class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 velocity;

    [SerializeField] private float yBorderUp, yBorderDown;
    [SerializeField] private float speed;

    [HideInInspector] public bool onLog;
    [HideInInspector] public Vector2 logVelocity;

    public Vector2 Velocity { get => velocity; set => velocity = value; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 input)
    {
        velocity = input * speed;
        if (velocity.sqrMagnitude <= 0.3f && input.sqrMagnitude <= 0.3f)
        {
            velocity = Vector2.zero;
        }
        if (onLog)
        {
            rb.MovePosition(rb.position + (velocity * Time.fixedDeltaTime) + logVelocity);
        }
        else
            rb.MovePosition(rb.position + (velocity * Time.fixedDeltaTime));
    }

    public void ClampPosition()
    {
        var pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, yBorderDown, yBorderUp);
        transform.position = pos;
    }
}
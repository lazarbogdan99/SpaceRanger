using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Logs : MonoBehaviour
{
    public bool ToTheRight;
    public float moveSpeed;

    private Vector2 velocity;

    GameObject Left;
    GameObject Right;

    [SerializeField] private float upDownSpeed = 2.0f, upDownLength = 0.1f, maxRandomOffset = 5.0f;
    private float initialYPosition;

    [Space]

    private GameObject Player;
    Vector2 PlayerOffSet;

    [Space]

    public Vector2 PlayerDis;
    public Vector2 PlayerDisMin;

    bool flag1 = false;

    // Modifications
    // Optimization
    PlayerMovement playerMovement;
    Rigidbody2D rb;
    private float randomOffset;


    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Left = GameObject.Find("LeftBorder");
        Right = GameObject.Find("RightBorder");

        playerMovement = FindObjectOfType<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        initialYPosition = rb.position.y;
        randomOffset = Random.value * maxRandomOffset;
    }

    private void FixedUpdate()
    { if (Player == null)

            PlayerDis = Vector2.one * 1000.0f;
      else 

        PlayerDis = new Vector2(Mathf.Abs(transform.position.x - Player.transform.position.x), Mathf.Abs(transform.position.y - Player.transform.position.y));
        var dir = (ToTheRight) ? Vector2.right : Vector2.left;
        dir.y = upDownLength * Mathf.Sin((initialYPosition * upDownSpeed) + randomOffset);
        initialYPosition += Time.fixedDeltaTime;
        velocity = dir.normalized * moveSpeed * Time.fixedDeltaTime;

        if (PlayerDis.x <= PlayerDisMin.x && PlayerDis.y <= PlayerDisMin.y)
        {
            playerMovement.onLog = true;
            playerMovement.logVelocity = velocity;
            flag1 = true;
        }
        else
        {
            if (flag1)
            {
                flag1 = false;
                playerMovement.onLog = false;
            }
        }

        rb.MovePosition(rb.position + velocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Side"))
        {
            if (PlayerDis.x <= PlayerDisMin.x && PlayerDis.y <= PlayerDisMin.y)
            {
                PlayerOffSet = Player.transform.position - transform.position;
                if (ToTheRight)
                {
                    Player.transform.position = new Vector2(Left.transform.position.x, transform.position.y) + PlayerOffSet;
                }
                else
                {
                    Player.transform.position = new Vector2(Right.transform.position.x, transform.position.y) + PlayerOffSet;
                }
            }

            if (ToTheRight)
            {
                transform.position = new Vector2(Left.transform.position.x, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(Right.transform.position.x, transform.position.y);
            }


        }
    }
}

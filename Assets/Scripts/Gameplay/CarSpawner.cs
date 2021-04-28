using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody2D))]




public class CarSpawner : MonoBehaviour
{
    public float Speed = 6f;
    public bool ToTheRight;

    GameObject Left;
    GameObject Right;

    private void Start()
    {
        Left = GameObject.Find("LeftBorder");
        Right = GameObject.Find("RightBorder");
        if (!ToTheRight)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
        }
    }

    private void FixedUpdate()
    {
        transform.GetComponent<Rigidbody2D>().velocity = (ToTheRight ? Vector2.right : Vector2.left) * Speed * Time.deltaTime * 300f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Side")
        {

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

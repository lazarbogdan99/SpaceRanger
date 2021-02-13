using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logs : MonoBehaviour


{
    [SerializeField] float velocity;
    public bool directionIsRight = true;
   // [RequireComponent] BoxCollider2D

     [SerializeField] public  float  logArea ;

    public bool isLog;

    // Start is called before the first frame update
    void Start()
    {
        
    }




    // Update is called once per frame
    void FixedUpdate()
    

    
    {
        Vector2 position = transform.localPosition;

        if (directionIsRight)
        {
            position.x = position.x + Time.deltaTime * velocity;
            if (position.x >= ((logArea / 2) - 1) + (logArea - 1) - GetComponent<SpriteRenderer>().size.x)
            {
                position.x = -logArea - GetComponent<SpriteRenderer>().size.x;
            }
        }
        else
        {
            position.x = position.x - Time.deltaTime * velocity;
            if (position.x <= ((-logArea / 2) + 1) - (logArea - 1) + GetComponent<SpriteRenderer>().size.x)
            {
              position.x = logArea + GetComponent<SpriteRenderer>().size.x;

            }

        }
        transform.localPosition = position;
    }

  /**  void OnTriggerStay2D(Collider2D coll)
    {
        
        if (coll.tag == "Player")
            coll.transform.parent = transform;
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
            coll.transform.parent = null;
    }*/
}


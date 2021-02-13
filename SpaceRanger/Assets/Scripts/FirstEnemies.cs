using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//[RequireComponent(typeof(Rigidbody2D))]




public class FirstEnemies : MonoBehaviour
{
    [SerializeField] private float velocity = 6f;
    public bool isFacingRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position  = transform.localPosition;
        if (isFacingRight)
        {
            position.x = position.x + (velocity * Vector2.right.x * Time.deltaTime);

            if (position.x >= 17)
            {
                position.x = -17;
            }
            
        } else
        {
            position.x = position.x +  (velocity * Vector2.left.x * Time.deltaTime); 

            if(position.x <= -17)
            {
                position.x = 17;
            }
        }
        
        transform.localPosition = position;
    }
}

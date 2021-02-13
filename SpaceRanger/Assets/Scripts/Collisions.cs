using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour

{

    //public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Danger"))
        {
          //  animator.SetBool("Dead",true);
          //  Debug.Log("ADios");
            
        } else if (collision.tag.Equals("Safe"))
        {
            //animator.SetBool("Danger", false);
          //  Debug.Log("Safe");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

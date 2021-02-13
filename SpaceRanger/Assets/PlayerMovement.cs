using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float velocity = 5f;

    public Rigidbody2D rb;

    public Animator animator;

    private float xScale;

    private float lastX, lastY;

    public GameObject player;

    private Transform trasformParent = null;

    private Vector3 lastParentPos;

    private bool isDead = false;


    Vector3 input = Vector3.zero;

    // Start is called before the first frame update

    void Start()
    {
        xScale = transform.localScale.x;

    }



    private void OnTriggerEnter2D(Collider2D collision)

    {
        if (collision.tag.Equals("Log"))
        {
            trasformParent = collision.gameObject.transform;
            lastParentPos = trasformParent.position; 
            Debug.Log("HOP on");
        }

        else if (collision.tag.Equals("Danger") && trasformParent ==null && isDead == false)

        {

            StartCoroutine(playerDeath());

            isDead = true;


        }
        else if (collision.tag.Equals("Safe"))
        {
            // isSafe = true;
            //Vector2 position = transform.localPosition;
        }


    }

    private IEnumerator playerDeath()

    {
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(1);
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        trasformParent = null;
    }







    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;

        input.x = Input.GetAxis("Horizontal");

        input.y = Input.GetAxis("Vertical");
        if (Mathf.Abs(input.x) > 0.1)
        {
            lastX = input.x;
        }
        if (Mathf.Abs(input.y) > 0.1)
        {
            lastY = input.y;
        }


        animator.SetFloat("Horizontal", lastX);
        animator.SetFloat("Vertical", lastY);
        animator.SetFloat("Velocity", input.sqrMagnitude);

        if (lastX > 0.1f)
        {
            transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);

        }
        else
        {
            transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
        }

        //checkForCollisions();


    }
    /**
        private void checkForCollisions()
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Collidable");

            foreach(GameObject gm in gameObjects)
            {
                ColideableObjects colideableObjects = gm.GetComponent<ColideableObjects>();

                if (colideableObjects.isColliding(this.gameObject))
                {
                    if (colideableObjects.isSafe)
                    {
                        Debug.Log("Is safe");
                    } else
                    {
                        Debug.Log("Not Safe");
                    }

                }
            }

        }
*/
    private void FixedUpdate()
    {
        if (isDead)
            return;

        Vector3 firstDistance = Vector3.zero;
        if(trasformParent != null)
        {
            firstDistance = lastParentPos - trasformParent.position;
            Debug.Log(firstDistance);
            lastParentPos = trasformParent.position;
        }

        transform.position +=-firstDistance + input * Time.deltaTime * velocity;
       // rb.transform.position += -firstDistance + input * Time.deltaTime * velocity;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -13.71605f, 15.38f), Mathf.Clamp(transform.position.y, -8.54f, 8.11f), transform.position.z);
        //rb.MovePosition(rb.position + input * velocity * Time.fixedDeltaTime);

        

        //checkForCollisions();
    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColideableObjects : MonoBehaviour
{

    public bool isSafe;

    Rect playerRect;
    Vector2 playerSize;
    Vector2 playerPosition;

    Rect collidableObjectRect;
    Vector2 collidableObjectSize;
    Vector2 collidableObjectPosition;


    public bool isColliding(GameObject gameObject)
    {
        playerSize = gameObject.transform.GetComponent<SpriteRenderer>().size;

        Debug.Log(playerSize);
        playerPosition = gameObject.transform.position;

        Debug.Log(playerPosition);


        collidableObjectSize = GetComponent<SpriteRenderer>().size;

        collidableObjectSize = transform.position;

        playerRect = new Rect(playerPosition.x , playerPosition.y, playerSize.x, playerSize.y);
        collidableObjectRect = new Rect(collidableObjectPosition.x , collidableObjectPosition.y, collidableObjectSize.x, collidableObjectSize.y);

        if (collidableObjectRect.Overlaps(playerRect, true)) { 

            return true;
        }
        return false;

    }



}

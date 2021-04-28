using UnityEngine;

internal class PlayerInput : MonoBehaviour
{
    private bool walkingLeft;

    public Vector2 HandleInput(Vector2 input)
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        if (x != 0 || y != 0)
        {
            input.x = x;
            input.y = y;
        }

        walkingLeft = input.x < 0;
        transform.localScale = new Vector3((walkingLeft ? 1f : -1f), transform.localScale.y, transform.localScale.z);

        return input;
    }
}
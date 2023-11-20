using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float min_x;
    public float max_x;

    public float speed;

    int direction = 1;
    private void FixedUpdate()
    {
        if (transform.position.x > max_x)
            direction = -1;
        else if (transform.position.x < min_x)
            direction = 1;

        transform.position += new Vector3(speed * direction, 0, 0);
    }
}

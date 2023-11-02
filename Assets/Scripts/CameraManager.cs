using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public Transform target;

    public Transform backg1;
    public Transform backg2;

    float size;
    // Start is called before the first frame update
    void Start()
    {
        size = backg1.GetComponent<BoxCollider2D>().size.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target.position.y > transform.position.y)
        {
            Vector3 targetPos = new Vector3(transform.position.x, target.position.y, transform.position.z);
            transform.position = targetPos;
        }

        if(transform.position.y >= backg2.position.y)
        {
            backg1.position = new Vector3(transform.position.x,backg2.position.y + size, backg1.position.z);
            switchBg();
        }
    }

    void switchBg()
    {
        Transform temp = backg1;
        backg1 = backg2;
        backg2 = temp;
    }

}
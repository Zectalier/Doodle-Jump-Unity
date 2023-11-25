using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.tag == "Finish" || collision.gameObject.tag == "Projectile")
        {
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish") //If the platform touch the finish trigger box, destroy this
            Destroy(this.gameObject);
    }
}

using DoodleJump;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : MonoBehaviour
{

    public float speed;
    public float delay;
    public string type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish") //If the platform touch the finish trigger box, destroy this
            Destroy(this.gameObject);
        else if (collision.tag == "Player" && GameObject.Find("Game Manager").GetComponent<GameManager>().getGameState() == GAME_STATE.playing) //If the trigger is the player, apply special behaviour
        {
            var player = collision.gameObject.GetComponent<Player>();
            if (player.isFlying == false)
            {
                collision.gameObject.GetComponent<Player>().startFlight(speed, delay, type);
                Destroy(this.gameObject);
            }
        }

    }
}

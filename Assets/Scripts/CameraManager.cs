using DoodleJump;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public Transform target;
    public GameObject gameManager;
    public GameObject platformGenerationManager;
    public GameObject monsterGenerationManager;

    GameManager gm;
    PlatformGenerationManager pgm;
    MonsterGenerator mgm;

    public Transform backg1;
    public Transform backg2;
    public Transform endMenu;
    float size;
    // Start is called before the first frame update
    void Start()
    {
        size = backg1.GetComponent<BoxCollider2D>().size.y;
        gm = gameManager.GetComponent<GameManager>();
        pgm = platformGenerationManager.GetComponent<PlatformGenerationManager>();
        mgm = monsterGenerationManager.GetComponent<MonsterGenerator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if target has been destroyed, show end menu
        if (target == null){
            Vector3 targetPos = new Vector3(endMenu.position.x, endMenu.position.y, transform.position.z);
            transform.position = targetPos;
            return;
        }

        if (target.position.y > transform.position.y)
        {
            Vector3 targetPos = new Vector3(transform.position.x, target.position.y, transform.position.z);
            transform.position = targetPos;
            endMenu.position = new Vector3(endMenu.position.x, target.position.y-size, endMenu.position.z);
        }

        if(transform.position.y >= backg2.position.y)
        {
            backg1.position = new Vector3(transform.position.x,backg2.position.y + size, backg1.position.z);
            switchBg();
        }

        if(gm.getGameState()==GAME_STATE.gameOver && target.position.y > endMenu.position.y)
        {
            Vector3 targetPos = new Vector3(transform.position.x, target.position.y, transform.position.z);
            transform.position = targetPos;
        }
    }

    void switchBg()
    {
        Transform temp = backg1;
        backg1 = backg2;
        backg2 = temp;
        pgm.spawnNextPlatforms(backg1.position.y + size/2, backg1.position.y + size + size / 2);
        mgm.spawnNextMonsters(backg1.position.y + size/2, backg1.position.y + size + size / 2);
    }

}
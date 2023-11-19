using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DoodleJump
{
    public enum GAME_STATE { level1, level2, level3, gameOver };

    public class GameManager : MonoBehaviour
    {
        private GAME_STATE gameState;

        public GameObject UIManager;

        public GameObject score;

        TextMeshProUGUI scoreinput;

        public GameObject mainCamera;
        public float currentScore;
        // Start is called before the first frame update
        void Start()
        {
            scoreinput = score.GetComponent<TextMeshProUGUI>();
        }

        void FixedUpdate()
        {
            if(gameState != GAME_STATE.gameOver)
            {
                currentScore = mainCamera.transform.position.y;
                scoreinput.text = ((int)(currentScore*10)).ToString();
            }
            else
            {
                UIManager.GetComponent<UIManager>().showEnd();
            }
        }

        public GAME_STATE getGameState()
        {
            return gameState;
        }

        public void setGameState(GAME_STATE state)
        {
            gameState = state;
        }
    }
}
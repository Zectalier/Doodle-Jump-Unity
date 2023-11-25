using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DoodleJump
{
    public enum GAME_STATE { playing, gameOver, paused};

    public class GameManager : MonoBehaviour
    {
        private GAME_STATE gameState;

        public GameObject UIManager;

        public GameObject score;

        TextMeshProUGUI scoreinput;

        public GameObject endScore;
        TextMeshProUGUI endScoreinput;
        public GameObject endHighScore;
        TextMeshProUGUI endHighScoreinput;

        public GameObject mainCamera;
        public float currentScore;
        // Start is called before the first frame update
        void Start()
        {
            scoreinput = score.GetComponent<TextMeshProUGUI>();
            endScoreinput = endScore.GetComponent<TextMeshProUGUI>();
            endHighScoreinput = endHighScore.GetComponent<TextMeshProUGUI>();

            if (!PlayerPrefs.HasKey("highscore"))
                PlayerPrefs.SetInt("highscore", 0);
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

                int currentHighScore = PlayerPrefs.GetInt("highscore");
                if (currentHighScore < currentScore * 10)
                {
                    PlayerPrefs.SetInt("highscore", (int)(currentScore * 10));
                    currentHighScore = (int)(currentScore * 10);
                }
                endScoreinput.text = ((int)(currentScore * 10)).ToString();
                endHighScoreinput.text = currentHighScore.ToString();
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
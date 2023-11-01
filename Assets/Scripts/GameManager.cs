using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DoodleJump
{
    public enum GAME_STATE { menu, play, gameOver };

    public class GameManager : MonoBehaviour
    {
        private GAME_STATE gameState;

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
            currentScore = mainCamera.transform.position.y;
            scoreinput.text = ((int)(currentScore*10)).ToString();
        }
    }
}
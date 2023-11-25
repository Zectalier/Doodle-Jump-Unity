using DoodleJump;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    GameObject[] endObjects;
    GameObject[] pauseObjects;
    GameManager gameManager;

    public Player player;
    public GameObject score;
    TextMeshProUGUI scoreinput;


    // Start is called before the first frame update
    void Start()
    {
        endObjects = GameObject.FindGameObjectsWithTag("ShowOnEnd");
        pauseObjects = GameObject.FindGameObjectsWithTag("PauseUi");
        scoreinput = score.GetComponent<TextMeshProUGUI>();

        GameObject gm = GameObject.Find("Game Manager");
        if(gm != null)
            gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        hideEnd();
        hidePause();
    }

    public void Reload(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit(){
        Application.Quit();
    }

    public void LoadScene(string levelName){
        SceneManager.LoadScene(levelName);
    }

    public void showEnd(){
        foreach(GameObject g in endObjects){
            scoreinput.text = ((int)(gameManager.currentScore*10)).ToString();
            g.SetActive(true);
        }
    }

    public void hideEnd(){
        foreach(GameObject g in endObjects){
            g.SetActive(false);
        }
    }

    public void pauseGame()
    {
        if (gameManager.getGameState() == GAME_STATE.playing)
        {
            Time.timeScale = 0;
            foreach (GameObject g in pauseObjects)
                g.SetActive(true);
        }
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        foreach (GameObject g in pauseObjects)
            g.SetActive(false);
    }

    void hidePause()
    {
        foreach (GameObject g in pauseObjects)
            g.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    GameObject[] endObjects;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        endObjects = GameObject.FindGameObjectsWithTag("ShowOnEnd");
        hideEnd();
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
            g.SetActive(true);
        }
    }

    public void hideEnd(){
        foreach(GameObject g in endObjects){
            g.SetActive(false);
        }
    }
}

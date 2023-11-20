using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTriggerBox : MonoBehaviour
{
    public string configToLoad;
    public GameObject levelConfigManager;

    LevelConfigManager cfgManager;

    // Start is called before the first frame update
    void Start()
    {
        cfgManager = levelConfigManager.GetComponent<LevelConfigManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LoadConfig();
            Debug.Log("New configuration loaded");
        }
    }

    private void LoadConfig()
    {
        cfgManager.LoadConfig(configToLoad);
    }
}

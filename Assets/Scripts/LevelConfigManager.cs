using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelConfigManager : MonoBehaviour
{
    public static LevelGemeratorConfig cfg = new LevelGemeratorConfig();
    string m_Path;

    private void Awake()
    {
        m_Path = Application.dataPath;
        if (!System.IO.File.Exists(m_Path + "/Resources/config.json"))
        {
            //Config par d�faut
            cfg = new LevelGemeratorConfig();
            cfg.distPlatform = 0.5f;
            cfg.max_distPlatform = 2;
            cfg.platformDict = new Dictionary<string, float>();
            cfg.platformDict.Add("Nothing",1f);
            cfg.platformDict.Add("BreakablePlatform", 0.5f);
            cfg.platformDict.Add("BasePlatform", 1f);
            cfg.platformDict.Add("BasePlatform_Spring", 0.1f);
            cfg.monsterDict = new Dictionary<string, float>();
            cfg.min_distMonster = 4;
            cfg.monsterDict.Add("Nothing", 1f);
            cfg.monsterDict.Add("Monster", 1f);
            cfg.monsterDict.Add("Fly", 0f);
            cfg.monsterDict.Add("BlackHole", 0f);
            string json = JsonConvert.SerializeObject(cfg);
            File.WriteAllText(m_Path + "/Resources/config.json", json);
        }
        else
        {
            LoadConfig("config");
        }
    }

    public void FixedUpdate()
    {
        Debug.Log(cfg);
    }

    public void SaveConfig()
    {
        //Convert the ConfigData object to a JSON string.
        string json = JsonConvert.SerializeObject(cfg);

        //Write the JSON string to a file on disk.
        File.WriteAllText(m_Path + "/Resources/config.json", json);
    }

    public void LoadConfig(string configName)
    {
        //Get the JSON string from the file on disk.
        TextAsset content = Resources.Load(configName) as TextAsset;
        string savedJson = content.ToString();

        //Convert the JSON string back to a ConfigData object.
        cfg = JsonConvert.DeserializeObject<LevelGemeratorConfig>(savedJson);
    }

    public LevelGemeratorConfig GetConfig()
    {
        return cfg;
    }
}

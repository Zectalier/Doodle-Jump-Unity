using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlatformConfigManager : MonoBehaviour
{
    public static PlatformGeneratorConfig cfg = new PlatformGeneratorConfig();
    string m_Path;

    private void Awake()
    {
        m_Path = Application.dataPath;
        if (!System.IO.File.Exists(m_Path + "/LevelConfigs/config.json"))
        {
            //Config par défaut
            cfg = new PlatformGeneratorConfig();
            cfg.distPlatform = 0.5f;
            cfg.max_distPlatform = 2;
            cfg.platformDict = new Dictionary<string, float>();
            cfg.platformDict.Add("Nothing",1f);
            cfg.platformDict.Add("BreakablePlatform", 0.5f);
            cfg.platformDict.Add("BasePlatform", 1f);
            cfg.platformDict.Add("BasePlatform_Spring", 0.1f);
            string json = JsonConvert.SerializeObject(cfg);
            File.WriteAllText(m_Path + "/LevelConfigs/config.json", json);
        }
        else
        {
            string savedJson = File.ReadAllText(m_Path + "/LevelConfigs/config.json");
            cfg = JsonConvert.DeserializeObject<PlatformGeneratorConfig>(savedJson);
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
        File.WriteAllText(m_Path + "/LevelConfigs/config.json", json);
    }

    public void LoadConfig()
    {
        //Get the JSON string from the file on disk.
        string savedJson = File.ReadAllText(m_Path + "/LevelConfigs/config.json");

        //Convert the JSON string back to a ConfigData object.
        cfg = JsonConvert.DeserializeObject<PlatformGeneratorConfig>(savedJson);
    }

    public PlatformGeneratorConfig GetConfig()
    {
        return cfg;
    }
}

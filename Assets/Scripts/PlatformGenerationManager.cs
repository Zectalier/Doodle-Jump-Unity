using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerationManager : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject springPrefab;
    public GameObject breakablePrefab;
    public GameObject onetimeusePrefab;
    public GameObject movingPrefab;

    public GameObject levelGenConfig;
    LevelConfigManager genConfig;

    public float dist_platform; //Distance vertical entre chaque platforme

    public float min_x;
    public float max_x;

    private int last_jumpable_platform = 999;

    void Start()
    {
        genConfig = levelGenConfig.GetComponent<LevelConfigManager>();
    }

    public void spawnNextPlatforms(float min_y, float max_y)
    {
        LevelGemeratorConfig cfg = genConfig.GetConfig();
        List<string> ind = new List<string>();
        List<float> probs = new List<float>();

        List<string> ind_jumpable = new List<string>();
        List<float> probs_jumpable = new List<float>();

        float totalprob = 0;
        float totalprob_jumpable = 0;

        foreach (KeyValuePair<string, float> entry in cfg.platformDict)
        {
            ind.Add(entry.Key);
            probs.Add(entry.Value + totalprob);
            totalprob += entry.Value;
            if (entry.Key != "Nothing" && entry.Key != "BreakablePlatform")
            {
                ind_jumpable.Add(entry.Key);
                probs_jumpable.Add(entry.Value + totalprob_jumpable);
                totalprob_jumpable += entry.Value;
            }
        }

        float current_y = min_y + cfg.distPlatform;
        while (current_y < max_y)
        {
            if (last_jumpable_platform < cfg.max_distPlatform)
            {
                float randomPoint = Random.value * totalprob;
                string platform_chosen = "Nothing";
                for (int i = 0; i < probs.Count; i++)
                {
                    if (randomPoint < probs[i])
                    {
                        platform_chosen = ind[i];
                        break;
                    }
                }
                if (platform_chosen == "BreakablePlatform")
                {
                    spawnPlatform(platform_chosen, current_y, min_x, max_x);
                    last_jumpable_platform++;
                }
                else if (platform_chosen != "Nothing")
                {
                    spawnPlatform(platform_chosen, current_y, min_x, max_x);
                    last_jumpable_platform = 0;
                }
                else
                {
                    last_jumpable_platform++;
                }
            }
            else
            {
                float randomPoint = Random.value * totalprob_jumpable;
                string platform_chosen = "Nothing";
                for (int i = 0; i < probs_jumpable.Count; i++)
                {
                    if (randomPoint < probs_jumpable[i])
                    {
                        platform_chosen = ind_jumpable[i];
                        break;
                    }
                }
                spawnPlatform(platform_chosen, current_y, min_x, max_x);
                last_jumpable_platform = 0;
            }
            current_y = current_y + cfg.distPlatform;
        }
    }

    void spawnPlatform(string platform_type, float y, float min_x, float max_x)
    {
        Vector3 pos = new Vector3(Random.Range(min_x, max_x), y, 0);
        switch (platform_type)
        {
            case "BasePlatform":
                Instantiate(platformPrefab, pos, Quaternion.identity);
                break;
            case "BasePlatform_Spring":
                GameObject plat = Instantiate(platformPrefab, pos, Quaternion.identity);
                Vector3 springPos = new Vector3(Random.Range((float)(pos.x - 0.225), (float)(pos.x + 0.225)), (float)(y + 0.24), 0);
                GameObject s = Instantiate(springPrefab, springPos, Quaternion.identity);
                s.transform.parent = plat.transform;
                break;
            case "BreakablePlatform":
                Instantiate(breakablePrefab, pos, Quaternion.identity);
                break;
            case "OnetimeusePlatform":
                Instantiate(onetimeusePrefab, pos, Quaternion.identity);
                break;
            case "MovingPlatform":
                Instantiate(movingPrefab, pos, Quaternion.identity);
                break;
        }
    }
}
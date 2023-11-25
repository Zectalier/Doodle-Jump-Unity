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
    public GameObject propellerPrefab;
    public GameObject jetpackPrefab;

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
        last_jumpable_platform = 999;
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
            int optional_distance = 0;
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
                switch (platform_chosen)
                {
                    case "BreakablePlatform":
                        optional_distance = spawnPlatform(platform_chosen, current_y, min_x, max_x);
                        last_jumpable_platform++;
                        break;
                    case "Nothing":
                        last_jumpable_platform++;
                        break;
                    default:
                        optional_distance = spawnPlatform(platform_chosen, current_y, min_x, max_x);
                        last_jumpable_platform = 0;
                        break;
                }
                current_y += cfg.distPlatform * optional_distance;
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
                optional_distance = spawnPlatform(platform_chosen, current_y, min_x, max_x);
                current_y += cfg.distPlatform * optional_distance;
                last_jumpable_platform = 0;
            }
            current_y = current_y + cfg.distPlatform;
        }
    }

    int spawnPlatform(string platform_type, float y, float min_x, float max_x)
    {
        Vector3 pos = new Vector3(Random.Range(min_x, max_x), y, 0);
        int platform_distance_optional = 0; //Number of platforms to skip (ie. for spring don't spawn new platform instantly after the spring)
        switch (platform_type)
        {
            case "BasePlatform":
                Instantiate(platformPrefab, pos, Quaternion.identity);
                break;
            case "BasePlatform_Spring":
                GameObject plat = Instantiate(platformPrefab, pos, Quaternion.identity);
                Vector3 springPos = new Vector3(Random.Range((float)(pos.x - 0.225), (float)(pos.x + 0.225)), (float)(y + 0.24), 0);
                GameObject bps = Instantiate(springPrefab, springPos, Quaternion.identity);
                bps.transform.parent = plat.transform;
                platform_distance_optional = 4;
                break;
            case "BasePlatform_Propeller":
                GameObject plat1 = Instantiate(platformPrefab, pos, Quaternion.identity);
                Vector3 propPos = new Vector3(Random.Range((float)(pos.x - 0.225), (float)(pos.x + 0.225)), (float)(y + 0.453), 0);
                GameObject bpp = Instantiate(propellerPrefab, propPos, Quaternion.identity);
                bpp.transform.parent = plat1.transform;
                platform_distance_optional = 4;
                break;
            case "BasePlatform_Jetpack":
                GameObject plat2 = Instantiate(platformPrefab, pos, Quaternion.identity);
                Vector3 jetPos = new Vector3(Random.Range((float)(pos.x - 0.225), (float)(pos.x + 0.225)), (float)(y + 0.492), 0);
                GameObject bpj = Instantiate(jetpackPrefab, jetPos, Quaternion.identity);
                bpj.transform.parent = plat2.transform;
                platform_distance_optional = 4;
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
            case "MovingPlatform_Spring":
                GameObject mplat = Instantiate(platformPrefab, pos, Quaternion.identity);
                Vector3 mspringPos = new Vector3(Random.Range((float)(pos.x - 0.225), (float)(pos.x + 0.225)), (float)(y + 0.24), 0);
                GameObject mps = Instantiate(springPrefab, mspringPos, Quaternion.identity);
                mps.transform.parent = mplat.transform;
                platform_distance_optional = 4;
                break;
            case "MovingPlatform_Propeller":
                GameObject mplat1 = Instantiate(movingPrefab, pos, Quaternion.identity); ;
                Vector3 propPos1 = new Vector3(Random.Range((float)(pos.x - 0.225), (float)(pos.x + 0.225)), (float)(y + 0.453), 0);
                GameObject mpp = Instantiate(propellerPrefab, propPos1, Quaternion.identity);
                mpp.transform.parent = mplat1.transform;
                platform_distance_optional = 4;
                break;
            case "MovingPlatform_Jetpack":
                GameObject mplat2 = Instantiate(platformPrefab, pos, Quaternion.identity);
                Vector3 jetPos1 = new Vector3(Random.Range((float)(pos.x - 0.225), (float)(pos.x + 0.225)), (float)(y + 0.492), 0);
                GameObject mpj = Instantiate(jetpackPrefab, jetPos1, Quaternion.identity);
                mpj.transform.parent = mplat2.transform;
                platform_distance_optional = 4;
                break;
        }
        return platform_distance_optional;
    }
}

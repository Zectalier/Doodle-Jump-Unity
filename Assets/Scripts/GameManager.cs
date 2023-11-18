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
        public GameObject platformPrefab;
        public GameObject springPrefab;

        public GameObject platformGenConfig;
        PlatformConfigManager genConfig;

        public GameObject score;
        TextMeshProUGUI scoreinput;

        public GameObject mainCamera;
        public float currentScore;

        public float dist_platform; //Distance vertical entre chaque platforme

        public float min_x;
        public float max_x;

        private int last_jumpable_platform = 999;
        // Start is called before the first frame update
        void Start()
        {
            scoreinput = score.GetComponent<TextMeshProUGUI>();
            genConfig = platformGenConfig.GetComponent<PlatformConfigManager>();
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

        public void spawnNextPlatforms(float min_y, float max_y)
        {
            PlatformGeneratorConfig cfg = genConfig.GetConfig();
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
                if(entry.Key != "Nothing")
                {
                    ind_jumpable.Add(entry.Key);
                    probs_jumpable.Add(entry.Value + totalprob_jumpable);
                    totalprob_jumpable += entry.Value;
                }
            }

            float current_y = min_y + cfg.distPlatform;
            while(current_y < max_y)
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
                    if (platform_chosen != "Nothing")
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
            switch (platform_type) {
                case "BasePlatform":
                    Instantiate(platformPrefab, pos, Quaternion.identity);
                    break;
                case "BasePlatform_Spring":
                    GameObject plat = Instantiate(platformPrefab, pos, Quaternion.identity);
                    Vector3 springPos = new Vector3(Random.Range((float)(pos.x - 0.225), (float)(pos.x + 0.225)), (float)(y + 0.24), 0);
                    GameObject s = Instantiate(springPrefab, springPos, Quaternion.identity);
                    s.transform.parent = plat.transform;
                    break;
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
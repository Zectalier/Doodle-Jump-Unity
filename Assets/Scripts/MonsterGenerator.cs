using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public GameObject blackHolePrefab;
    public GameObject monsterPrefab;
    public GameObject fly;
    public GameObject levelGenConfig;
    LevelConfigManager genConfig;

    public float min_x;
    public float max_x;

    public int lastObstacle;
    public int min_dist_monsters; // Minimum distance before spawning a new monster
    // Start is called before the first frame update
    void Start()
    {
        genConfig = levelGenConfig.GetComponent<LevelConfigManager>();
        min_dist_monsters = genConfig.GetConfig().min_dist_monster;
    }

    public void spawnNextMonsters(float min_y, float max_y){
        LevelGemeratorConfig cfg = genConfig.GetConfig();
        List<string> ind = new List<string>();
        List<float> probs = new List<float>();

        float totalprob = 0;

        foreach (KeyValuePair<string, float> entry in cfg.monsterDict)
        {
            ind.Add(entry.Key);
            probs.Add(entry.Value + totalprob);
            totalprob += entry.Value;
        }

        float current_y = min_y + cfg.distMonster;
        while (current_y < max_y)
        {
            float randomPoint = Random.value * totalprob;
            string monster_chosen = "Nothing";
            for (int i = 0; i < probs.Count; i++)
            {
                if (randomPoint < probs[i])
                {
                    monster_chosen = ind[i];
                    break;
                }
            }

            if (lastObstacle >= min_dist_monsters)
            {
                switch (monster_chosen)
                {
                    case "Monster":
                        spawnMonster(monster_chosen, current_y, min_x, max_x);
                        current_y += cfg.distMonster;
                        lastObstacle = 0;
                        break;
                    case "Fly":
                        spawnMonster(monster_chosen, current_y, min_x, max_x);
                        current_y += cfg.distMonster;
                        lastObstacle = 0;
                        break;
                    case "BlackHole":
                        spawnMonster(monster_chosen, current_y, min_x, max_x);
                        current_y += cfg.distMonster;
                        lastObstacle = 0;
                        break;
                    case "Nothing":
                        lastObstacle += 1;
                        current_y++;
                        break;
                }
            }
            else
            {
                lastObstacle += 1;
                current_y++;
            }
        }
    }

    public void spawnMonster(string monster_chosen, float y, float min_x, float max_x)
    {
        Vector3 pos = new Vector3(Random.Range(min_x, max_x), y, 0);
        switch (monster_chosen)
        {
            case "Monster":
                Instantiate(monsterPrefab, pos, Quaternion.identity);
                break;
            case "Fly":
                Instantiate(fly, pos, Quaternion.identity);
                break;
            case "BlackHole":
                Instantiate(blackHolePrefab, pos, Quaternion.identity);
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoodleJump;

public class LevelGemeratorConfig
{
    public float distPlatform; //distance minimum entre chaque platforme

    public int max_distPlatform; //distance maximum entre deux platformes

    public Dictionary<string, float> platformDict; //dictionnaire contenant les platformes existantes et leur proba d'être choisie
}

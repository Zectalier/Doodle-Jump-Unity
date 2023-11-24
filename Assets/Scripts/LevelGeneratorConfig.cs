using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoodleJump;

public class LevelGemeratorConfig
{
    public float distPlatform; //distance minimum entre chaque platforme

    public int max_distPlatform; //distance maximum entre deux platformes

    public int min_distMonster; //distance minimum entre deux monstres

    public Dictionary<string, float> platformDict; //dictionnaire contenant les platformes existantes et leur proba d'�tre choisie

    public Dictionary<string, float> monsterDict; //dictionnaire contenant les monstres existants et leur proba d'�tre choisi
}

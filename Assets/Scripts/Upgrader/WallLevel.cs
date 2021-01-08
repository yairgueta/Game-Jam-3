using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Wall levels")]
public class WallLevel : ScriptableObject
{
    public Sprite sprite;
    public int curLevel;
    public float wallPower;
    public bool isDamaged;
    public String description;
    public int requiredWoods;
    public int requiredRocks;

}

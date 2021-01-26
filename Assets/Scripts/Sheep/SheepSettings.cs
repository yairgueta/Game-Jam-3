using System;
using System.Collections.Generic;
using Collectables;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Settings/Sheep")]
    public class SheepSettings : ScriptableObject
    {
        public List<Sheep.Sheep> sheeps;
        public float timeToCollect;
        public int manaAddition;
        public float fillTime;
        public float maxHealth;
        public int maxSheepInScene;
    }
}

using System;
using System.Collections.Generic;
using Collectables;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Settings/Sheep")]
    public class SheepSettings : ScriptableObject
    {
        public List<Sheep> sheeps;
        public Sprite awake, sleep;
        public Sprite emptyAwake, emptySleep;
        public Sprite glowSheep;
        public float timeToCollect;
        public int manaAddition;
        public float fillTime;
        public float maxHealth;
    }
}

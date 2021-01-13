using System;
using UnityEngine;

namespace Upgrader
{
    [CreateAssetMenu(menuName = "UpgradableObject")]
    public class UpgradableObject : ScriptableObject
    {
        public Sprite[] completeSprites;
        public Sprite[] crackedSprited;
        public int spriteIndex;
        public String description;
        public int garde;
        public float healthPoints;
        public int requiredWoods;
        public int requiredRocks;

    }
}

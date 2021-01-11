using System;
using UnityEngine;

namespace Upgrader
{
    [CreateAssetMenu(menuName = "UpgradableObject")]
    public class UpgradableObject : ScriptableObject
    {
        public Sprite sprite;
        public String description;
        public int garde;
        public float healthPoints;
        public int requiredWoods;
        public int requiredRocks;

    }
}

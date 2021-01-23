using UnityEngine;

namespace Upgrader
{
    [CreateAssetMenu(menuName = "UpgradableObject")]
    public class UpgradableObject : ScriptableObject
    {
        public Sprite[] completeSprites;
        public Sprite[] crackedSprites;
        public float healthPoints;
        public int requiredWoods;
        public int requiredRocks;

    }
}

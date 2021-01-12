using System;
using Events;
using Player.Inventory;
using Selections;
using UnityEngine;

namespace Upgrader
{
    [RequireComponent(typeof(Selectable))]
    public class Upgradable : MonoBehaviour
    {
        [SerializeField] private InventoryObject inventory;
        [SerializeField] private UpgradableObject[] grades;
        public Action onUpgrade;
        private SpriteRenderer sr;
        private int curLevel;

        
        private void Start()
        {
            curLevel = 1;
            sr = GetComponent<SpriteRenderer>();
        }

        public UpgradableObject GetNextGradeAttributes()
        {
            return curLevel == grades.Length-1 ? null : grades[curLevel+1];
        }

        public void Upgrade()
        {
            curLevel++;

            sr.sprite = grades[curLevel].sprite;
            inventory[ResourceType.Wood] -= grades[curLevel].requiredWoods;
            inventory[ResourceType.Rock] -= grades[curLevel].requiredRocks;
            onUpgrade?.Invoke();
            
        }

        public void ReduceToLevel(int level)
        {
            curLevel = level;
            sr.sprite = grades[curLevel].sprite;
        }
        
        public UpgradableObject GetCurGradeAttributes()
        {
            return grades[curLevel];
        }
        
    }
}

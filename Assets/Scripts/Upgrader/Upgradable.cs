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
        private int curGrade;

        
        private void Awake()
        {
            curGrade = 1;
            sr = GetComponent<SpriteRenderer>();
        }

        public UpgradableObject GetNextGradeAttributes()
        {
            return curGrade == grades.Length-1 ? null : grades[curGrade+1];
        }

        public void Upgrade()
        {
            curGrade++;

            sr.sprite = grades[curGrade].sprite;
            inventory[ResourceType.Wood] -= grades[curGrade].requiredWoods;
            inventory[ResourceType.Rock] -= grades[curGrade].requiredRocks;
            onUpgrade?.Invoke();
            
        }

        public void ReduceToGrade(int grade)
        {
            curGrade = grade;
            sr.sprite = grades[curGrade].sprite;
        }
        
        public UpgradableObject GetCurGradeAttributes()
        {
            return grades[curGrade];
        }
        
    }
}

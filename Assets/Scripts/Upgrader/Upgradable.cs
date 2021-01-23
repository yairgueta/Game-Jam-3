using System;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Upgrader
{
    public class Upgradable : MonoBehaviour
    {
        public Action onUpgrade;
        
        [SerializeField] private UpgradableObject[] grades;
        private InventoryObject Inventory => PlayerController.CurrentInventory;
        private int curGrade;
        private int spriteIndex;
        
        public Sprite CompleteSprite => grades[curGrade].completeSprites[spriteIndex];
        
        public Sprite CrackedSprite => grades[curGrade].crackedSprites[spriteIndex];

        private void Awake()
        {
            curGrade = 0;
            Upgrade();
        }
        
        public void Upgrade()
        {
            curGrade++;
            UpgradableObject current = grades[curGrade];
            spriteIndex = Random.Range(0, current.completeSprites.Length);
            Inventory[ResourceType.Wood] -= grades[curGrade].requiredWoods;
            Inventory[ResourceType.Rock] -= grades[curGrade].requiredRocks;
            onUpgrade?.Invoke();
        }

        public void ReduceToGrade(int grade)
        {
            curGrade = grade;
            UpgradableObject current = grades[curGrade];
            spriteIndex = Random.Range(0, current.completeSprites.Length);
            onUpgrade?.Invoke();
        }

        public UpgradableObject GetCurGradeAttributes()
        {
            return grades[curGrade];
        }
        
        public UpgradableObject GetNextGradeAttributes()
        {
            return IsMaxGrade ? null : grades[curGrade+1];
        }
        
        public UpgradableObject GetPreviousGradeAttributes()
        {
            return curGrade == 0 ? grades[curGrade] : grades[curGrade-1];
        }

        public int NextGradeRequiredWood => GetNextGradeAttributes().requiredWoods;
        public int NextGradeRequiredRock => GetNextGradeAttributes().requiredRocks;

        public bool IsMaxGrade => curGrade == grades.Length - 1;
    }
}

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
        private int curGrade;
        private int spriteIndex;
        private static InventoryObject Inventory => PlayerController.CurrentInventory;
        
        public Sprite CompleteSprite => grades[curGrade].completeSprites[spriteIndex];
        public Sprite CrackedSprite => grades[curGrade].crackedSprites[spriteIndex];
        public Sprite DestroyedSprite => grades[0].completeSprites[0];

        private void Awake()
        {
            curGrade = 1;
            UpgradableObject current = grades[curGrade];
            spriteIndex = Random.Range(0, current.completeSprites.Length);
            onUpgrade?.Invoke();
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

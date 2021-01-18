using System;
using Player;
using Player.Inventory;
using Selections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Upgrader
{
    public class Upgradable : MonoBehaviour
    {
        private InventoryObject inventory;
        [SerializeField] private UpgradableObject[] grades;
        public Action onUpgrade;
        private SpriteRenderer sr;
        private int curGrade;
        private int spriteIndex;

        
        private void Awake()
        {
            curGrade = 1;
            inventory = PlayerController.CurrentInventory;

            sr = GetComponent<SpriteRenderer>();
        }

        public Sprite GetCompleteSprite()
        {
            return grades[curGrade].completeSprites[spriteIndex];
        }
        
        public Sprite GetCrackedSprite()
        {
            return grades[curGrade].crackedSprited[spriteIndex];
        }

        public void Upgrade()
        {
            curGrade++;
            UpgradableObject current = grades[curGrade];
            spriteIndex = Random.Range(0, current.completeSprites.Length);
            current.spriteIndex = spriteIndex;
            sr.sprite = current.completeSprites[spriteIndex];
            inventory[ResourceType.Wood] -= grades[curGrade].requiredWoods;
            inventory[ResourceType.Rock] -= grades[curGrade].requiredRocks;
            onUpgrade?.Invoke();
            
        }

        public void ReduceToGrade(int grade)
        {
            curGrade = grade;
            UpgradableObject current = grades[curGrade];
            spriteIndex = Random.Range(0, current.completeSprites.Length);
            current.spriteIndex = spriteIndex;
            sr.sprite = current.completeSprites[spriteIndex];
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

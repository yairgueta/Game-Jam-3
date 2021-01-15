using System;
using Player.Inventory;
using Selections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Upgrader
{
    public class Upgradable : MonoBehaviour
    {
        [SerializeField] private InventoryObject inventory;
        [SerializeField] private UpgradableObject[] grades;
        public Action onUpgrade;
        private SpriteRenderer sr;
        private int curGrade;
        private int spriteIndex;

        
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

        public void Cracked()
        {
            sr.sprite = grades[curGrade].crackedSprited[spriteIndex];
        }
        
        public UpgradableObject GetCurGradeAttributes()
        {
            return grades[curGrade];
        }
        
    }
}

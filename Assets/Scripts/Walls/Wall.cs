using Events;
using UnityEngine;
using Upgrader;

namespace Walls
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private Collider2D wallCollider;
        [SerializeField] private GameEvent onWallDestroy;
        private Upgradable upgradable;
        private Fixable fixable;
        private SpriteRenderer sr;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            upgradable = GetComponent<Upgradable>();
            fixable = GetComponent<Fixable>() ?? GetComponentInChildren<Fixable>();
            OnUpgrade();
            upgradable.onUpgrade += OnUpgrade;
            fixable.onDeath += OnDestroyed;
            fixable.onFixed += () => sr.sprite = upgradable.CompleteSprite;
            fixable.onHalfHealth += () => sr.sprite = upgradable.CrackedSprite;
        }

        private void OnDestroyed()
        {
            onWallDestroy.Raise();
            upgradable.ReduceToGrade(0);
            wallCollider.enabled = false;
        }


        public void OnUpgrade()    
        {
            var gradeAttributes = upgradable.GetCurGradeAttributes();
            var prevGrade = upgradable.GetPreviousGradeAttributes();
        
            fixable.SetUp(gradeAttributes.healthPoints, prevGrade.requiredWoods, prevGrade.requiredRocks);
            wallCollider.enabled = true;
            sr.sprite = upgradable.CompleteSprite;
        }

        


    }
}

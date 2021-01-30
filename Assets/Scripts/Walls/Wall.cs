using Events;
using UnityEngine;
using Upgrader;

namespace Walls
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private GameEvent onWallDestroy;
        [SerializeField] private GameObject shadowSprite;
        private Collider2D wallCollider;
        private Upgradable upgradable;
        private Fixable fixable;
        private SpriteRenderer sr;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            wallCollider = GetComponent<Collider2D>();
            fixable = GetComponent<Fixable>();
            upgradable = GetComponent<Upgradable>();
        }

        void Start()
        {
            OnUpgrade();
            upgradable.onUpgrade += OnUpgrade;
            fixable.onDeath += OnDestroyed;
            fixable.onFixed += OnFixed;
            fixable.onHalfHealth += () => sr.sprite = upgradable.CrackedSprite;
        }

        private void OnFixed()
        {
            sr.sprite = upgradable.CompleteSprite;
            wallCollider.enabled = true;
        }

        private void OnDestroyed()
        {
            onWallDestroy.Raise();
            upgradable.ReduceToGrade(1);
            wallCollider.enabled = false;
            sr.sprite = upgradable.DestroyedSprite;
            shadowSprite.SetActive(false);
        }
        
        private void OnUpgrade()    
        {
            var gradeAttributes = upgradable.GetCurGradeAttributes();
            var prevGrade = upgradable.GetPreviousGradeAttributes();
            shadowSprite.SetActive(true);
            fixable.SetUp(gradeAttributes.healthPoints, prevGrade.requiredWoods, prevGrade.requiredRocks);
            wallCollider.enabled = true;
            sr.sprite = upgradable.CompleteSprite;
        }
    }
}

using Events;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Settings/Player Settings", order = 0)]
    public class PlayerSettingsObject : ScriptableObject
    {
        //TODO: make them private?
        [Header("Attributes")]
        public float speed = 10f;
        public float maxHealth = 50f;
        public int maxMana = 100;
        public int initialMana = 70;
        
        [Header("Bullets Properties")]
        public int bulletManaCost = 5;
        public float bulletSpeed = 10f;
        public float bulletPower= 1.5f;
        public float bulletDuration = 3f;
        public float bulletCoolDown = 0.3f;
        
        [Header("Run Time Attributes")]
        public float curHealth;
        public float curMana;

        [Header("Events")]
        public GameEvent onDeath;
        public GameEvent onHealthChange;
        public GameEvent onManaChange;
        public GameEvent onOutOfMana;
        public GameEvent onBulletExplode;

        public void UpdateLife(float damage)
        {
            curHealth += damage;
            Debug.Log("health player "+ curHealth);
            if (curHealth <= 0) onDeath.Raise();
            curHealth = Mathf.Clamp(curHealth, 0, maxHealth);
            onHealthChange.Raise();
        }
        
        public bool UpdateMana(int count)
        {
            var newMana = curMana + count;
            if (newMana < 0)
            {
                onOutOfMana.Raise();
                return false;
            }
            curMana = Mathf.Clamp(newMana, 0, maxMana);
            onManaChange.Raise();
            return true;
        }
    }
}
using Events;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Settings/Player Settings", order = 0)]
    public class PlayerSettingsObject : ScriptableObject
    {
        public float speed = 10f;
        public float maxHealth = 50f;
        public GameEvent onDeath;
        public float curHealth;

        public void UpdateCurrentHealth()
        {
            
        }

    }
}
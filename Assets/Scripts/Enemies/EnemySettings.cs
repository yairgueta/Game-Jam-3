using System;
using Events;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(menuName = "Settings/Enemy Settings")]
    public class EnemySettings : ScriptableObject
    {
        [Header("Attributes")]
        public float attackPower = 1;
        public float health = 5f;
        public float speed = 15f;
        public float fadeSpeed = 0.2f;

        [Header("AI Attributes")]
        public float nextWaypointDistance = 3f;
        public float pathRepeatRate = .5f;
        public float targetRefreshTime = 5f;

        [Header("Events")] 
        public GameEvent onDeath; //todo: do we need it?
        

 
    }
}
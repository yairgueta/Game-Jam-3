using Events;
using UnityEngine;

namespace Cycles
{
    [CreateAssetMenu(menuName = "Settings/Cycle Settings", fileName = "Cycle@New Cycle")]
    public class CycleObject : ScriptableObject
    {
        public Color GlobalLightColor => globalLightColor;
        public float GlobalLightIntensity => globalLightIntensity;
        public float Duration
        {
            get => duration;
            set => duration = value;
        }
        public GameEvent OnCycleStart => onCycleStart;
        
        public GameEvent OnCycleEnd
        {
            get => onCycleEnd;
            set => onCycleEnd = value;
        }
        [Header("General Settings")]
        [SerializeField] private float duration;
        
        [Header("Global Light")] 
        [SerializeField] private Color globalLightColor;
        [SerializeField] private float globalLightIntensity;

        [Header("Events")] 
        [SerializeField] private GameEvent onCycleStart;
        [SerializeField] private GameEvent onCycleEnd;

        [Header("UI")] 
        [SerializeField] private Sprite UITimer;
        [SerializeField] private Sprite UITimerFiller;

    }
}
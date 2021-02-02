using System;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace Cycles
{
    [CreateAssetMenu(menuName = "Settings/Cycle Settings", fileName = "Cycle@New Cycle")][Serializable]
    public class CycleObject : ScriptableObject
    {
        public CyclesType CycleType => cycleType;
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

        public Sprite UIActiveSprite => uiActiveSprite;

        [Header("General Settings")]
        [SerializeField] private float duration;
        [SerializeField] private CyclesType cycleType;


        [Header("Global Light")] 
        [SerializeField] private Color globalLightColor;
        [SerializeField] private float globalLightIntensity;


        [Header("Events")] 
        [SerializeField] private GameEvent onCycleStart;
        [SerializeField] private GameEvent onCycleEnd;


        [Header("UI")] 
        [SerializeField] private Sprite uiActiveSprite;
    }
    public enum CyclesType
    {
        Day = 0, 
        Night = 1,
        Eclipse = 2
    }
}
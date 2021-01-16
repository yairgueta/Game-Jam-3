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

        public Sprite UITimer => uiTimer;
        public Sprite UITimerFiller => uiTimerFiller;
        public Image.FillMethod FillMethod => fillMethod;
        public Image.Origin90 FillOrigin => fillOrigin;


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
        [SerializeField] private Sprite uiTimer;
        [SerializeField] private Sprite uiTimerFiller;
        [SerializeField] private Image.FillMethod fillMethod;
        [SerializeField] private Image.Origin90 fillOrigin;
        
    }
    public enum CyclesType
    {
        Day, 
        Night,
        Eclipse
    }
}
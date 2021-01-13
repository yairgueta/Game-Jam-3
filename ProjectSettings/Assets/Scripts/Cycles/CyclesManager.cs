using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cycles
{
    public class CyclesManager : Singleton<CyclesManager>
    {
        private enum Cycle
        {
            Day,
            Magic,
            Night
        }
    
        [SerializeField] public UnityEvent onDayTimeEnter;
        [SerializeField] public UnityEvent onDayTimeExit;
        
        [SerializeField] public UnityEvent onNightTimeEnter;
        [SerializeField] public UnityEvent onNightTimeExit;
        
        [SerializeField] public UnityEvent onMagicTimeEnter;
        [SerializeField] public UnityEvent onMagicTimeExit;
        
        private Dictionary<Cycle, float> cyclesDurations;
        private Cycle currentCycle;
        private float timer;

        protected override void Awake()
        {
            base.Awake();
            onNightTimeEnter.AddListener(()=>onDayTimeExit?.Invoke());
            onMagicTimeEnter.AddListener(()=>onNightTimeExit?.Invoke());
            onDayTimeEnter.AddListener(()=>onMagicTimeExit?.Invoke());
        }

        void Start()
        {
            cyclesDurations = new Dictionary<Cycle, float>
            {
                {Cycle.Day, 7f},
                {Cycle.Night, 600f},
                {Cycle.Magic, 5f},
            };
            currentCycle = Cycle.Day;
            timer = cyclesDurations[currentCycle];
            onDayTimeEnter.Invoke();
        }
        
        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer > 0) return;
            switch (currentCycle)
            {
                case Cycle.Day:
                    currentCycle = Cycle.Night;
                    onNightTimeEnter?.Invoke();
                    break;
                case Cycle.Night:
                    currentCycle = Cycle.Magic;
                    onMagicTimeEnter?.Invoke();
                    break;
                case Cycle.Magic:
                    currentCycle = Cycle.Day;
                    onDayTimeEnter?.Invoke();
                    break;
            }
            timer = cyclesDurations[currentCycle];
        }

        public float GetRemainingTime()
        {
            return timer / cyclesDurations[currentCycle];
        }
    }
}
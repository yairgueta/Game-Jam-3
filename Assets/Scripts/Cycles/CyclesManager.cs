using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// represents the type of cycles in the game.


// updates the current game cycle.
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
        [SerializeField] public UnityEvent onNightTimeEnter;
        [SerializeField] public UnityEvent onMagicTimeEnter;

        private Dictionary<Cycle, float> cyclesDurations;
        private Cycle currentCycle;
        private float timer;


        void Start()
        {
            cyclesDurations = new Dictionary<Cycle, float>
            {
                {Cycle.Day, 10f},
                {Cycle.Night, 5f},
                {Cycle.Magic, 2f},
            };
            currentCycle = Cycle.Day;
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

        // returns the time count of the current game cycle.
        public float GetTimeInCurrentCycle()
        {
            return timer / cyclesDurations[currentCycle];
        }
    }
}
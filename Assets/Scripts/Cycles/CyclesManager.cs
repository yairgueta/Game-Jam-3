using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cycles
{
    public class CyclesManager : MonoBehaviour
    {
        public static CyclesManager Instance { get; private set; }


        [HideInInspector] [SerializeField] private int[] cyclesOrder = {0,1,2};
        [SerializeField] private CycleObject daySettings;
        [SerializeField] private CycleObject nightSettings;
        [SerializeField] private CycleObject eclipseSettings;
        
        private CycleObject currentCycle;
        private float timer;

        private void OnEnable()
        {
            Instance = this;
        }

        private void OnDisable()
        {
            Instance = null;
        }

        private void Start()
        {
            timer = 0;
        }

        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer > 0) return;
            // switch (currentCycle)
            // {
            //     case Cycle.Day:
            //         currentCycle = Cycle.Night;
            //         onNightTimeEnter?.Invoke();
            //         break;
            //     case Cycle.Night:
            //         currentCycle = Cycle.Magic;
            //         onMagicTimeEnter?.Invoke();
            //         break;
            //     case Cycle.Magic:
            //         currentCycle = Cycle.Day;
            //         onDayTimeEnter?.Invoke();
            //         break;
            // }
            // timer = cyclesDurations[currentCycle];
        }

        public float GetRemainingTime()
        {
            // return timer / cyclesDurations[currentCycle];
            return 0;
        }
    }
}
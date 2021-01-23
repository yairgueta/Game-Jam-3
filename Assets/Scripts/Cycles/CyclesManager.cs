using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cycles
{
    public class CyclesManager : MonoBehaviour
    {
        public static CyclesManager Instance { get; private set; }

        public IEnumerable<CycleObject> CyclesSettings => cyclesSettings;
        public CycleObject DaySettings => cyclesSettings[0];
        public CycleObject NightSettings => cyclesSettings[1];
        public CycleObject EclipseSettings => cyclesSettings[2];
        public CyclesType CurrentCycle => currentCycle.CycleType;

        [HideInInspector][SerializeField] private List<int> cyclesOrder = new List<int>{0,1,2};
        [HideInInspector][SerializeField] private CycleObject[] cyclesSettings;
        private CycleObject currentCycle;
        private Queue<CycleObject> cyclesQueue;
        private float timer;
        private bool isPlaying;

        private void OnEnable()
        {
            Instance = this;
        }

        private void OnDisable()
        {
            Instance = null;
        }
        
        private void Awake()
        {
            var orderedCycles = new []{cyclesSettings[cyclesOrder[0]], cyclesSettings[cyclesOrder[1]], cyclesSettings[cyclesOrder[2]]};
            for (var i = 0; i < 3; i++)
                orderedCycles[i].OnCycleEnd = orderedCycles[(i + 1) % 3].OnCycleStart;
            currentCycle = orderedCycles[0];
            cyclesQueue = new Queue<CycleObject>(orderedCycles);
        }

        public void StartPlaying()
        {
            isPlaying = true;
        }
        
        private void Update()
        {
            if (!isPlaying) return;
            timer -= Time.deltaTime;
            if (timer > 0) return;
            ProgressCycle();
        }


        private void ProgressCycle()
        {
            currentCycle = cyclesQueue.Dequeue();
            cyclesQueue.Enqueue(currentCycle);
            timer = currentCycle.Duration;
            currentCycle.OnCycleStart.Raise();
        }
        
        private void OnGUI()
        {
            GUI.Label(new Rect(Screen.width*.8f, 0, Screen.width*.2f, Screen.height*.1f), currentCycle.name + ": " + Math.Round(timer, 2));
        }

        public float TimePercentage => timer / currentCycle.Duration;
    }
}
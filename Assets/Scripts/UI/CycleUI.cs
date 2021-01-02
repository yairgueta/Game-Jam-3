using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI
{
    public class CycleUI : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private List<CycleToColor> cyclesToColor;

        [Header("References")]
        [SerializeField] private Image radialTimer;

        private void Awake()
        {
            CyclesManager.Instance.onCycleChange.AddListener(cycle =>
            {
                radialTimer.color = cyclesToColor.Find(ctc => ctc.cycle == cycle).color;
                Debug.Log(cycle);
            });
        }

        private void Update()
        {
            radialTimer.fillAmount = 1 - CyclesManager.Instance.GetTimeInCurrentCycle();
        }
        
        [Serializable]
        public struct CycleToColor
        {
            public Cycles cycle;
            public Color color;
        }
    }
}

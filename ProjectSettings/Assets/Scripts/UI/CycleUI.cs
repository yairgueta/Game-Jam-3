using System;
using System.Collections.Generic;
using System.Linq;
using Cycles;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI
{
    public class CycleUI : MonoBehaviour
    {
        [Header("Attributes")] 
        [SerializeField] private Color dayTimeColor, nightTimeColor, magicTimeColor;

        [Header("References")]
        [SerializeField] private Image radialTimer;

        private void Start()
        {
            CyclesManager.Instance.onDayTimeEnter.AddListener(() => ChangeTimerColor(dayTimeColor));
            CyclesManager.Instance.onNightTimeEnter.AddListener(() => ChangeTimerColor(nightTimeColor));
            CyclesManager.Instance.onMagicTimeEnter.AddListener(() => ChangeTimerColor(magicTimeColor));
        }

        private void ChangeTimerColor(Color color)
        {
            radialTimer.color = color;
        }

        private void Update()
        {
            radialTimer.fillAmount = 1 - CyclesManager.Instance.GetRemainingTime();
        }
        
    }
}

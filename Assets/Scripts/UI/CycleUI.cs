using System.Collections.Generic;
using Cycles;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CycleUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image outerFill;
        [SerializeField] private Image innerFill;
        [SerializeField] private Image activeImage;
        [SerializeField] private GameObject pointer;
        [SerializeField] private List<float> fillThreshold;
        private float allPercentage;
        private int currentCycle = 0;
        
        private void Start()
        {
            foreach (var cycle in CyclesManager.Instance.CyclesSettings)
            {
                cycle.OnCycleStart.Register(gameObject, o =>
                {
                    activeImage.sprite = cycle.UIActiveSprite;
                    currentCycle = (int)cycle.CycleType;
                    allPercentage = (currentCycle == 2 ? 1 : fillThreshold[currentCycle + 1]) - fillThreshold[currentCycle];
                });
            }
        }
        
        

        private void Update()
        {
            outerFill.fillAmount =  CyclesManager.Instance.TimePercentage*allPercentage + fillThreshold[currentCycle];
            innerFill.fillAmount =  CyclesManager.Instance.TimePercentage*allPercentage + fillThreshold[currentCycle];
            pointer.transform.eulerAngles = new Vector3(0, 0, -(CyclesManager.Instance.TimePercentage*allPercentage+ 
                                                              fillThreshold[currentCycle])*360);
        }
        
    }
}

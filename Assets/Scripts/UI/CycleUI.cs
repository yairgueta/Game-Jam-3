using Cycles;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CycleUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image radialTimer;
        [SerializeField] private Image filler;

        private void Start()
        {
            foreach (var cycle in CyclesManager.Instance.CyclesSettings)
            {
                var listener = gameObject.AddComponent<GameEventListener>();
                listener.InitEvent(cycle.OnCycleStart);
                listener.response.AddListener(o =>
                {
                    radialTimer.sprite = cycle.UITimer;
                    filler.sprite = cycle.UITimerFiller;

                    filler.fillMethod = cycle.FillMethod;
                    filler.fillOrigin = (int) cycle.FillOrigin;
                    filler.fillAmount = 0;
                });
            }
        }

        private void Update()
        {
            filler.fillAmount = 1 - CyclesManager.Instance.TimePercentage;
        }
        
    }
}

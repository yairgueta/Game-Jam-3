using Cycles;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CycleUI : MonoBehaviour
    {
        [Header("Attributes")] 
        [SerializeField] private Sprite dayUI;
        [SerializeField] private Sprite nightUI;
        [SerializeField] private Sprite eclipseUI;
        
        [SerializeField] private Sprite dayFiller;
        [SerializeField] private Sprite nightFiller;
        [SerializeField] private Sprite eclipseFiller;
        
        
        [Header("References")]
        [SerializeField] private Image radialTimer;
        [SerializeField] private Image filler;

        private void Start()
        {
            CyclesManager.Instance.onDayTimeEnter.AddListener(() =>
            {
                filler.fillMethod = Image.FillMethod.Radial90;
                ChangeTimerColor(dayUI, dayFiller);
                filler.fillOrigin = (int) Image.Origin90.BottomLeft;
            });
            CyclesManager.Instance.onNightTimeEnter.AddListener(() =>
            {
                filler.fillMethod = Image.FillMethod.Radial90;
                ChangeTimerColor(nightUI, nightFiller);
                filler.fillOrigin = (int) Image.Origin90.BottomRight;
            });
            CyclesManager.Instance.onMagicTimeEnter.AddListener(() =>
            {
                ChangeTimerColor(eclipseUI, eclipseFiller);
                filler.fillMethod = Image.FillMethod.Radial360;
                filler.fillOrigin = (int) Image.Origin360.Top;
            });
        }

        private void ChangeTimerColor(Sprite UI, Sprite _filler)
        {
            radialTimer.sprite = UI;
            filler.sprite = _filler;
        }

        private void Update()
        {
            filler.fillAmount = 1 - CyclesManager.Instance.GetRemainingTime();
        }
        
    }
}

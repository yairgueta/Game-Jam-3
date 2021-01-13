using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

namespace Cycles
{
    public class CyclesEnvironment : MonoBehaviour
    {

        [Header("Global Light Preferences")] 
        [SerializeField] private float coloringDuration = .5f;
        [SerializeField] private Light2D globalLight;
        [SerializeField] private Color dayLightColor, nightLightColor, magicLightColor;

        private void Start()
        {
            CyclesManager.Instance.onDayTimeEnter.AddListener(()=>ModifyGlobalLight(dayLightColor));
            CyclesManager.Instance.onNightTimeEnter.AddListener(()=>ModifyGlobalLight(nightLightColor));
            CyclesManager.Instance.onMagicTimeEnter.AddListener(()=>ModifyGlobalLight(magicLightColor));
        }

        private void ModifyGlobalLight(Color lightColor)
        {
            DOTween.To(() => globalLight.color, c => globalLight.color = c, lightColor, coloringDuration);
        }

    }
}

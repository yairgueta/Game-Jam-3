using System;
using System.Linq;
using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Cycles
{
    public class CyclesEnvironment : MonoBehaviour
    {
        [SerializeField] private float animationDuration = .5f;
        private Light2D globalLight;
        private Sequence animationSequence;


        private void Start()
        {
            animationSequence = DOTween.Sequence();
            
            var lights = FindObjectsOfType<Light2D>();
            globalLight = lights.First(l => l.lightType == Light2D.LightType.Global);

            foreach (var cycle in CyclesManager.Instance.CyclesSettings)
            {
                var listener = gameObject.AddComponent<GameEventListener>();
                listener.InitEvent(cycle.OnCycleStart);
                listener.response.AddListener(o => TweenLight(cycle));
            }
        }

        private void TweenLight(CycleObject cycle)
        {
            if(animationSequence.active) animationSequence.Kill(true);
            animationSequence = DOTween.Sequence()
                .Join(DOTween.To(() => globalLight.color, c => globalLight.color = c, cycle.GlobalLightColor,
                    animationDuration))
                .Join(DOTween.To(() => globalLight.intensity, i => globalLight.intensity = i,
                    cycle.GlobalLightIntensity, animationDuration));
        }
    }
}
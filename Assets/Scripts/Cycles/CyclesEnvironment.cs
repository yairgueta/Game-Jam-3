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
        [SerializeField] private SpriteRenderer eclipseBG;
        [SerializeField] private ParticleSystem fireflyParticlePrefab;
        private Light2D globalLight;
        private Sequence animationSequence;
        private Camera mainCam;

        private ParticleSystem fireflyParticleInstance;

        private void Start()
        {
            fireflyParticleInstance = Instantiate(fireflyParticlePrefab);
            animationSequence = DOTween.Sequence();
            mainCam = Camera.main;
            var lights = FindObjectsOfType<Light2D>();
            globalLight = lights.First(l => l.lightType == Light2D.LightType.Global);

            eclipseBG.color = new Color(255, 255, 255, 0);
            
            // Light changes through times
            foreach (var cycle in CyclesManager.Instance.CyclesSettings)
                cycle.OnCycleStart.Register(gameObject, o => TweenLight(cycle));
            
            // camera culling mask in eclipse
            CyclesManager.Instance.EclipseSettings.OnCycleStart.Register(gameObject, o =>
            {
                mainCam.cullingMask |= 1 << LayerMask.NameToLayer("Eclipse");
                fireflyParticleInstance.Play();
                eclipseBG.DOFade(1, animationDuration);
            });
            CyclesManager.Instance.EclipseSettings.OnCycleEnd.Register(gameObject,o =>
            {
                mainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("Eclipse"));
                fireflyParticleInstance.Stop();
                eclipseBG.DOFade(0, animationDuration);
            });

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
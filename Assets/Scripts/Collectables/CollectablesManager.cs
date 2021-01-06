using System;
using Cycles;
using UnityEngine;
using UnityEngine.Events;

namespace Collectables
{
    public class CollectablesManager : Singleton<CollectablesManager>
    {
        public Action<Resource> onResourceCollected;
        public Action<HealthFlower> onHealthFlowerCollected;

        public Transform resourcesParent, flowersParent;

        protected override void Awake()
        {
            base.Awake();
            
            var resourcesParentName = "**Resources Collectables**";
            var par = GameObject.Find(resourcesParentName);
            resourcesParent = par == null ? new GameObject(resourcesParentName).transform : par.transform;
            
            var flowerParentName = "**Flowers**";
            par = GameObject.Find(flowerParentName);
            flowersParent = par == null ? new GameObject(flowerParentName).transform : par.transform;
        }

        private void Start()
        {
            CyclesManager.Instance.onDayTimeEnter.AddListener(() => resourcesParent.gameObject.SetActive(true));
            CyclesManager.Instance.onDayTimeExit.AddListener(() => resourcesParent.gameObject.SetActive(false));
            
            CyclesManager.Instance.onMagicTimeEnter.AddListener(() => flowersParent.gameObject.SetActive(true));
            CyclesManager.Instance.onMagicTimeExit.AddListener(() => flowersParent.gameObject.SetActive(false));

        }
    }
}

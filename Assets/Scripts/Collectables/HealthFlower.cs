using System;
using UnityEngine;

namespace Collectables
{
    [CreateAssetMenu(menuName = "Collectables/Flower_@@")]
    public class HealthFlower : CollectableObject
    {
        
        public override void OnCollected()
        {
            base.OnCollected();
        }
    }
}
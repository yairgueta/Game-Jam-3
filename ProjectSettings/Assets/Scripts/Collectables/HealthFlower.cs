using System;
using Player;
using UnityEditor;
using UnityEngine;

namespace Collectables
{
    [CreateAssetMenu(menuName = "Collectables/Flower_@@")]
    public class HealthFlower : CollectableObject
    {
        [SerializeField] private PlayerSettingsObject playerSettings;
        public override void OnCollected()
        {
            base.OnCollected();
            // playerSettings.
        }
    }
}
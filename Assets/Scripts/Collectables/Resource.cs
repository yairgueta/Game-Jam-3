using Player;
using UnityEngine;

namespace Collectables
{
    [CreateAssetMenu(menuName = "Collectables/Resource")]
    public class Resource : CollectableObject
    {
        [SerializeField] private ResourceType collectableType;
        [SerializeField] private int quantity;
        
        public ResourceType CollectableType => collectableType;
        public int Quantity => quantity;

        public override void OnCollected()
        {
            base.OnCollected();
            PlayerController.CurrentInventory[collectableType] += quantity;
        }
    }
}
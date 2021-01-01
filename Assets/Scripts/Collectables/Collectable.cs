using Player;
using UnityEngine;

namespace Collectables
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private ResourcesType collectableType;
        
        
        public ResourcesType CollectableType => collectableType;
        
        private int quantity;
        
        public int Collect()
        {
            
            
            return 0;
        }

        public void EnterHighlight()
        {
            
        }

        public void ExitHighlight()
        {
            
        }
        
    }
}

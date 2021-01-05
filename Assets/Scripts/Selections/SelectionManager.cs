using UnityEngine;
using UnityEngine.Events;

namespace Selections
{
    public class SelectionManager : Singleton<SelectionManager>
    {
        public UnityEvent<Selectable> onNewSelection;
        
        [Header("Default selection materials")]
        [SerializeField] private Material defaultOverMaterial;
        [SerializeField] private Material defaultClickedDownMaterial;
        [SerializeField] private Material defaultSelectedMaterial;

        public Material DefaultOverMaterial => defaultOverMaterial;
        public Material DefaultClickedDownMaterial => defaultClickedDownMaterial;
        public Material DefaultSelectedMaterial => defaultSelectedMaterial;

        public Selectable CurrentSelected => currentSelected;
        private Selectable currentSelected;
        
        private void Start()
        {
            currentSelected = null;
        }

        internal void NewSelected(Selectable selected)
        {
            if (currentSelected != null)
            {
                currentSelected.Deselect();
            }

            currentSelected = selected;
            currentSelected.Select();
            onNewSelection?.Invoke(currentSelected);
        }
    
    }
}

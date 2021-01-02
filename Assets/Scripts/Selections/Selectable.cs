using System;
using UnityEngine;
using UnityEngine.Events;

namespace Selections
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider))]
    public class Selectable : MonoBehaviour
    {
        internal static Action<Selectable> OnSelected;

        public UnityEvent onThisSelected;
        
        [Header("Materials (keep null for defaults)")]
        [SerializeField] private Material overMaterial = null;
        [SerializeField] private Material clickedDownMaterial = null;
        [SerializeField] private Material selectedMaterial = null;

        private SpriteRenderer sr;
        private Material originalMaterial;
        private bool isSelected;

        private void Start()
        {
            isSelected = false;
            
            if (!overMaterial) overMaterial = SelectionManager.Instance.DefaultOverMaterial;
            if (!selectedMaterial) selectedMaterial = SelectionManager.Instance.DefaultSelectedMaterial;
            if (!clickedDownMaterial) clickedDownMaterial = SelectionManager.Instance.DefaultClickedDownMaterial;
        
            sr = GetComponent<SpriteRenderer>();
            originalMaterial = sr.material;
        }

        private void OnMouseEnter()
        {
            sr.material = overMaterial;
        }

        private void OnMouseExit()
        {
            sr.material = isSelected? selectedMaterial : originalMaterial;
        }

        private void OnMouseDown()
        {
            sr.material = clickedDownMaterial;
        }

        private void OnMouseUp()
        {
            OnSelected?.Invoke(this);
        }

        internal void Select()
        {
            sr.material = selectedMaterial;
            isSelected = true;
            onThisSelected?.Invoke();
        }
        
        public void Deselect()
        {
            sr.material = originalMaterial;
            isSelected = false;
        }
    }
}

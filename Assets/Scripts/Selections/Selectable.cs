using System;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Selections
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class Selectable : MonoBehaviour
    {
        public UnityEvent onThisSelected;
        
        [Header("Materials (keep null for defaults)")]
        [SerializeField] private Material overMaterial = null;
        [SerializeField] private Material clickedDownMaterial = null;
        [SerializeField] private Material selectedMaterial = null;

        private SpriteRenderer sr;
        private Material originalMaterial;
        private bool isSelected;

        private bool interactable;
        private bool hasEnteredAndChanged;

        private void Awake()
        {
            isSelected = false;
            interactable = true;
            hasEnteredAndChanged = false;

            if (!overMaterial) overMaterial = SelectionManager.Instance.DefaultOverMaterial;
            if (!selectedMaterial) selectedMaterial = SelectionManager.Instance.DefaultSelectedMaterial;
            if (!clickedDownMaterial) clickedDownMaterial = SelectionManager.Instance.DefaultClickedDownMaterial;
        
            sr = GetComponent<SpriteRenderer>();
            originalMaterial = sr.material;
        }

        private void OnMouseEnter()
        {
            if (!interactable)
            {
                hasEnteredAndChanged = false;
                return;
            }
            sr.material = overMaterial;
            hasEnteredAndChanged = true;
        }

        private void OnMouseOver()
        {
            if (!interactable)
            {
                hasEnteredAndChanged = false;
                return;
            }
            if (hasEnteredAndChanged) return;
            if (interactable) sr.material = overMaterial;
            hasEnteredAndChanged = true;
        }

        private void OnMouseExit()
        {
            if (interactable) sr.material = isSelected? selectedMaterial : originalMaterial;
            hasEnteredAndChanged = false;
        }

        private void OnMouseDown()
        {
            if (interactable) sr.material = clickedDownMaterial;
        }

        private void OnMouseUp()
        {
            if (interactable) SelectionManager.Instance.NewSelected(this);
        }

        internal void Select()
        {
            if (!interactable) Debug.LogWarning("Selected uninteractable object: " + gameObject.name);
            
            sr.material = selectedMaterial;
            isSelected = true;
            onThisSelected?.Invoke();
        }
        
        public void Deselect()
        {
            sr.material = originalMaterial;
            isSelected = false;
        }

        public void SetInteractable(bool isOn)
        {
            if (isSelected && !isOn) Deselect();
            sr.material = originalMaterial;
            interactable = isOn;
        }
    }
}

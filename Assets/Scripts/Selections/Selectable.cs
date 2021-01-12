using System;
using System.Security.Cryptography;
using Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Selections
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class Selectable : MonoBehaviour
    {
        public Action onThisSelected;
        public float DragTime { get; private set; }
        
        [Header("Materials (keep null for defaults)")]
        [SerializeField] private Material overMaterial = null;
        [SerializeField] private Material clickedDownMaterial = null;
        [SerializeField] private Material selectedMaterial = null;

        private SpriteRenderer sr;
        private Material originalMaterial;

        private float startDragTime;
        
        private bool isSelected;
        private bool interactable;
        private bool hasEnteredAndChanged;

        private void Awake()
        {
            isSelected = false;
            interactable = true;
            hasEnteredAndChanged = false;
            sr = GetComponent<SpriteRenderer>();
            originalMaterial = sr.material;
        }

        private void Start()
        {
            if (!overMaterial) overMaterial = SelectionManager.Instance.DefaultOverMaterial;
            if (!selectedMaterial) selectedMaterial = SelectionManager.Instance.DefaultSelectedMaterial;
            if (!clickedDownMaterial) clickedDownMaterial = SelectionManager.Instance.DefaultClickedDownMaterial;
            
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
            if (!interactable) return;
            sr.material = clickedDownMaterial;
            startDragTime = Time.time;
            DragTime = 0;
        }

        private void OnMouseUp()
        {
            if (!interactable) return;
            SelectionManager.Instance.NewSelected(this);
            onThisSelected?.Invoke();
            DragTime = -1;
        }

        private void OnMouseDrag()
        {
            if (!interactable) return;
            DragTime = Time.time - startDragTime;
        }

        internal void Select()
        {
            if (!interactable) Debug.LogWarning("Selected uninteractable object: " + gameObject.name);
            
            sr.material = selectedMaterial;
            isSelected = true;
        }
        
        internal void Deselect()
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

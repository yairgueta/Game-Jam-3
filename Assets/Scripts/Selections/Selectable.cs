using System;
using System.Security.Cryptography;
using Events;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

namespace Selections
{
    [RequireComponent(typeof(Collider2D))]
    public class Selectable : MonoBehaviour
    {
        public Action onThisSelected;
        public float DragTime { get; private set; }
        
        [Header("Materials (keep null for defaults)")]
        [SerializeField] private Material overMaterial = null;
        [SerializeField] private Material clickedDownMaterial = null;
        [SerializeField] private Material selectedMaterial = null;
        [SerializeField] private SpriteRenderer spriteRenderer;
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
            originalMaterial = spriteRenderer.material;
        }

        private void Start()
        {
            // spriteRenderer ??= GetComponentInParent<SpriteRenderer>() ?? GetComponent<SpriteRenderer>() ?? GetComponentInChildren<SpriteRenderer>();
            
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
            spriteRenderer.material = overMaterial;
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
            if (!interactable) return;
            spriteRenderer.material = overMaterial; 
            hasEnteredAndChanged = true;

        }

        private void OnMouseExit()
        {
            if (interactable) spriteRenderer.material = isSelected? selectedMaterial : originalMaterial;
            hasEnteredAndChanged = false;
        }

        private void OnMouseDown()
        {
            if (!interactable) return;
            spriteRenderer.material = clickedDownMaterial;
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
            
            spriteRenderer.material = selectedMaterial;
            isSelected = true;
        }
        
        internal void Deselect()
        {
            spriteRenderer.material = originalMaterial;
            isSelected = false;
            DragTime = -1;
        }

        public void SetInteractable(bool isOn)
        {
            if (isSelected && !isOn) Deselect();
            spriteRenderer.material = originalMaterial;
            interactable = isOn;
            DragTime = -1;
            hasEnteredAndChanged = false;
        }
    }
}

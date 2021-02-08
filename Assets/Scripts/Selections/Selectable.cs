using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Selections
{
    [AddComponentMenu("")]
    public class Selectable : MonoBehaviour
    {
        public Action onThisMouseEnter, onThisMouseExit;
        public Action onThisSelected;
        public Action onThisEnabled, onThisDisabled;
        public float DragTime { get; private set; }
        public bool IsMouseOver { get; private set; }
        [Tooltip("keep null for default")] [SerializeField] private Material overMaterial = null;
        [Tooltip("keep null for default")] [SerializeField] private Material clickedDownMaterial = null;
        public SpriteRenderer spriteRenderer;
        private Material originalMaterial;

        private float startDragTime;
        
        private void Awake()
        {
            spriteRenderer ??= GetComponent<SpriteRenderer>();
            originalMaterial = spriteRenderer.material;
        }

        private void Start()
        {
            if (!overMaterial) overMaterial = MouseInputHandler.Instance.DefaultOverMaterial;
            if (!clickedDownMaterial) clickedDownMaterial = MouseInputHandler.Instance.DefaultClickedDownMaterial;
        }

        public void MouseEnter()
        {
            spriteRenderer.material = overMaterial;
            onThisMouseEnter?.Invoke();
            IsMouseOver = true;
        }

        public void MouseExit()
        {
            spriteRenderer.material = originalMaterial;
            DragTime = -1;
            IsMouseOver = false;
            onThisMouseExit?.Invoke();
        }

        public void MouseDown()
        {
            spriteRenderer.material = clickedDownMaterial;
            onThisSelected?.Invoke();
            startDragTime = Time.time;
            DragTime = 0;
        }

        public void MouseDrag()
        {
            DragTime = Time.time - startDragTime;
        }
        
        public void MouseUp()
        {
            spriteRenderer.material = originalMaterial;
            DragTime = -1;
        }

        private void OnDisable()
        {
            spriteRenderer.material = originalMaterial;
            DragTime = -1;
            onThisDisabled?.Invoke();
        }

        private void OnEnable()
        {
            onThisEnabled?.Invoke();
        }

        internal void Deselect()
        {
            spriteRenderer.material = originalMaterial;
            DragTime = -1;
        }
    }
}

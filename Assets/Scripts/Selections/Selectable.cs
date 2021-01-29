using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Selections
{
    [RequireComponent(typeof(Collider2D))]
    public class Selectable : MonoBehaviour
    {
        public Action onThisSelected;
        public Action onThisDiabled;
        public float DragTime { get; private set; }
        [Tooltip("keep null for default")] [SerializeField] private Material overMaterial = null;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Material originalMaterial;

        private float startDragTime;

        //     {
        //         var ret = isInteractable && !EventSystem.current.IsPointerOverGameObject();
        //         if (!ret) spriteRenderer.material = isSelected? selectedMaterial : originalMaterial;
        //         return ret;
        //     }
        //     set => isInteractable = value;
        // }

        private void Awake()
        {
            originalMaterial = spriteRenderer.material;
        }

        private void Start()
        {
            if (!overMaterial) overMaterial = MouseInputHandler.Instance.DefaultOverMaterial;
        }

        public void MouseEnter()
        {
            spriteRenderer.material = overMaterial;
        }

        public void MouseExit()
        {
            spriteRenderer.material = originalMaterial;
            DragTime = -1;
        }

        public void MouseDown()
        {
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
            DragTime = -1;
        }

        private void OnDisable()
        {
            spriteRenderer.material = originalMaterial;
            DragTime = -1;
            onThisDiabled?.Invoke();
        }

        internal void Deselect()
        {
            spriteRenderer.material = originalMaterial;
            DragTime = -1;
        }
    }
}

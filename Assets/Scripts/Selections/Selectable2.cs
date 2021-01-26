using System;
using UnityEngine;

namespace Selections
{
    [RequireComponent(typeof(Collider2D))]
    public class Selectable2 : MonoBehaviour
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
        private bool isInteractable;
        
    }
}
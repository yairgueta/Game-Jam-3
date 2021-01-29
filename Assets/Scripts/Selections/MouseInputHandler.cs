using System;
using System.Collections;
using System.Diagnostics;
using Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Selections
{
    public class MouseInputHandler : Singleton<MouseInputHandler>
    {
        private static readonly int MAX_HIT = 10;
        
        public Action<Vector2> onRightClick, onLeftClick;
        public Material DefaultOverMaterial => defaultOverMaterial;
        public Material DefaultClickedDownMaterial => defaultClickedDownMaterial;
        
        [SerializeField] private Material defaultOverMaterial;
        [SerializeField] private Material defaultClickedDownMaterial;
        [SerializeField] private GameEvent onSelectionChangeEvent;
        
        private bool isDragging;
        private Vector2 mousePosition;
        private Collider2D[] hits;

        private int layerMask;
        private int rightMouse, leftMouse;
        private Camera mainCamera;
        private EventSystem currentEventSystem;

        #region Current Selectables (Over / Selected / Dragged)
        private Selectable __currentSelected;
        public Selectable currentSelected
        {
            get
            {
                if (__currentSelected)
                    return __currentSelected.enabled ? __currentSelected : null;
                return null;
            }
            private set => __currentSelected = value;
        }

        private Selectable __currentMouseOver;
        private Selectable currentMouseOver
        {
            get
            {
                if (__currentMouseOver)
                    return __currentMouseOver.enabled ? __currentMouseOver : null;
                return null;
            }
            set => __currentMouseOver = value;
        }
        private Selectable __currentDragged;
        private Selectable currentDragged
        {
            get
            {
                if (__currentDragged)
                    return __currentDragged.enabled ? __currentDragged : null;
                return null;
            }
            set => __currentDragged = value;
        }
        #endregion

        
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            mainCamera = Camera.main;
            currentEventSystem = EventSystem.current;
            
            layerMask = 1 << LayerMask.NameToLayer("Selectable") | 1 << LayerMask.NameToLayer("UI");

            rightMouse = (int) MouseButton.RightMouse;
            leftMouse = (int) MouseButton.LeftMouse;

            hits = new Collider2D[MAX_HIT];
        }

        private void Update()
        {
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(rightMouse)) onRightClick?.Invoke(mousePosition);
            
            
            if (Input.GetMouseButtonDown(leftMouse))
            {
                LeftClickDownHandle();
                onLeftClick?.Invoke(mousePosition);
            }
            
            isDragging = Input.GetMouseButton(leftMouse);

            if (Input.GetMouseButtonUp(leftMouse))
            {
                if (currentMouseOver) currentMouseOver.MouseUp();
            }
        }

        private void FixedUpdate()
        {
            Selectable selectable = GetSelectableFromHit();
            if (selectable != currentMouseOver)
            {
                // if(currentMouseOver && !IsOverCurrentSelected) currentMouseOver.MouseExit();
                if(currentMouseOver) currentMouseOver.MouseExit();
                if(IsSelectableActive(selectable)) selectable.MouseEnter();
            }
            else
            {
                if (isDragging && IsSelectableActive(selectable)) HandleDrag(selectable);
                else currentDragged = null;
            }
            
            currentMouseOver = selectable;
        }

        private void HandleDrag(Selectable dragged)
        {
            if (currentDragged)
            {
                if (currentDragged == dragged) dragged.MouseDrag();
                else
                {
                    currentDragged.Deselect();
                    currentDragged = null;
                }
            }
        }

        private Selectable GetSelectableFromHit()
        {

            if (currentEventSystem.IsPointerOverGameObject(-1)) return null;
            var hitCount = Physics2D.OverlapPointNonAlloc(mousePosition, hits, layerMask);

            if (hitCount == 0)
            {
                overType = OverType.None;
                return null;
            }

            var bestHit = hits[0];

            for (int i = 1; i < hitCount; i++)
                if (bestHit.transform.position.y > hits[i].transform.position.y) bestHit = hits[i];

            var s = bestHit.GetComponent<Selectable>();
            overType = s ? OverType.Selectable : OverType.Other;
            return s;
        }

        private void LeftClickDownHandle()
        {
            if (overType != OverType.Other)
            {
                if (currentSelected) currentSelected.Deselect();
                currentSelected = currentDragged = currentMouseOver;
                currentSelected?.MouseDown();
                onSelectionChangeEvent.Raise();
            }
            
        }

        private bool IsSelectableActive(Selectable selectable) => selectable && selectable.enabled;

        public void Deselect()
        {
            if (currentSelected) currentSelected.Deselect();
            currentSelected = null;
        }

        private OverType overType;
        private enum OverType
        {
            None,
            Other,
            Selectable
        }
    }
}

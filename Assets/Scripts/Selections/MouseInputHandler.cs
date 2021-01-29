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
        
        public Action<Vector2> onRightClick, onLeftClick;
        public Material DefaultOverMaterial => defaultOverMaterial;
        
        [SerializeField] private Material defaultOverMaterial;
        [SerializeField] private GameEvent onSelectionChangeEvent;
        
        private bool isDragging;
        private Vector2 mousePosition;
        private int rightMouse, leftMouse;
        private Camera mainCamera;
        
        
        
        private EventSystem currentEventSystem;
        private int layerMask;
        private Coroutine dragCoroutine;

        
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

        
        private bool IsOverCurrentSelected => currentSelected && currentMouseOver == currentSelected;


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
            var newMouseOver = Physics2D.OverlapPoint(mousePosition, layerMask);
            if (!newMouseOver)
            {
                overType = OverType.None;
                return null;
            }
            var s = newMouseOver.GetComponent<Selectable>();
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

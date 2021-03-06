using System;
using Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Selections
{
    public class MouseInputHandler : MonoBehaviour
    {
        public static MouseInputHandler Instance { get; private set; }
        
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
        private int UILayer;
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

        // Raw current mouse over object.
        private Selectable __currentMouseOver;
        
        // Returns null if there is selectable over the mouse but it is not enabled.
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

        private void OnEnable()
        {
            Instance = this;
        }

        private void OnDisable()
        {
            Instance = null;
        }

        private void Start()
        {
            mainCamera = Camera.main;
            currentEventSystem = EventSystem.current;
            if (!currentEventSystem)
                currentEventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule)).GetComponent<EventSystem>();
            
            UILayer = LayerMask.NameToLayer("UI");
            layerMask = 1 << LayerMask.NameToLayer("Selectable") | 1 << UILayer;

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
                if (currentMouseOver)
                {

                    // currentMouseOver.onThisEnabled = null;
                    currentMouseOver.MouseExit();
                }
                if(IsSelectableActive(selectable)) selectable.MouseEnter();
            }
            else
            {
                if (isDragging && IsSelectableActive(selectable)) HandleDrag(selectable);
                else currentDragged = null;
            }

            if (__currentMouseOver) __currentMouseOver.onThisEnabled = null;
            if (selectable) selectable.onThisEnabled = selectable.MouseEnter;
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

            if (currentEventSystem.IsPointerOverGameObject(-1))
            {
                overType = OverType.Other;
                return null;
            }
            
            var hitCount = Physics2D.OverlapPointNonAlloc(mousePosition, hits, layerMask);
            if (hitCount == 0)
            {
                overType = OverType.None;
                return null;
            }

            var bestY = Mathf.Infinity;
            Selectable bestSelectable = null;
            for (int i = 0; i < hitCount; i++)
            {
                if (hits[i].gameObject.layer == UILayer)
                {
                    overType = OverType.Other;
                    return null;
                }

                if (hits[i].transform.position.y < bestY)
                {
                    var hitSelectable = hits[i].GetComponent<Selectable>();
                    if (hitSelectable && hitSelectable.enabled)
                    {
                        bestSelectable = hitSelectable;
                        bestY = hits[i].transform.position.y;
                    }
                }
            }

            overType = bestSelectable ? OverType.Selectable : OverType.Other;
            return bestSelectable;
        }

        private void LeftClickDownHandle()
        {
            if (overType != OverType.Other)
            {
                // if (currentSelected) currentSelected.Deselect();
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

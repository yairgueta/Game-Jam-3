using System;
using System.Reflection;
using Player.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerAimWeapon : MonoBehaviour
    {
        [SerializeField] private LayerMask shootingLayerMask;
        
        private Player player;
        private Transform aimTransform;
        private Transform aimGunEndPoinTransform;
        private bool ableToShoot = true;
        private Camera mainCamera;
        private Vector3 mousePosition;

        public event EventHandler<OnShootEventArgs> OnSoot; 
        public class OnShootEventArgs : EventArgs
        { 
            public Vector3 gunEndPointPos;
            public Vector3 shootPosition;
        }

        private void Awake()
        {
            aimTransform = transform.Find("Aim");
            aimGunEndPoinTransform = aimTransform.Find("GunEndPos");
            player = GetComponent<Player>();
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (ableToShoot)
            {
                Aiming();
                Shooting();
            }
        }

        private Vector3 GetMousePos()
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            return mousePosition;
        }
    
    
        // aiming the gun at the direction og the mouse
        private void Aiming()
        {
            mousePosition = GetMousePos();
            Vector3 aimDirection = (mousePosition - transform.position).normalized;
            // angle in radians converted to angle in deg
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            aimTransform.eulerAngles = new Vector3(0,0, angle);
        
            // rotate the gun to the right direction
            Vector3 aimLocalScale = Vector3.one;
            if (angle > 90 || angle < -90)
            {
                aimLocalScale.y = -1f;
            }
            else
            {
                aimLocalScale.y = +1f;
            }

            aimTransform.localScale = aimLocalScale;


        }

        private void Shooting()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            if (!player.Inventory.ConsumeItem(ResourceType.Mushroom, 1)) return;
            var hit = Physics2D.Raycast(mousePosition, Vector3.forward, 15f, shootingLayerMask);
            if (hit || EventSystem.current.IsPointerOverGameObject(-1)) return;
            
            OnSoot?.Invoke(this, new OnShootEventArgs
            {
                gunEndPointPos = aimGunEndPoinTransform.position,
                shootPosition = mousePosition,
                    
            });
        }
    }
}

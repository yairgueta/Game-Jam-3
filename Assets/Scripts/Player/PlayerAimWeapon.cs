using System;
using DG.Tweening;
using Selections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAimWeapon : MonoBehaviour
    {
        [SerializeField] private LayerMask shootingLayerMask;
        [SerializeField] private Transform aimGunEndPoinTransform;
        [SerializeField] private Transform bulletGFX;

        private Tween tween;
        private Vector3 originScale;
        
        private Transform aimTransform;
        private bool ableToShoot = true;
        private Camera mainCamera;
        private Vector3 mousePosition;
        public Action<Vector3, Vector3> onSoot;
        
        private void Awake()
        {
            aimTransform = transform.Find("Aim");
            mainCamera = Camera.main;
        }

        private void Start()
        {
            originScale = bulletGFX.localScale;
            
        }

        void Update()
        {
            if (!ableToShoot) return;
            Aiming();
            Shooting();
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
            aimLocalScale.y = angle > 90 || angle < -90 ? -1f : +1f;

            aimTransform.localScale = aimLocalScale;


        }

        private void Shooting()
        {
            if (!Input.GetMouseButtonDown(1)) return;
            
            var hit = Physics2D.Raycast(mousePosition, Vector3.forward, 15f, shootingLayerMask);

            if (hit || EventSystem.current.IsPointerOverGameObject(-1)) return;
            
            if (SelectionManager.Instance.CurrentSelected != null)
            {
                SelectionManager.Instance.Deselect();
                return;
            }
            
            if (!PlayerController.PlayerSettings.UpdateMana(-PlayerController.PlayerSettings.bulletManaCost)) return;
            
            onSoot?.Invoke(aimGunEndPoinTransform.position, mousePosition);
            BulletAnimation();
        }
        
        
        private void BulletAnimation()
        {
            bulletGFX.localScale = Vector3.zero;
            tween?.Kill(true);
            ableToShoot = false;
            tween = DOTween.Sequence()
                .Append(bulletGFX.DOScale(originScale,PlayerController.PlayerSettings.bulletCoolDown * .5f)
                        .SetDelay(PlayerController.PlayerSettings.bulletCoolDown * .5f))
                .AppendCallback(()=>ableToShoot=true);
        }
        
    }
}

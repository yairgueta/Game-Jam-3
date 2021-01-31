using System;
using System.Collections;
using DG.Tweening;
using Selections;
using UnityEditor.Animations;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAimWeapon : MonoBehaviour
    {
        [SerializeField] private Transform aimGunEndPoinTransform;
        [SerializeField] private Transform bulletGFX;
        private PlayerController playerController;

        private Tween tween;
        private Vector3 originScale;
        
        // private Transform aimTransform;
        private bool ableToShoot = true;
        // private Vector3 mousePosition;
        public Action<Vector3, Vector3> onSoot;

        private void Awake()
        {
            // aimTransform = transform.Find("Aim");
        }

        private void Start()
        {
            originScale = bulletGFX.localScale;
            MouseInputHandler.Instance.onRightClick += Shooting;
            playerController = GetComponent<PlayerController>();
        }

        void Update()
        {
            // Aiming();
            // Shooting();
        }

        // private Vector3 GetMousePos()
        // {
        //     // Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //     mousePosition.z = 0f;
        //     return mousePosition;
        // }
    
    
        // aiming the gun at the direction og the mouse
        // private void Aiming()
        // {
        //     mousePosition = GetMousePos();
        //     Vector3 aimDirection = (mousePosition - transform.position).normalized;
        //     // angle in radians converted to angle in deg
        //     float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //     aimTransform.eulerAngles = new Vector3(0,0, angle);
        //
        //     // rotate the gun to the right direction
        //     Vector3 aimLocalScale = Vector3.one;
        //     aimLocalScale.y = angle > 90 || angle < -90 ? -1f : +1f;
        //
        //     aimTransform.localScale = aimLocalScale;
        //
        //
        // }

        private void Shooting(Vector2 mousePosition)
        {
            // bool right = Input.GetMouseButtonDown(1);
            // bool left = Input.GetMouseButtonDown(0);
            //
            // if (!(right || left)) return;
            // var hit = Physics2D.Raycast(mousePosition, Vector3.forward, 15f, shootingLayerMask);
            // if (hit || EventSystem.current.IsPointerOverGameObject(-1)) return;
            // if (SelectionManager.Instance.CurrentSelected != null)
            // {
            //     SelectionManager.Instance.Deselect();
            //     return;
            // }
            // if (!right) return;
            if (!ableToShoot) return;
            if (!PlayerController.PlayerSettings.UpdateMana(-PlayerController.PlayerSettings.bulletManaCost)) return;
            AnimateTowardsShoot(mousePosition);
            StartCoroutine(ShootDelay(mousePosition, true));
        }

        private IEnumerator ShootDelay(Vector2 mousePosition, bool shouldWait)
        {
            yield return new WaitForSeconds(0.05f);
            if (shouldWait)
            {
                StartCoroutine(ShootDelay(mousePosition, false));
            }
            else
            {
                onSoot?.Invoke(aimGunEndPoinTransform.position, mousePosition);
                BulletAnimation();
            }
        }
        
        private void AnimateTowardsShoot(Vector2 mousePosition)
        {
            if (IsWalking()) return;
            var playerPosition = playerController.gameObject.transform.position;
            if (mousePosition.x - playerPosition.x > 0f)
            {
                playerController.LastMoveDirection = new Vector2(1f, 0f);
                return;
            }
            playerController.LastMoveDirection = new Vector2(-1f, 0f);
        }
        
        private bool IsWalking()
        {
            return playerController.MoveDirection.x > 0f || playerController.MoveDirection.y > 0f;
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

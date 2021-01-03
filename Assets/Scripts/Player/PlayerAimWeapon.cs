using System;
using UnityEngine;

namespace Player
{
    public class PlayerAimWeapon : MonoBehaviour
    {

        private Transform aimTransform;
        private Transform aimGunEndPoinTransform;
        private bool ableToShoot = true;
    
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
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            return mousePosition;
        }
    
    
        // aiming the gun at the direction og the mouse
        private void Aiming()
        {
            Vector3 mousePosition = GetMousePos();
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
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = GetMousePos();

                OnSoot?.Invoke(this, new OnShootEventArgs
                {
                    gunEndPointPos = aimGunEndPoinTransform.position,
                    shootPosition = mousePosition,
                    
                });
            }
        }
    
    
    
    
    
    
    
    }
}

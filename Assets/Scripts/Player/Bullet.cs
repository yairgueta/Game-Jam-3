using System;
using Enemies;
using UnityEngine;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed = 100f;

        public void Setup(Vector3 shootingDirection)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.AddForce(shootingDirection * bulletSpeed, ForceMode2D.Impulse);
            Invoke("Disable", 1f);   
        }

        void Disable()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
                Disable();
            }
        }
        

    }
}

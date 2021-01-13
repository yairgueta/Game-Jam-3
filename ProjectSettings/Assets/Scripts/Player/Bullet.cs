using System;
using Enemies;
using Events;
using UnityEngine;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody2D rb2d;
        private GameEvent onExplosionEvent;
        private float power;

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            gameObject.SetActive(false);
        }

        public void Setup(Vector3 shootingDirection, float speed, float duration, GameEvent explosionEvent, float _power)
        {
            gameObject.SetActive(true);
            onExplosionEvent = explosionEvent;
            power = _power;
            
            rb2d.AddForce(shootingDirection * speed, ForceMode2D.Impulse);
            Invoke(nameof(Disable), duration);   
        }

        public void Disable()
        {
            onExplosionEvent.Raise(this);
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy == null) return;
            enemy.TakeDamage(power);
            Disable();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Disable();
        }
    }
}

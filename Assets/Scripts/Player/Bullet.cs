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
            Invoke(nameof(Disable), duration+5);   
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
            Debug.Log("bullet collided with: "+other.gameObject.name);
            var enemy = other.GetComponent<Enemy>();
            if (enemy == null)
            { 
                Disable();
                return;
            }

            enemy.TakeDamage(power);
            Disable();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Disable();
        }
    }
}

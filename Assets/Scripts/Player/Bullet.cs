using System;
using System.Collections;
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
        private Collider2D collider;
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private GameObject circle;

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();
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
            StartCoroutine(Particle());
            rb2d.velocity = Vector2.zero;
            // gameObject.SetActive(false);
        }

        IEnumerator Particle()
        {
            particle.Play();
            circle.SetActive(false);
            collider.enabled = false;
            yield return new WaitForSeconds(0.4f);
            rb2d.velocity = Vector2.zero;
            particle.Stop();
            circle.SetActive(true);
            collider.enabled = true;
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
        
        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     var enemy = other.GetComponent<Enemy>();
        //     if (enemy == null)
        //     { 
        //         Disable();
        //         return;
        //     }
        //
        //     enemy.TakeDamage(power);
        //     Disable();
        // }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy == null)
            { 
                Disable();
                return;
            }
            Debug.Log(other);
            enemy.TakeDamage(power);
            Disable();
        }
    }
}

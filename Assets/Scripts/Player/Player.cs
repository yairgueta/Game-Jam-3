using System;
using System.Collections;
using System.Collections.Generic;
using Collectables;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private float speed;
        [SerializeField] private Animator anim;
        private Rigidbody2D rb;
        private Vector2 moveVelocity;
        private Vector2 moveDirection;
        private Vector2 lastMoveDirection;
        private float curLives = 3;
        private float maxLives = 5;
       
        public UnityEvent<float> OnLivesChange;



        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            CollectablesManager.Instance.onHealthFlowerCollected += CollectedHealthFlower;
            
        }

        void Update()
        {
            PlayerMovement();
            Animate();
        }
        
        
        
        
        // movement of a player
        private void PlayerMovement()
        {
            var moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if ((moveInput.x == 0 && moveInput.y == 0) && moveDirection.x != 0 || moveDirection.y !=0)
            {
                lastMoveDirection = moveDirection;
            }
            moveDirection = moveInput.normalized;
            moveVelocity =  moveDirection * speed;
        }
    

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }

        private void Animate()
        {
            anim.SetFloat("AnimMoveX", moveDirection.x);
            anim.SetFloat("AnimMoveY", moveDirection.y);
            anim.SetFloat("AnimMoveMagnitude", moveDirection.magnitude);
            anim.SetFloat("AnimLastMoveX", lastMoveDirection.x);
            anim.SetFloat("AnimLastMoveY", lastMoveDirection.y);

        }
        

        public void UpdateLife(float damage)
        {
            curLives += damage;
            if (curLives <= 0)
            {
                Die();
            }

            if (curLives > maxLives)
            {
                curLives = maxLives;
            }
            OnLivesChange?.Invoke(curLives);
        }

        public void TakeDamage(float damage)
        {
            UpdateLife(-damage);
        }


        private void CollectedHealthFlower(HealthFlower flower)
        {
            UpdateLife(1);
        }

        private void Die()
        {
            //todo: what happens
            Debug.Log("player is dead");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<Collectable>()?.EnableCollecting(true);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            other.GetComponent<Collectable>()?.EnableCollecting(false);
        }
    }
}

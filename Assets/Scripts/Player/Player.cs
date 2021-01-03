using System;
using System.Collections;
using System.Collections.Generic;
using Collectables;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Rigidbody2D rb;
        private Vector2 moveVelocity;
        private int lives = 5;


        void Start()
        {
            rb = GetComponent<Rigidbody2D>();

        }

        void Update()
        {
            PlayerMovement();
        }
        
        
        
        // movement of a player
        private void PlayerMovement()
        {
            var moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            moveVelocity = moveInput.normalized * speed;
        }
    

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }

        public void ReduceLife(int damage)
        {
            lives-= damage;
            if (lives <= 0)
            {
                Die();
            }
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

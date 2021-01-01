using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Rigidbody2D rb;
        private Vector2 moveVelocity;


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


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Collectible"))
            {
                MirisCollectible col = other.gameObject.GetComponent<MirisCollectible>();
                col.GetNearPlayer();
            }
        }
    }
}

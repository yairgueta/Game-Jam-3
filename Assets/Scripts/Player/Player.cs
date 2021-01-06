using System;
using Collectables;
using Enemies;
using Player.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private InventoryObject inventory;
        [SerializeField] private PlayerSettingsObject playerSettings;
        
        public InventoryObject Inventory => inventory;
        public Vector2 MoveDirection { get; private set; }
        public Vector2 LastMoveDirection { get; private set; }
        public UnityEvent<float> onLivesChange;

        private Rigidbody2D rb;


        private void Awake()
        {
            inventory.Setup();
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            playerSettings.curHealth = playerSettings.maxHealth;
        }

        private void Update()
        {
            PlayerMovement();
        }
        
        private void PlayerMovement()
        {
            var moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if ((moveInput.x == 0 && moveInput.y == 0) && MoveDirection.x != 0 || MoveDirection.y !=0)
            {
                LastMoveDirection = MoveDirection;
            }
            MoveDirection = moveInput.normalized;
        }

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + playerSettings.speed * Time.fixedDeltaTime * MoveDirection);
        }

        private void UpdateLife(float damage)
        {
            playerSettings.curHealth += damage;
            if (playerSettings.curHealth <= 0) Die();
            
            playerSettings.curHealth = playerSettings.maxHealth;
            playerSettings.curHealth = Mathf.Clamp(playerSettings.curHealth, 0, playerSettings.maxHealth);
            
            // onLivesChange?.Invoke(curLives / maxLives);
        }

        public void TakeDamage(float damage)
        {
            UpdateLife(-damage);
        }

        private void Die()
        {
            playerSettings.onDeath.Raise();
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

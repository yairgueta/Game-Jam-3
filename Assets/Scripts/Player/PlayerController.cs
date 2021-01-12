using System;
using Collectables;
using Enemies;
using Player.Inventory;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        public static InventoryObject CurrentInventory { get; private set; }
        public static PlayerSettingsObject PlayerSettings { get; private set; }
        [SerializeField] private InventoryObject inventory;
        [SerializeField] private PlayerSettingsObject playerSettings;

        public Vector2 MoveDirection { get; private set; }
        public Vector2 LastMoveDirection { get; private set; }
        private Rigidbody2D rb;

        private void OnEnable()
        {
            inventory.Setup();
            CurrentInventory = inventory;
            PlayerSettings = playerSettings;
        }

        private void OnDisable()
        {
            CurrentInventory = null;
            PlayerSettings = null;
        }


        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            playerSettings.curHealth = playerSettings.maxHealth;
            playerSettings.UpdateMana(0);
            playerSettings.curMana = playerSettings.initialMana;
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

        public void TakeDamage(float damage)
        {
            playerSettings.UpdateLife(-damage);
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

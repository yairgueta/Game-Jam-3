using System;
using System.Collections.Generic;
using Collectables;
using Enemies;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour, IEnemyDamage
    {
        public static InventoryObject CurrentInventory { get; private set; }
        public static PlayerSettingsObject PlayerSettings { get; private set; }
        public static PlayerController Instance { get; private set; }
        
        [SerializeField] private InventoryObject inventory;
        [SerializeField] private PlayerSettingsObject playerSettings;

        public Vector2 MoveDirection { get; private set; }
        public Vector2 LastMoveDirection { get; set; }
        private Rigidbody2D rb;
        private bool isStart = true;

        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        private void OnEnable()
        {
            CurrentInventory = inventory;
            PlayerSettings = playerSettings;
            inventory.Setup();
            Instance = this;
        }

        private void OnDisable()
        {
            CurrentInventory = null;
            PlayerSettings = null;
            Instance = null;
        }


        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            playerSettings.curHealth = playerSettings.maxHealth;
            playerSettings.UpdateMana(0);
            playerSettings.curMana = playerSettings.initialMana;
            playerSettings.onHealthChange.Raise();
            playerSettings.onManaChange.Raise();
        }

        private void Update()
        {
            PlayerMovement();
        }
        
        private void PlayerMovement()
        {
            var moveInput = new Vector2(Input.GetAxisRaw(Horizontal), Input.GetAxisRaw(Vertical));
            if (isStart) moveInput = Vector2.right; isStart= false;
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

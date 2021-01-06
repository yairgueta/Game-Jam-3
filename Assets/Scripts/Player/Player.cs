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
        [SerializeField] private float speed;
        [SerializeField] private float maxLives = 5;
        
        private Rigidbody2D rb;

        public InventoryObject Inventory => inventory;
        public Vector2 MoveDirection { get; private set; }
        public Vector2 LastMoveDirection { get; private set; }
        private float curLives;

        public UnityEvent<float> onLivesChange;

        private void Awake()
        {
            inventory.Setup();
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            CollectablesManager.Instance.onHealthFlowerCollected += CollectedHealthFlower;
            curLives = 1;

            inventory[ResourceType.Mushroom] += 11;
            inventory[ResourceType.Mushroom] -= 11;
            
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
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * MoveDirection);
        }

        private void UpdateLife(float damage)
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
            onLivesChange?.Invoke(curLives / maxLives);
        }

        public void TakeDamage(float damage)
        {
            UpdateLife(-damage);
        }


        private void CollectedHealthFlower(HealthFlower flower)
        {
            //TODO: mashu aher
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

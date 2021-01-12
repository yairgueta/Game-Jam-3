using System.Collections.Generic;
using System.Linq;
using Events;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerShootProjectiles : MonoBehaviour
    {
        [SerializeField] private int pooledAmount;
        [SerializeField] private GameObject pooledObjectPrefab;

        private Queue<Bullet> bulletsPool;

        private void Awake()
        {
            bulletsPool = new Queue<Bullet>();
            GetComponent<PlayerAimWeapon>().onSoot += PlayerSootProjectiles_OnSoot;
        }

        private void Start()
        {
            var listener = gameObject.AddComponent<GameEventListener>();
            listener.InitEvent(PlayerController.PlayerSettings.onBulletExplode);
            listener.response.AddListener(o => bulletsPool.Enqueue((Bullet)o));
            
            for (int i = 0; i < pooledAmount; i++)
            {
                var bulletPrefab = Instantiate(pooledObjectPrefab);
                var bullet = bulletPrefab.GetComponent<Bullet>();
                bulletsPool.Enqueue(bullet);
            }
        }
        
        
        private void PlayerSootProjectiles_OnSoot(Vector3 gunEndPointPos, Vector3 shootPosition)
        {
            if (bulletsPool.Count == 0)
            {
                Debug.LogWarning("Bullet Pool is Empty");
                return;
            }
            
            var bullet = bulletsPool.Dequeue();
            bullet.transform.position = gunEndPointPos;
            var shootDirection = (shootPosition - gunEndPointPos).normalized;
            
            bullet.Setup(shootDirection, PlayerController.PlayerSettings.bulletSpeed, PlayerController.PlayerSettings.bulletDuration, 
                PlayerController.PlayerSettings.onBulletExplode, PlayerController.PlayerSettings.bulletPower);
        }
    }
}

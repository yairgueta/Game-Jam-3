using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace Player
{
    public class PlayerShootProjectiles : MonoBehaviour
    {

        [SerializeField] private int pooledAmount;
        private GameObject[] bulletsList;
        [SerializeField] private GameObject pooledObject;
        private GameObject curBullet;
        


        private void Awake()
        {
            GetComponent<PlayerAimWeapon>().OnSoot += PlayerSootProjectiles_OnSoot;
        }

        private void Start()
        {
            bulletsList = new GameObject[pooledAmount];
            for (int i = 0; i < pooledAmount; i++)
            {
                GameObject bullet = Instantiate(pooledObject);
                bullet.SetActive(false);
                bulletsList[i] = bullet;
            }
        }
        
        
        private void PlayerSootProjectiles_OnSoot(Object sender, PlayerAimWeapon.OnShootEventArgs e)
        {
            for (int i = 0; i < pooledAmount; i++)
            {
                if (!bulletsList[i].activeInHierarchy)
                {
                    curBullet = bulletsList[i];
                }
            }
            curBullet.transform.position = e.gunEndPointPos;
            Vector3 shootDirection = (e.shootPosition - e.gunEndPointPos).normalized;
            curBullet.SetActive(true);
            curBullet.transform.GetComponent<Bullet>().Setup(shootDirection);
        }


    }
}

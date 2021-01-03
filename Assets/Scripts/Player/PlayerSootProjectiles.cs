using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class PlayerSootProjectiles : MonoBehaviour
{

    [SerializeField] private Transform bulletPrefab; 
    private void Awake()
    {
        GetComponent<PlayerAimWeapon>().OnSoot += PlayerSootProjectiles_OnSoot;
    }

    private void PlayerSootProjectiles_OnSoot(Object sender, PlayerAimWeapon.OnShootEventArgs e)
    {
        Transform bulletTransform = Instantiate(bulletPrefab, e.gunEndPointPos, Quaternion.identity);
        Vector3 shootDirection = (e.shootPosition - e.gunEndPointPos).normalized;
        bulletTransform.GetComponent<Bullet>().Setup(shootDirection);

    }

}

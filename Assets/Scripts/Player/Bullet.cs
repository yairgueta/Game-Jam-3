using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 shootDirection;
    [SerializeField] private float bulletSpeed = 100f;

    public void Setup(Vector3 shootingDirection)
    {
        shootDirection = shootingDirection;
        Destroy(gameObject, 1f);
    }


    void Update()
    {
        transform.position += bulletSpeed * Time.deltaTime * shootDirection;
    }
}

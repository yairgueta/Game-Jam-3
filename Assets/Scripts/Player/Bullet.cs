using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 shootDirection;
    [SerializeField] private float bulletSpeed = 100f;

    // public void Setup(Vector3 shootingDirection)
    // {
        // shootDirection = shootingDirection;
        // Destroy(gameObject, 1f);
    // }
    
    // void Update()
    // {
    //     transform.position += bulletSpeed * Time.deltaTime * shootDirection;
    // }
    
    public void Setup(Vector3 shootingDirection)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(shootingDirection * bulletSpeed, ForceMode2D.Impulse);
        Destroy(gameObject, 1f);
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
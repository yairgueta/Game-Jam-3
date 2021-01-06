using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTakerTester : MonoBehaviour, IDamageable
{
    private float lifePoints = 10;
    private bool isDead;

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
        lifePoints -= damage;
        if (lifePoints <= 0)
        {
            isDead = true;
        }
    }

    public float GetLifePoints()
    {
        return lifePoints;
    }
}

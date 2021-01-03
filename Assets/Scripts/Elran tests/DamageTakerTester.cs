using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTakerTester : MonoBehaviour, IDamageable
{
    private int lifePoints = 10;
    private bool isDead;

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(int damage)
    {
        lifePoints -= damage;
        if (lifePoints <= 0)
        {
            isDead = true;
        }
    }

    public int GetLifePoints()
    {
        return lifePoints;
    }
}

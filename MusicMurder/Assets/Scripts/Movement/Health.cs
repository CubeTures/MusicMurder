using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{

    int health;

    public Health(int health)
    {
        this.health = health;
    }   

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public int GetHealth()
    {
        return health;
    }
}

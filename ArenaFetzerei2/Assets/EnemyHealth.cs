using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public static int healthMax = 100;
    private int health = healthMax;


    public void TakeDamage (int damageAmount) {
        health -= damageAmount;
        if (health <= 0) Die();
    }

    void Die() {
        ScoreScript.scoreValue += 10;
        Destroy(gameObject);
    }

    public int GetHealth() {
        return health;
    }

}

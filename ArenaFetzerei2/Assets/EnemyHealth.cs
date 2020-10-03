using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public GameObject DeathParticle;

    public static int healthMax = 100;
    private int health = healthMax;

    public void TakeDamage (int damageAmount) {
        health -= damageAmount;
        if (health <= 0) Die();
    }

    void Die() {
        ScoreScript.scoreValue += 10;
        Destroy(gameObject);
        GameObject DeathParticleInstant = Instantiate(DeathParticle, transform.position, Quaternion.identity);
        Destroy(DeathParticleInstant, 2f);
    }

    public int GetHealth() {
        return health;
    }

}

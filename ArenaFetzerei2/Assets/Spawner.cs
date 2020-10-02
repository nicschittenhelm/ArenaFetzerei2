using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public float spawnDelay = 15;
    public GameObject EnemyPrefab;
    public int delayInterval = 5;
    public float minSpawnDelay = 1.0f;

    private int delayIntervalCounter = 0;

    IEnumerator Start() {
        while(true) {
            Spawn();
            delayIntervalCounter++;
            Debug.Log(spawnDelay);
            if (delayIntervalCounter >= delayInterval && spawnDelay >= minSpawnDelay) {
                spawnDelay = spawnDelay * 0.8f;
                delayIntervalCounter = 0;
            }
            yield return new WaitForSeconds(spawnDelay * Random.Range(0.8f,1.2f));
        }
    }

    void Spawn() {
        Instantiate(EnemyPrefab, this.transform.position, Quaternion.identity);
    }
}

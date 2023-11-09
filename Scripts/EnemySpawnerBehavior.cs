using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerBehavior : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] float enemyCooldown;
    [SerializeField] Transform[] SpawnerPoints;
    void Start()
    {
        StartCoroutine(EnemySpawner());
    }
    void Update()
    {
        
    }

    IEnumerator EnemySpawner()
    {
        yield return new WaitForSeconds(enemyCooldown);

        Vector3 spawnAtPos = new Vector3(Random.Range(SpawnerPoints[0].position.x, SpawnerPoints[1].position.x), Random.Range(SpawnerPoints[0].position.y, SpawnerPoints[2].position.y), Random.Range(SpawnerPoints[0].position.z, SpawnerPoints[3].position.z));

        Instantiate(enemies[Random.Range(0, enemies.Length)], spawnAtPos, transform.rotation);

        StartCoroutine(EnemySpawner());
    }
}

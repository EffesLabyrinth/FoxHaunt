using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] spawnObject;
    [SerializeField] float minSpawnTime;
    [SerializeField] float maxSpawnTime;
    float startSpawnTime;
    [SerializeField] int minSpawnedEntity;
    [SerializeField] int maxSpawnedEntity;
    int noOfSpawnedEntity;
    [SerializeField] float spawnRadius;

    private void Start()
    {
        startSpawnTime = 7;
    }
    // Update is called once per frame
    void Update()
    {
        if (startSpawnTime > 0) startSpawnTime -= Time.deltaTime;
        else
        {
            noOfSpawnedEntity = Random.Range(minSpawnedEntity, maxSpawnedEntity + 1);
            for (int i = 0; i < noOfSpawnedEntity; i++)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius)) + transform.position;
                Instantiate(spawnObject[Random.Range(0, spawnObject.Length)], spawnPos, Quaternion.identity);
            }
            startSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}

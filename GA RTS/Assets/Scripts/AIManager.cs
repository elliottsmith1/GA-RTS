using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;

    [SerializeField] float spawnTimer = 0.0f;
    [SerializeField] float spawnDelay = 100.0f;
    [SerializeField] float groupSize = 5;
    [SerializeField] float spawnMultiplier = 1.25f;

    private Vector3 playerPos = new Vector3(10, 0, 10);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnDelay)
        {
            spawnTimer = 0.0f;

            SpawnEnemy();

            groupSize *= spawnMultiplier;

            if (spawnDelay > 10.0f)
            spawnDelay -= 5.0f;
        }
    }

    private void SpawnEnemy()
    {
        float num = Random.Range(0, groupSize);

        for (int i = 0; i < num; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.GetComponent<NavMeshAgent>().destination = playerPos;
            enemy.GetComponent<Unit>().SetColour("red");
            enemy.gameObject.tag = "Enemy";
        }
    }
}

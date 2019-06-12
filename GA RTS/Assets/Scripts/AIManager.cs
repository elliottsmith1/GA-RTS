using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;

    private float spawnTimer = 0.0f;
    private float spawnDelay = 80.0f;
    private float groupSize = 5;

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

            groupSize *= 1.5f;

            if (spawnDelay > 10.0f)
            spawnDelay -= 10.0f;
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

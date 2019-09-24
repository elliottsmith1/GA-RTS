using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField] float baseSpace = 10.0f;
    [SerializeField] int numTrees;
    [SerializeField] int numPlants;

    [SerializeField] GameObject corner1;
    [SerializeField] GameObject corner2;

    [SerializeField] List<GameObject> trees;
    [SerializeField] List<GameObject> plants;

    private float minX;
    private float minZ;
    private float maxX;
    private float maxZ;

    // Start is called before the first frame update
    void Start()
    {
        minX = corner1.transform.position.x;
        minZ = corner1.transform.position.z;
        maxX = corner2.transform.position.x;
        maxZ = corner2.transform.position.z;

        SpawnTrees();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnTrees()
    {
        float x = 0;
        float z = 0;
        Vector3 pos = new Vector3(x, 0, z);

        int treeCounter = 0;

        for (int i = 0; i < numTrees; i++)
        {
            x = Random.Range(minX, maxX);
            z = Random.Range(minZ, maxZ);

            pos.x = x;
            pos.z = z;

            if (Vector3.Distance(corner1.transform.position, pos) > baseSpace && Vector3.Distance(corner2.transform.position, pos) > baseSpace)
            {
                GameObject tree = Instantiate(trees[treeCounter], pos, Quaternion.identity);
                tree.transform.SetParent(this.transform);
            }

            treeCounter++;

            if (treeCounter >= trees.Count)
            {
                treeCounter = 0;
            }
        }

        for (int i = 0; i < numPlants; i++)
        {
            x = Random.Range(minX, maxX);
            z = Random.Range(minZ, maxZ);

            pos.x = x;
            pos.z = z;

            GameObject plant = Instantiate(plants[treeCounter], pos, Quaternion.identity);
            plant.transform.SetParent(this.transform);

            treeCounter++;

            if (treeCounter >= plants.Count)
            {
                treeCounter = 0;
            }
        }
    }
}

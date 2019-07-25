using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class AIManager : MonoBehaviour
{
    [Header("Building prefabs")]
    [SerializeField] GameObject barracks;
    [SerializeField] GameObject archeryRange;
    [SerializeField] GameObject house;
    [SerializeField] GameObject lumberMill;
    [SerializeField] GameObject market;

    [SerializeField] GameObject enemyPrefab;

    [SerializeField] float groupSize = 5;

    private Vector3 playerPos = new Vector3(10, 0, 10);

    private List<GameObject> enemyBuildings = new List<GameObject>();

    //base building
    private float expansionFactor = 10.0f;
    private float compactFactor = 10.0f;
    private float economicFactor = 10.0f;
    private float militaryFactor = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        enemyBuildings.Add(GameObject.Find("Enemy TownHall"));
    }

    // Update is called once per frame
    void Update()
    {
        enemyBuildings.RemoveAll(item => item == null);
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

    public List<GameObject> GetEnemyBuildings()
    {
        return enemyBuildings;
    }

    private void ConstructNewBuilding(GameObject _buildingPrefab)
    {
        GameObject building = _buildingPrefab;
        
        if (!_buildingPrefab)
        {
            building = barracks;
        }

        float distanceAllowed = 0.0f;
        BoxCollider box = building.GetComponent<BoxCollider>();

        if (box.size.x > box.size.z)
        {
            distanceAllowed = box.size.x / 2;
        }
        else
        {
            distanceAllowed = box.size.z / 2;
        }

        Vector3 newPosition = enemyBuildings[0].transform.position;

        float expansionDistance = 10.0f;

        NavMeshHit nHit = new NavMeshHit();

        int counter = 0;

        do
        {
            newPosition = enemyBuildings[0].transform.position;

            newPosition.x += Random.Range(-expansionDistance, 0);
            newPosition.z += Random.Range(-expansionDistance, 0);

            NavMesh.FindClosestEdge(newPosition, out nHit, NavMesh.AllAreas);

            expansionDistance *= 1.1f;

            counter++;

            if (counter > 100)
                break;

        } while (Vector3.Distance(newPosition, enemyBuildings[0].transform.position) < 10 || nHit.distance < distanceAllowed || nHit.distance == Mathf.Infinity);//(nHit.distance < 10 || nHit.distance > 1000));

        if (counter < 100)
        {
            GameObject newBuilding = Instantiate(building, newPosition, Quaternion.Euler(new Vector3(-90, 0, Random.Range(0, 359))));

            Building build = newBuilding.GetComponent<Building>();
            build.enabled = true;
            build.ActivateObject();
            newBuilding.tag = "EnemyBuilding";

            enemyBuildings.Add(newBuilding);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(AIManager))]
    public class MachineEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawDefaultInspector();

            AIManager myScript = (AIManager)target;

            if (GUILayout.Button("Spawn enemies"))
            {
                if (Application.isPlaying)
                {
                    myScript.SpawnEnemy();
                }
            }

            if (GUILayout.Button("Spawn building"))
            {
                if (Application.isPlaying)
                {
                    myScript.ConstructNewBuilding(null);
                }
            }
        }
    }
#endif
}

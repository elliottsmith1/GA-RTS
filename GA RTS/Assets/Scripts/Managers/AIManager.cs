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

    [SerializeField] Material enemyMaterial;

    [SerializeField] float decisionTimerDelay = 15.0f;
    private float decisionTimer = 0.0f;

    private Purchasables purchasables;

    private Vector3 playerPos = new Vector3(10, 0, 10);

    private List<GameObject> enemyBuildings = new List<GameObject>();
    private List<Building> buildings = new List<Building>();

    private List<GameObject> playerBuildings = new List<GameObject>();

    private AIUnitManager unitManager;    

    //resources
    private int gold = 50;
    private int wood = 50;

    private float maxFactorValue = 100.0f;

    //base building
    private float expansionFactor = 80.0f;
    private float compactFactor = 1.0f;
    private float economicFactor = 60.0f;
    private float militaryFactor = 20.0f;
    private float populationFactor = 10.0f;

    //population
    private int populationMax = 200;
    private int currentPopulationMax = 20;
    private int population = 0;

    //army expansion
    private float armyExpansionFactor = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerBuildings.Add(GameObject.Find("Player TownHall"));

        enemyBuildings.Add(GameObject.Find("Enemy TownHall"));

        purchasables = GameObject.Find("UI").GetComponent<Purchasables>();

        unitManager = GetComponent<AIUnitManager>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyBuildings.RemoveAll(item => item == null);
        buildings.RemoveAll(item => item == null);

        playerBuildings.RemoveAll(item => item == null);

        decisionTimer += Time.deltaTime;

        if (decisionTimer > decisionTimerDelay)
        {
            decisionTimer = 0.0f;

            MakeDecision();
        }

        ModifyDecisionFactors();
    }

    private void ModifyDecisionFactors()
    {
        float increase = Time.deltaTime;

        if (expansionFactor < 0)
            expansionFactor = 0;

        if (economicFactor < 0)
            economicFactor = 0;

        if (militaryFactor < 0)
            militaryFactor = 0;

        if (populationFactor < 0)
            populationFactor = 0;

        if (armyExpansionFactor < 0)
            armyExpansionFactor = 0;


        //base building
        if (expansionFactor < 100)
            expansionFactor += increase;

        if (economicFactor < 100)
            economicFactor += increase;

        if (militaryFactor < 100)
            militaryFactor += increase;

        if (populationFactor < 100)
        {
            if (population > (currentPopulationMax * 0.75f))
            {
                populationFactor += increase;
            }
        }

        //army expansion
        if (armyExpansionFactor < 100)
            armyExpansionFactor += increase;
    }

    private void MakeDecision()
    {
        bool newUnit = false;
        bool newBuilding = false;

        float rand = Random.Range(0, 100);

        if (rand < expansionFactor)
            newBuilding = true;

        if (rand < armyExpansionFactor)
            newUnit = true;

        if (newBuilding && newUnit)
        {
            //int randNum = Random.Range(0, 2);

            //switch(randNum)
            //{
            //    case 0:
            //        ChooseNewBuilding();
            //        break;
            //    case 1:
            //        SpawnEnemy();
            //        break;
            //}

            if (armyExpansionFactor > expansionFactor)
            {
                SpawnEnemy();
            }
            else
            {
                ChooseNewBuilding();
            }
            return;
        }

        if (newBuilding)
        {
            ChooseNewBuilding();
        }

        if (newUnit)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        float num = Random.Range(0, groupSize);
        int goldCost = 1;
        int populationCost = 1;

        for (int i = 0; i < num; i++)
        {
            if (gold > goldCost && (population + populationCost) <= currentPopulationMax)
            {
                foreach (Building build in buildings)
                {
                    if (build.GetBuilt())
                    {
                        if (build.GetSpawner())
                        {
                            if (build.GetSpawnQueue().Count < 6)
                            {
                                if (unitManager.GetUnits().Count >= unitManager.GetMaxUnitPrep())
                                {
                                    //Debug.Log(unitManager.GetUnits().Count + " / " + unitManager.GetMaxUnitPrep() + " Failed");
                                    return;
                                }

                                //Debug.Log(unitManager.GetUnits().Count + " / " + unitManager.GetMaxUnitPrep() + " Passed");

                                GameObject enemy = Instantiate(enemyPrefab, build.transform.position, Quaternion.identity);
                                Unit unit = enemy.GetComponent<Unit>();
                                unit.SetLayer(enemy.transform, LayerMask.NameToLayer("Enemy"));
                                enemy.gameObject.tag = "Enemy";
                                unit.SetColour("red");
                                AddPopulation(populationCost);
                                enemy.SetActive(false);
                                unit.SetPopulationValue(populationCost);
                                build.NewSpawnUnit(enemy.GetComponent<Unit>(), goldCost);

                                unitManager.NewUnit(enemy, unit);

                                //Debug.Log(unitManager.GetUnits().Count);

                                gold -= goldCost;

                                if (armyExpansionFactor > 10)
                                    armyExpansionFactor -= 10;
                            }
                        }
                    }
                }
            }
        }
    }

    public List<GameObject> GetEnemyBuildings()
    {
        return enemyBuildings;
    }

    private void ChooseNewBuilding()
    {
        float rand = Random.Range(0, 100);
        bool military = false;
        bool economic = false;
        bool population = false;

        if (rand < militaryFactor)
            military = true;

        if (rand < economicFactor)
            economic = true;

        if (rand < populationFactor)
            population = true;

        if (military && economic && population)
        {
            //int randNum = Random.Range(0, 3);

            //switch (randNum)
            //{
            //    case 0:
            //        ConstructNewBuilding(NewMilitaryBuilding());
            //        return;
            //    case 1:
            //        ConstructNewBuilding(NewEconomicBuilding());
            //        return;
            //    case 2:
            //        ConstructNewBuilding(house);
            //        return;
            //}

            if (populationFactor > economicFactor && populationFactor > militaryFactor)
            {
                ConstructNewBuilding(house);
            }
            else if (economicFactor > populationFactor && economicFactor > militaryFactor)
            {
                ConstructNewBuilding(NewEconomicBuilding());
            }

            else if (militaryFactor > populationFactor && militaryFactor > economicFactor)
            {
                ConstructNewBuilding(NewMilitaryBuilding());
            }
            return;
        }

        if (military && economic)
        {
            //int randNum = Random.Range(0, 2);

            //switch (randNum)
            //{
            //    case 0:
            //        ConstructNewBuilding(NewMilitaryBuilding());
            //        return;
            //    case 1:
            //        ConstructNewBuilding(NewEconomicBuilding());
            //        return;
            //}

            if (militaryFactor > economicFactor)
            {
                ConstructNewBuilding(NewMilitaryBuilding());
            }
            else
            {
                ConstructNewBuilding(NewEconomicBuilding());
            }
            return;
        }

        if (military && population)
        {
            //int randNum = Random.Range(0, 2);

            //switch (randNum)
            //{
            //    case 0:
            //        ConstructNewBuilding(NewMilitaryBuilding());
            //        return;
            //    case 1:
            //        ConstructNewBuilding(house);
            //        return;
            //}

            if (populationFactor > militaryFactor)
            {
                ConstructNewBuilding(house);
            }
            else
            {
                ConstructNewBuilding(NewMilitaryBuilding());
            }
            return;
        }

        if (population && economic)
        {
            //int randNum = Random.Range(0, 2);

            //switch (randNum)
            //{
            //    case 0:
            //        ConstructNewBuilding(house);
            //        return;
            //    case 1:
            //        ConstructNewBuilding(NewEconomicBuilding());
            //        return;
            //}
            if (populationFactor > economicFactor)
            {
                ConstructNewBuilding(house);
            }
            else
            {
                ConstructNewBuilding(NewEconomicBuilding());
            }
            return;
        }

        if (military)
        {
            ConstructNewBuilding(NewMilitaryBuilding());
            return;
        }

        if (economic)
        {
            ConstructNewBuilding(NewEconomicBuilding());
            return;
        }

        if (population)
        {
            ConstructNewBuilding(house);
            return;
        }
    }

    private GameObject NewEconomicBuilding()
    {
        economicFactor -= 50;

        int rand = Random.Range(0, 2);

        switch (rand)
        {
            case 0:
                return market;
            case 1:
                return lumberMill;
        }

        return market;
    }

    private GameObject NewMilitaryBuilding()
    {
        militaryFactor -= 50;

        int rand = Random.Range(0, 2);

        switch (rand)
        {
            case 0:
                return barracks;
            case 1:
                return archeryRange;
        }

        return barracks;
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

        distanceAllowed += compactFactor;

        Vector3 newPosition = enemyBuildings[0].transform.position;

        float expansionDistance = 10.0f;

        NavMeshHit nHit = new NavMeshHit();

        int counter = 0;

        //generate new building spawn pos until it's far enough away and not close to any edges
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

        } while (Vector3.Distance(newPosition, enemyBuildings[0].transform.position) < 10 || nHit.distance < distanceAllowed || nHit.distance == Mathf.Infinity);

        if (counter < 100)
        {
            GameObject newBuilding = Instantiate(building, newPosition, Quaternion.Euler(new Vector3(-90, 0, Random.Range(0, 359))));

            Building build = newBuilding.GetComponent<Building>();
            build.enabled = true;

            build.SetEnemyBuilding();

            //if (build.GetCollector())
            //{
            //    newBuilding.GetComponent<ResourceCollection>().SetEnemyBuilding();
            //}

            List<int> costs = purchasables.GetBuildingCost(newBuilding.name);

            int goldCost = costs[0];
            int woodCost = costs[1];         

            if (gold < goldCost || wood < woodCost)
            {
                Destroy(newBuilding);
                return;
            }

            gold -= goldCost;
            wood -= woodCost;

            expansionFactor -= 25;
            
            //newBuilding.tag = "EnemyBuilding";

            build.SetMaterial(enemyMaterial);

            //build.SetBuildState(Building.BUILDSTATE.NOT_BUILT);

            Outline outl = newBuilding.GetComponent<Outline>();
            outl.OutlineColor = Color.red;
            outl.enabled = false;

            enemyBuildings.Add(newBuilding);
            buildings.Add(build);
        }
    }
    
    private void AddPopulation(int _pop)
    {
        population += _pop;
    }

    public void NewHouse()
    {
        populationFactor -= 90;

        currentPopulationMax += 10;

        if (currentPopulationMax > populationMax)
        {
            currentPopulationMax = populationMax;
        }
    }

    public void DestroyedHouse()
    {
        currentPopulationMax -= 10;
    }

    public void AddGold(int _val)
    {
        gold += _val;
    }

    public void AddWood(int _val)
    {
        wood += _val;
    }

    public List<GameObject> GetPlayerBuildings()
    {
        return playerBuildings;
    }

    public void NewPlayerBuilding(GameObject _building)
    {
        playerBuildings.Add(_building);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

public class Building : MonoBehaviour
{
    public enum BUILDSTATE
    {
        PLACING,
        NOT_BUILT,
        BUILDING,
        FINISHED
    }

    public enum SPAWNER_TYPE
    {
        MELEE,
        RANGED,
        MAGIC,
        MOUNTED,
        RESEARCH
    }

    [SerializeField] Image buildtimerBackground;
    [SerializeField] Image buildtimerForeground;

    [SerializeField] GameObject healthBarPrefab;

    [SerializeField] Mesh foundationMesh;
    [SerializeField] Mesh constructionMesh;
    [SerializeField] Mesh finishedMesh;

    [SerializeField] float buildTime = 10.0f;

    [SerializeField] float maxHealth = 1000;

    [SerializeField] bool increasePopulation = false;

    [SerializeField] PlayerSkillManager.BUILDING_TYPE buildingType = PlayerSkillManager.BUILDING_TYPE.MILITARY;
    [SerializeField] bool collector = false;
    [SerializeField] bool spawner = false;
    [SerializeField] SPAWNER_TYPE spawnerType = SPAWNER_TYPE.MELEE;    

    public float buildTimer = 0.0f;

    private bool built = false;

    private MeshFilter meshFilter;
    private Interactable interactable;
    private BoxCollider boxCollider;
    private NavMeshObstacle navMeshObstacle;

    private PlayerManager playerManager;
    private BuildingManager buildingManager;
    private AIManager aiManager;

    private UIManager uiManager;

    private List<Unit> spawnQueue = new List<Unit>();
    private List<int> spawnQueueCosts = new List<int>();
    private bool spawning = false;
    private float spawnTimer = 0.0f;

    private float health = 300;

    private int goldCost = 0;
    private int woodCost = 0;

    private GameObject healthUI;
    private Image healthBar;

    private bool enemyBuilding = false;

    [SerializeField] BUILDSTATE buildState = BUILDSTATE.PLACING;    

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        meshFilter = GetComponent<MeshFilter>();
        interactable = GetComponent<Interactable>();
        boxCollider = GetComponent<BoxCollider>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();

        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        aiManager = GameObject.Find("AI Manager").GetComponent<AIManager>();

        //if (buildState == BUILDSTATE.NOT_BUILT)
        //{
        //    DeactivateObject();
        //}

        SetCosts(GameObject.Find("UI").GetComponent<Purchasables>().GetBuildingCost(gameObject.name));

        Vector3 pos = transform.position;
        pos.y += (transform.localScale.y * 5);
        healthUI = Instantiate(healthBarPrefab, pos, Quaternion.identity);
        healthUI.transform.SetParent(transform);
        Vector3 sca = healthUI.transform.localScale;
        sca *= 5;
        healthUI.transform.localScale = sca;
        healthBar = healthUI.transform.Find("foreground").GetComponent<Image>();
        healthUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {        
        if (!built)
        {
            Construction();
        }
        else
        {
            if (spawning)
            {
                SpawnQueue();
            }
        }
    }

    private void SpawnQueue()
    {
        if (spawnQueue.Count > 0)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer > spawnQueue[0].GetSpawnTime())
            {
                spawnQueue[0].gameObject.SetActive(true);
                spawnQueue[0].gameObject.GetComponent<NavMeshAgent>().SetDestination(transform.position + (Vector3.forward * 5));
                spawnQueue.RemoveAt(0);
                spawnTimer = 0.0f;

                if (!enemyBuilding)
                    PlayerSkillManager.instance.NewAction(PlayerSkillManager.ACTION_TYPES.NEW_UNIT);
            }
        }
        else
        {
            spawning = false;
        }
    }
    public void NewSpawnUnit(Unit _unit, int _cost)
    {
        spawnQueue.Add(_unit);
        spawnQueueCosts.Add(_cost);
        spawning = true;
    }

    public void CancelSpawnUnit(int _id)
    {
        if (_id == 0)
        {
            spawnTimer = 0.0f;
        }

        playerManager.AddGold(spawnQueueCosts[_id]);

        Destroy(spawnQueue[_id].gameObject);
        spawnQueue.RemoveAt(_id);
        spawnQueueCosts.RemoveAt(_id);
    }

    private void CancelSpawnQueue()
    {
        for (int i = 0; i < spawnQueue.Count; i++)
        {
            CancelSpawnUnit(i);
        }
    }

    private void Construction()
    {
        switch(buildState)
        {
            case BUILDSTATE.PLACING:
                break;
            case BUILDSTATE.NOT_BUILT:

                if (meshFilter.mesh != foundationMesh)
                {
                    meshFilter.mesh = foundationMesh;
                }

                buildTimer += Time.deltaTime;
                buildtimerForeground.fillAmount = buildTimer / buildTime;

                if (buildTimer > (buildTime / 2))
                {
                    buildState = BUILDSTATE.BUILDING;
                    if (meshFilter)
                    {
                        meshFilter.mesh = constructionMesh;
                    }

                }
                break;
            case BUILDSTATE.BUILDING:

                if (meshFilter.mesh != constructionMesh)
                {
                    meshFilter.mesh = constructionMesh;
                }

                buildTimer += Time.deltaTime;
                buildtimerForeground.fillAmount = buildTimer / buildTime;

                if (buildTimer > buildTime)
                {
                    if (increasePopulation)
                    {
                        if (!enemyBuilding)
                        {
                            playerManager.NewHouse();
                        }
                        else
                        {
                            aiManager.NewHouse();
                        }
                    }

                    if (collector)
                    {
                        GetComponent<ResourceCollection>().enabled = true;
                    }

                    buildState = BUILDSTATE.FINISHED;

                    if (meshFilter)
                    {
                        meshFilter.mesh = finishedMesh;
                    }
                }
                break;
            case BUILDSTATE.FINISHED:
                if (meshFilter.mesh != finishedMesh)
                {
                    meshFilter.mesh = finishedMesh;

                    buildtimerBackground.enabled = false;
                    buildtimerForeground.enabled = false;

                    built = true;
                }
                return;
        }
    }

    private void DestroyBuilding(bool _refund)
    {
        if (_refund)
        {
            playerManager.AddGold(goldCost);
            playerManager.AddWood(woodCost);
        }

        if (increasePopulation)
        {
            if (!enemyBuilding)
            {
                playerManager.DestroyedHouse();
            }
            else
            {
                aiManager.DestroyedHouse();
            }
        }

        if (!enemyBuilding)
        {
            PlayerSkillManager.instance.BuildingLost(buildingType);
        }
        else
        {
            PlayerSkillManager.instance.EnemyBuildingKill();
        }

        CancelSpawnQueue();

        Destroy(this.gameObject);
    }

    public void ActivateObject()
    {
        boxCollider.enabled = true;
        navMeshObstacle.enabled = true;
        interactable.enabled = true;
        buildState = BUILDSTATE.NOT_BUILT;
    }

    public void DeactivateObject()
    {
        //boxCollider.enabled = false;
        navMeshObstacle.enabled = false;
        interactable.enabled = false;
    }

    private void OnMouseDown()
    {
        if (!enemyBuilding)
        {
            if (buildState == BUILDSTATE.FINISHED)
            {
                buildingManager.SetActiveBuilding(this);

                string spawn = "buildings";

                if (spawner)
                {
                    switch (spawnerType)
                    {
                        case SPAWNER_TYPE.MELEE:
                            spawn = "barracks";
                            break;
                        case SPAWNER_TYPE.MAGIC:
                            spawn = "magetower";
                            break;
                        case SPAWNER_TYPE.MOUNTED:
                            spawn = "stables";
                            break;
                        case SPAWNER_TYPE.RANGED:
                            spawn = "archery";
                            break;
                    }                    
                }
                else
                {
                    switch (buildingType)
                    {
                        case PlayerSkillManager.BUILDING_TYPE.TECHNOLOGY:
                            spawn = "research";
                            break;
                    }
                }
                uiManager.ActivatePane(spawn);
            }
        }
    }

    public void SetBuildingCost(int _gold, int _wood)
    {
        goldCost = _gold;
        woodCost = _wood;
    }

    public void TakeDamage(float _dam)
    {
        health -= _dam;

        healthUI.SetActive(true);
        healthBar.fillAmount = health / maxHealth;

        if (health < 0)
        {
            DestroyBuilding(false);
        }
    }

    public Vector3 GetUnitSpawnPos()
    {
        Vector3 pos = transform.position + Vector3.forward;
        return pos;
    }

    public List<Unit> GetSpawnQueue()
    {
        return spawnQueue;
    }

    public float GetSpawnTimerP()
    {
        if (spawnQueue.Count > 0)
        {
            return spawnTimer / spawnQueue[0].GetSpawnTime();
        }

        return 0;
    }

    public void SetBuildState(BUILDSTATE _state)
    {
        buildState = _state;
    }

    public void SetMaterial(Material _mat)
    {
        GetComponent<MeshRenderer>().material = _mat;
    }

    public void SetEnemyBuilding()
    {
        gameObject.tag = "EnemyBuilding";

        gameObject.layer = LayerMask.NameToLayer("Enemy");

        if (GetComponent<ResourceCollection>())
        {
            GetComponent<ResourceCollection>().SetEnemyBuilding();
        }

        enemyBuilding = true;

        buildtimerBackground.enabled = false;
        buildtimerForeground.enabled = false;
    }

    public void SetCosts(List<int> _costs)
    {
        goldCost = _costs[0];
        woodCost = _costs[1];
    }

    public int GetGoldCost()
    {
        return goldCost;
    }

    public int GetWoodCost()
    {
        return woodCost;
    }

    public bool GetSpawner()
    {
        return spawner;
    }

    public SPAWNER_TYPE GetBuildType()
    {
        return spawnerType;
    }

    public BUILDSTATE GetBuildState()
    {
        return buildState;
    }

    public bool GetBuilt()
    {
        return built;
    }

    public bool GetCollector()
    {
        return collector;
    }

    public PlayerSkillManager.BUILDING_TYPE GetBuildingType()
    {
        return buildingType;
    }
}

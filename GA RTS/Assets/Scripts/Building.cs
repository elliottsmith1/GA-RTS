﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

public class Building : MonoBehaviour
{
    private enum BUILDSTATE
    {
        PLACING,
        NOT_BUILT,
        BUILDING,
        FINISHED
    }

    private enum SPAWNERTYPE
    {
        MELEE,
        RANGED,
        MAGIC,
        MOUNTED
    }

    [SerializeField] Image buildtimerBackground;
    [SerializeField] Image buildtimerForeground;

    [SerializeField] Mesh foundationMesh;
    [SerializeField] Mesh constructionMesh;
    [SerializeField] Mesh finishedMesh;

    [SerializeField] float buildTime = 10.0f;

    [SerializeField] bool increasePopulation = false;

    [SerializeField] bool collector = false;
    [SerializeField] bool spawner = false;
    [SerializeField] SPAWNERTYPE spawnerType = SPAWNERTYPE.MELEE;

    public float buildTimer = 0.0f;

    private bool built = false;

    private MeshFilter meshFilter;
    private Interactable interactable;
    private BoxCollider boxCollider;
    private NavMeshObstacle navMeshObstacle;

    private PlayerManager playerManager;
    private BuildingManager buildingManager;

    private UIManager uiManager;

    private List<Unit> spawnQueue = new List<Unit>();
    private List<int> spawnQueueCosts = new List<int>();
    private bool spawning = false;
    private float spawnTimer = 0.0f;

    private float health = 300;

    private int goldCost = 0;
    private int woodCost = 0;

    [SerializeField] BUILDSTATE buildState = BUILDSTATE.PLACING;    

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        interactable = GetComponent<Interactable>();
        boxCollider = GetComponent<BoxCollider>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();

        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        if (buildState == BUILDSTATE.NOT_BUILT)
        {
            DeactivateObject();
        }
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
                        playerManager.NewHouse();
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

        Destroy(this.gameObject);
    }

    public void ActivateObject()
    {
        boxCollider.enabled = true;
        navMeshObstacle.enabled = true;
        interactable.enabled = true;
        buildState = BUILDSTATE.NOT_BUILT;
    }

    private void DeactivateObject()
    {
        boxCollider.enabled = false;
        navMeshObstacle.enabled = false;
        interactable.enabled = false;
    }

    private void OnMouseDown()
    {
        if (buildState == BUILDSTATE.FINISHED)
        {
            buildingManager.SetActiveBuilding(this);
            if (spawner)
            {
                string spawn = "buildings";

                switch (spawnerType)
                {
                    case SPAWNERTYPE.MELEE:
                        spawn = "barracks";
                        break;
                    case SPAWNERTYPE.MAGIC:
                        spawn = "magetower";
                        break;
                    case SPAWNERTYPE.MOUNTED:
                        spawn = "stables";
                        break;
                    case SPAWNERTYPE.RANGED:
                        spawn = "archery";
                        break;
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

    //private void OnMouseOver()
    //{
    //    if (buildState != BUILDSTATE.FINISHED)
    //    {
    //        buildtimerBackground.enabled = true;
    //        buildtimerForeground.enabled = true;
    //    }
    //}

    //private void OnMouseExit()
    //{
    //    buildtimerBackground.enabled = false;
    //    buildtimerForeground.enabled = false;
    //}
}

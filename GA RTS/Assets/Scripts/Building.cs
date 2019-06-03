using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Building : MonoBehaviour
{
    private enum BUILDSTATE
    {
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

    [SerializeField] bool spawner = false;
    [SerializeField] SPAWNERTYPE spawnerType = SPAWNERTYPE.MELEE;

    public float buildTimer = 0.0f;

    private bool built = false;

    private MeshFilter meshFilter;
    private Interactable interactable;
    private BoxCollider boxCollider;
    private NavMeshObstacle navMeshObstacle;
    private BuildingManager buildingManager;

    private UIManager uiManager;

    [SerializeField] BUILDSTATE buildState = BUILDSTATE.NOT_BUILT;    

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        interactable = GetComponent<Interactable>();
        boxCollider = GetComponent<BoxCollider>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();

        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();

        DeactivateObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (!built)
        {
            Construction();
        }
    }

    private void Construction()
    {
        switch(buildState)
        {
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

    public void ActivateObject()
    {
        boxCollider.enabled = true;
        navMeshObstacle.enabled = true;
        interactable.enabled = true;
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

    public Vector3 GetUnitSpawnPos()
    {
        Vector3 pos = transform.position + Vector3.forward;
        Debug.Log(transform.position + " " + Vector3.forward);
        return pos;
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

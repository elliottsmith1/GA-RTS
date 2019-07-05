using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] Purchasables purchasables;

    [SerializeField] GameObject selectedBuilding;   

    [SerializeField] float distanceAllowance = 5.0f;

    public bool holdingObject = false;
    private bool rotatingObject = false;
    private bool canPlace = false;

    private Camera cam;
    private Outline outline;

    private Building activeBuilding;

    private int selectedBuildingGoldCost = 0;
    private int selectedBuildingWoodCost = 0;

    private List<GameObject> playerBuildings = new List<GameObject>();

    [SerializeField] PlayerManager playerManager;

    public enum BUILDINGS
    {
        BARRACKS,
        ARCHERY
    }

    [Header("Building prefabs")]
    [SerializeField] GameObject barracks;
    [SerializeField] GameObject archeryRange;
    [SerializeField] GameObject house;
    [SerializeField] GameObject lumberMill;
    [SerializeField] GameObject market;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        playerBuildings.Add(GameObject.Find("Player TownHall"));
    }

    // Update is called once per frame
    void Update()
    {
        playerBuildings.RemoveAll(item => item == null);

        if (holdingObject)
        {
            Vector3 point = new Vector3(0, 0, 0);

            RaycastHit[] hits;

            hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));

            foreach (RaycastHit h in hits)
            {
                if (h.transform.tag == "Terrain")
                {
                    point = h.point;

                    NavMeshHit nHit;

                    if (NavMesh.FindClosestEdge(point, out nHit, NavMesh.AllAreas))
                    {
                        selectedBuilding.transform.position = point;

                        if (nHit.distance > distanceAllowance)
                        {
                            canPlace = true;
                            outline.OutlineColor = Color.green;
                        }
                        else
                        {
                            canPlace = false;
                            outline.OutlineColor = Color.red;
                        }
                        break;
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Destroy(selectedBuilding);
                holdingObject = false;
                canPlace = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (canPlace)
                {
                    playerManager.AddGold(-selectedBuildingGoldCost);
                    playerManager.AddWood(-selectedBuildingWoodCost);

                    holdingObject = false;
                    canPlace = false;
                    rotatingObject = true;
                    return;
                }
            }
        }

        if (rotatingObject)
        {
            float rot = Input.GetAxis("Rotate");
            float rotateSpeed = 3.0f;

            if (rot > 0.1f)
            {
                selectedBuilding.transform.Rotate(0, rotateSpeed, 0, Space.World);
            }
            else if (rot < 0.0f)
            {
                selectedBuilding.transform.Rotate(0, -rotateSpeed, 0, Space.World);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Building build = selectedBuilding.GetComponent<Building>();
                build.enabled = true;
                outline.OutlineColor = Color.white;
                build.ActivateObject();

                playerBuildings.Add(selectedBuilding);

                selectedBuilding = null;
                holdingObject = false;
                rotatingObject = false;
            }
        }
    }

    public void SelectBuilding(string _building)
    {
        holdingObject = true;
        Quaternion rot = new Quaternion(-45, 0, 0, 0);
        switch (_building)
        {
            case "BARRACKS":
                SetSelectedBuildingCost(purchasables.barracksGoldCost, purchasables.barracksWoodCost);
                selectedBuilding = Instantiate(barracks, transform);
                break;
            case "ARCHERY":
                SetSelectedBuildingCost(purchasables.archeryGoldCost, purchasables.archeryWoodCost);
                selectedBuilding = Instantiate(archeryRange, transform);
                break;
            case "HOUSE":
                SetSelectedBuildingCost(purchasables.houseGoldCost, purchasables.houseWoodCost);
                selectedBuilding = Instantiate(house, transform);
                break;
            case "LUMBER":
                SetSelectedBuildingCost(purchasables.lumberGoldCost, purchasables.lumberWoodCost);
                selectedBuilding = Instantiate(lumberMill, transform);
                break;
            case "MARKET":
                SetSelectedBuildingCost(purchasables.marketGoldCost, purchasables.marketWoodCost);                
                selectedBuilding = Instantiate(market, transform);
                break;
        }
        outline = selectedBuilding.GetComponent<Outline>();
        outline.enabled = true;

        BoxCollider box = selectedBuilding.GetComponent<BoxCollider>();

        if (box.size.x > box.size.z)
        {
            distanceAllowance = box.size.x / 2;
        }
        else
        {
            distanceAllowance = box.size.z / 2;
        }
    }

    private void SetSelectedBuildingCost(int _gold, int _wood)
    {
        selectedBuildingGoldCost = _gold;
        selectedBuildingWoodCost = _wood;
    }

    public void SetActiveBuilding(Building _b)
    {
        activeBuilding = _b;
    }

    public Building GetActiveBuilding()
    {
        return activeBuilding;
    }

    public List<GameObject> GetPlayerBuildings()
    {
        return playerBuildings;
    }
}



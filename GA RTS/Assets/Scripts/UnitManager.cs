using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    public List<GameObject> allUnits = new List<GameObject>();
    public List<Unit> selectedUnits = new List<Unit>();

    [SerializeField] BuildingManager buildingManager;

    [SerializeField] GameObject archerPrefab;
    [SerializeField] GameObject crossbowmanPrefab;
    [SerializeField] GameObject heavyInfantryPrefab;
    [SerializeField] GameObject infantryPrefab;
    [SerializeField] GameObject magePrefab;
    [SerializeField] GameObject pikemanPrefab;
    [SerializeField] GameObject spearmanPrefab;
    [SerializeField] GameObject mountedArcherPrefab;
    [SerializeField] GameObject mountedInfantryPrefab;
    [SerializeField] GameObject mountedSpearmanPrefab;
    [SerializeField] GameObject mountedMagePrefab;

    public enum SPEARS
    {
        SPEAR,
        LANCE1,
        LANCE2,
        LANCE3,
        CAV_SPEAR
    }
    public enum STAFFS
    {
        STAFF1,
        STAFF2,
        STAFF3,
        STAFF4
    }
    public enum POLEARMS
    {
        BARDICHE,
        GLAIVE,
        HALBERD
    }
    public enum ONE_HANDS
    {
        AXE1,
        AXE2,
        AXE3,
        AXE4,
        BROAD_SWORD,
        CLUB,
        DAGGER1,
        DAGGER2,
        HAMMER,
        WARHAMMER,
        MACE,
        MAUL,
        MORNING_STAR,
        PICKAXE,
        SHORT_SWORD,
        SWORD
    }
    public enum TWO_HANDS
    {
        AXE,
        AXE2,
        CLAYMORE,
        CLUB,
        MAUL,
        SWORD,
        WARHAMMER
    }
    public enum BOWS
    {
        LONG,
        RECURVE,
        SHORT
    }
    public enum CROSSBOWS
    {
        CROSSBOW
    }
    public enum SHIELDS
    {
        SHIELD1,
        SHIELD2,
        SHIELD3,
        SHIELD4,
        SHIELD5,
        SHIELD6,
        SHIELD7,
        SHIELD8,
        SHIELD9,
        SHIELD10,
        SHIELD11,
        SHIELD12,
        SHIELD13,
        SHIELD14,
        SHIELD15,
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        allUnits.RemoveAll(item => item == null);
        selectedUnits.RemoveAll(item => item == null);

        DetectInput();
    }

    private void DetectInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedUnits.Count > 0)
            {
                int rowNum = (int)Mathf.Sqrt(selectedUnits.Count);
                int rowCounter = 0;

                Vector3 point = new Vector3(0, 0, 0);

                RaycastHit[] hits;

                hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));

                foreach (RaycastHit h in hits)
                {
                    if (h.transform.tag == "Terrain")
                    {
                        point = h.point;

                        if (h.transform.tag == "Terrain")
                        {
                            float offset = 2.0f;

                            point.z -= (offset * (rowNum / 2));
                            point.x -= (offset * (rowNum / 2));

                            Vector3 newPos = point;


                            foreach (Unit unit in selectedUnits)
                            {
                                bool manual = false;

                                //if (unit.GetTarget())
                                //{
                                //    manual = true;
                                //}

                                unit.NewDestination(newPos, manual);

                                newPos.x += offset;

                                rowCounter++;

                                if (rowCounter >= rowNum)
                                {
                                    rowCounter = 0;
                                    newPos.x = point.x;
                                    newPos.z += offset;
                                }
                            }
                        }

                        break;
                    }
                }

                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //RaycastHit hit;
                //if (Physics.Raycast(ray, out hit))
                //{
                //    point = hit.point;
                //}                
            }
        }
    }

    public void SpawnUnit(string _unit)
    {
        Vector3 pos = buildingManager.GetActiveBuilding().GetUnitSpawnPos();
        GameObject prefab = infantryPrefab;

        switch (_unit)
        {
            case "infantry":
                prefab = infantryPrefab;
                break;
            case "archer":
                prefab = archerPrefab;
                break;
            case "crossbowman":
                prefab = crossbowmanPrefab;
                break;
            case "spearman":
                prefab = spearmanPrefab;
                break;
            case "pikeman":
                prefab = pikemanPrefab;
                break;
            case "mage":
                prefab = magePrefab;
                break;
            case "heavyinfantry":
                prefab = heavyInfantryPrefab;
                break;
            case "mountedinfantry":
                prefab = mountedInfantryPrefab;
                break;
            case "mountedarcher":
                prefab = mountedArcherPrefab;
                break;
            case "mountedmage":
                prefab = mountedMagePrefab;
                break;
            case "mountedspearman":
                prefab = mountedSpearmanPrefab;
                break;
        }

        GameObject unit = Instantiate(prefab, pos, Quaternion.identity);
        unit.GetComponent<NavMeshAgent>().SetDestination(pos + (Vector3.forward*5));
    }

    public void NewUnit(GameObject _unit)
    {
        allUnits.Add(_unit);
    }

    public void SelectUnit(Unit _unit)
    {
        selectedUnits.Add(_unit);
    }

    public List<GameObject> GetAllUnits()
    {
        return allUnits;
    }

    public void DeselectSelection()
    {
        foreach(Unit unit in selectedUnits)
        {
            unit.gameObject.GetComponent<Outline>().enabled = false;
        }

        selectedUnits.Clear();
    }
}

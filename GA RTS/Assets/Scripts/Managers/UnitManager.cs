using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    public List<GameObject> allUnits = new List<GameObject>();
    public List<Unit> allUnitsU = new List<Unit>();
    public List<Unit> selectedUnits = new List<Unit>();

    [SerializeField] Purchasables purchasables;
    [SerializeField] BuildingManager buildingManager;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] TechnologyManager techManager;

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

    private int selectedUnitPopCost = 0;
    private int selectedUnitGoldCost = 0;

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
        allUnitsU.RemoveAll(item => item == null);
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
                            float offset = 1.2f;

                            point.z -= (offset * (rowNum / 2));
                            point.x -= (offset * (rowNum / 2));

                            Vector3 newPos = point;


                            foreach (Unit unit in selectedUnits)
                            {
                                bool manual = false;

                                if (unit.GetTarget())
                                {
                                    manual = true;
                                }

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

                            PlayerSkillManager.instance.NewAction(PlayerSkillManager.ACTION_TYPES.MOVE_UNITS);
                        }

                        break;
                    }
                }        
            }
        }
    }

    public void SpawnUnit(string _unit)
    {
        if (buildingManager.GetActiveBuilding().GetSpawnQueue().Count < 6)
        {
            Vector3 pos = buildingManager.GetActiveBuilding().GetUnitSpawnPos();
            GameObject prefab = infantryPrefab;

            switch (_unit)
            {
                case "infantry":
                    playerManager.AddGold(-purchasables.infantryGoldCost);
                    selectedUnitPopCost = purchasables.infantryPopCost;
                    selectedUnitGoldCost = purchasables.infantryGoldCost;
                    prefab = infantryPrefab;
                    break;
                case "archer":
                    playerManager.AddGold(-purchasables.archerGoldCost);
                    selectedUnitPopCost = purchasables.archerPopCost;
                    selectedUnitGoldCost = purchasables.archerGoldCost;
                    prefab = archerPrefab;
                    break;
                case "crossbowman":
                    playerManager.AddGold(-purchasables.crossbowmanGoldCost);
                    selectedUnitPopCost = purchasables.crossbowmanPopCost;
                    selectedUnitGoldCost = purchasables.crossbowmanGoldCost;
                    prefab = crossbowmanPrefab;
                    break;
                case "spearman":
                    playerManager.AddGold(-purchasables.spearmanGoldCost);
                    selectedUnitPopCost = purchasables.spearmanPopCost;
                    selectedUnitGoldCost = purchasables.spearmanGoldCost;
                    prefab = spearmanPrefab;
                    break;
                case "pikeman":
                    playerManager.AddGold(-purchasables.pikemanGoldCost);
                    selectedUnitPopCost = purchasables.pikemanPopCost;
                    selectedUnitGoldCost = purchasables.pikemanGoldCost;
                    prefab = pikemanPrefab;
                    break;
                case "mage":
                    playerManager.AddGold(-purchasables.infantryGoldCost);
                    selectedUnitPopCost = purchasables.infantryPopCost;
                    selectedUnitGoldCost = purchasables.infantryGoldCost;
                    prefab = magePrefab;
                    break;
                case "heavyinfantry":
                    playerManager.AddGold(-purchasables.heavyInfantryGoldCost);
                    selectedUnitPopCost = purchasables.heavyInfantryPopCost;
                    selectedUnitGoldCost = purchasables.heavyInfantryGoldCost;
                    prefab = heavyInfantryPrefab;
                    break;
                case "mountedinfantry":
                    playerManager.AddGold(-purchasables.infantryGoldCost);
                    selectedUnitPopCost = purchasables.infantryPopCost;
                    selectedUnitGoldCost = purchasables.infantryGoldCost;
                    prefab = mountedInfantryPrefab;
                    break;
                case "mountedarcher":
                    playerManager.AddGold(-purchasables.infantryGoldCost);
                    selectedUnitPopCost = purchasables.infantryPopCost;
                    selectedUnitGoldCost = purchasables.infantryGoldCost;
                    prefab = mountedArcherPrefab;
                    break;
                case "mountedmage":
                    playerManager.AddGold(-purchasables.infantryGoldCost);
                    selectedUnitPopCost = purchasables.infantryPopCost;
                    selectedUnitGoldCost = purchasables.infantryGoldCost;
                    prefab = mountedMagePrefab;
                    break;
                case "mountedspearman":
                    playerManager.AddGold(-purchasables.infantryGoldCost);
                    selectedUnitPopCost = purchasables.infantryPopCost;
                    selectedUnitGoldCost = purchasables.infantryGoldCost;
                    prefab = mountedSpearmanPrefab;
                    break;
            }

            if ((playerManager.GetPopulation() + selectedUnitPopCost) <= playerManager.GetPopulationLimit())
            {
                GameObject unit = Instantiate(prefab, pos, Quaternion.identity);
                playerManager.AddPopulation(selectedUnitPopCost);
                NewUnit(unit);
                unit.SetActive(false);
                Unit unitScript = unit.GetComponent<Unit>();
                unitScript.SetPopulationValue(selectedUnitPopCost);
                if (unitScript.GetMelee())
                {
                    unitScript.IncreaseDamage(techManager.GetTechLevel("meleeDamage") * techManager.GetBuffAmount());
                    unitScript.IncreaseArmour(techManager.GetTechLevel("meleeArmour") * techManager.GetBuffAmount());
                }
                else
                {
                    unitScript.IncreaseDamage(techManager.GetTechLevel("rangedDamage") * techManager.GetBuffAmount());
                    unitScript.IncreaseArmour(techManager.GetTechLevel("rangedArmour") * techManager.GetBuffAmount());
                }
                buildingManager.GetActiveBuilding().NewSpawnUnit(unit.GetComponent<Unit>(), selectedUnitGoldCost);                
            }
        }
    }

    public void NewUnit(GameObject _unit)
    {
        allUnits.Add(_unit);
        allUnitsU.Add(_unit.GetComponent<Unit>());
    }

    public void RemoveUnit(GameObject _unit)
    {
        allUnits.Remove(_unit);

        if (selectedUnits.Count > 0)
        {
            foreach (Unit unit in selectedUnits)
            {
                if (unit.gameObject == _unit)
                {
                    selectedUnits.Remove(unit);
                }
            }
        }
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

    public int GetArmyPopulation()
    {
        int pop = 0;

        foreach (Unit unit in allUnitsU)
        {
            pop += unit.GetPopulationValue();
        }

        return pop;
    }
}

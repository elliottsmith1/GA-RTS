using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyManager : MonoBehaviour
{
    private bool isPlayer = false;
    private PlayerManager playerManager;
    private BuildingManager buildingManager;
    private Purchasables purchasables;

    //buff tech
    private int meleeDamage = 1;
    private int meleeDefence = 1;

    private int rangedDamage = 1;
    private int rangedDefence = 1;

    private int maxBuff = 4;

    //unlock tech
    private int armourTech = 1;
    private int weaponTech = 1;
    private int rangedTech = 1;

    private int maxMeleeTech = 3;
    private int maxRangedTech = 2;

    //current research
    private bool researchingMeleeDam = false;
    private bool researchingMeleeDef = false;
    private bool researchingRangedDam = false;
    private bool researchingRangedDef = false;
    private bool researchingWeaponTech = false;
    private bool researchingArmourTech = false;
    private bool researchingRangedTech= false;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<PlayerManager>())
        {
            isPlayer = true;
            playerManager = GetComponent<PlayerManager>();
            buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
            purchasables = playerManager.GetPurchasables();
        }
    }

    public void StartResearch(string _tech)
    {
        if (!buildingManager.GetActiveResearchBuilding().GetTechManager())
            buildingManager.GetActiveResearchBuilding().SetTechManager(this);

        if (buildingManager.GetActiveResearchBuilding().GetIsResearching())
            return;

        switch (_tech)
        {
            case "meleeDamage":
                buildingManager.GetActiveResearchBuilding().NewResearch(purchasables.GetResearchTime(meleeDamage), "meleeDamage");
                researchingMeleeDam = true;                
                playerManager.AddGold(-Mathf.RoundToInt(purchasables.GetUpgradeCost(meleeDamage)));
                playerManager.AddWood(-Mathf.RoundToInt(purchasables.GetUpgradeCost(meleeDamage)));
                break;
            case "meleeDefence":
                buildingManager.GetActiveResearchBuilding().NewResearch(purchasables.GetResearchTime(meleeDefence), "meleeDefence");
                researchingMeleeDef = true;
                playerManager.AddGold(-Mathf.RoundToInt(purchasables.GetUpgradeCost(meleeDefence)));
                playerManager.AddWood(-Mathf.RoundToInt(purchasables.GetUpgradeCost(meleeDefence)));
                break;
            case "rangedDamage":
                buildingManager.GetActiveResearchBuilding().NewResearch(purchasables.GetResearchTime(rangedDamage), "rangedDamage");
                researchingRangedDam = true;
                playerManager.AddGold(-Mathf.RoundToInt(purchasables.GetUpgradeCost(rangedDamage)));
                playerManager.AddWood(-Mathf.RoundToInt(purchasables.GetUpgradeCost(rangedDamage)));
                break;
            case "rangedDefence":
                buildingManager.GetActiveResearchBuilding().NewResearch(purchasables.GetResearchTime(rangedDefence), "rangedDefence");
                researchingRangedDef = true;
                playerManager.AddGold(-Mathf.RoundToInt(purchasables.GetUpgradeCost(rangedDefence)));
                playerManager.AddWood(-Mathf.RoundToInt(purchasables.GetUpgradeCost(rangedDefence)));
                break;

            case "armourTech":
                buildingManager.GetActiveResearchBuilding().NewResearch(purchasables.GetResearchTime(armourTech), "armourTech");
                researchingArmourTech = true;
                playerManager.AddGold(-Mathf.RoundToInt(purchasables.GetUpgradeCost(armourTech)));
                playerManager.AddWood(-Mathf.RoundToInt(purchasables.GetUpgradeCost(armourTech)));
                break;
            case "weaponTech":
                buildingManager.GetActiveResearchBuilding().NewResearch(purchasables.GetResearchTime(weaponTech), "weaponTech");
                researchingWeaponTech = true;
                playerManager.AddGold(-Mathf.RoundToInt(purchasables.GetUpgradeCost(weaponTech)));
                playerManager.AddWood(-Mathf.RoundToInt(purchasables.GetUpgradeCost(weaponTech)));
                break;
            case "rangedTech":
                buildingManager.GetActiveResearchBuilding().NewResearch(purchasables.GetResearchTime(rangedTech), "rangedTech");
                researchingRangedTech = true;
                playerManager.AddGold(-Mathf.RoundToInt(purchasables.GetUpgradeCost(rangedTech)));
                playerManager.AddWood(-Mathf.RoundToInt(purchasables.GetUpgradeCost(rangedTech)));
                break;
        }
    }

    public void UnlockTech(string _tech)
    {
        switch (_tech)
        {
            case "meleeDamage":
                meleeDamage++;
                if (meleeDamage < maxBuff)
                    researchingMeleeDam = false;
                break;
            case "meleeDefence":
                meleeDefence++;
                if (meleeDefence < maxBuff)
                    researchingMeleeDef = false;
                break;
            case "rangedDamage":
                rangedDamage++;
                if (rangedDamage < maxBuff)
                    researchingRangedDam = false;
                break;
            case "rangedDefence":
                rangedDefence++;
                if (rangedDefence < maxBuff)
                    researchingRangedDef = false;
                break;

            case "armourTech":
                armourTech++;
                if (armourTech < maxMeleeTech)
                    researchingArmourTech = false;
                break;
            case "weaponTech":
                weaponTech++;
                if (weaponTech < maxMeleeTech)
                    researchingWeaponTech = false;
                break;
            case "rangedTech":
                rangedTech++;
                if (rangedTech < maxRangedTech)
                    researchingRangedTech = false;
                break;
        }

        if (isPlayer)
            playerManager.NewTech();
    }

    public int GetTechLevel(string _tech)
    {
        switch (_tech)
        {
            case "meleeDamage":
                return meleeDamage;
            case "meleeDefence":
                return meleeDefence;
            case "rangedDamage":
                return rangedDamage;
            case "rangedDefence":
                return rangedDefence;

            case "armourTech":
                return armourTech;
            case "weaponTech":
                return weaponTech;
            case "rangedTech":
                return rangedTech;
        }

        return 1;
    }

    public void SetCurrentResearch(string _tech, bool _current)
    {
        switch (_tech)
        {
            case "meleeDamage":
                researchingMeleeDam = _current;
                break;
            case "meleeDefence":
                researchingMeleeDef = _current;
                break;
            case "rangedDamage":
                researchingRangedDam = _current;
                break;
            case "rangedDefence":
                researchingRangedDef = _current;
                break;

            case "armourTech":
                researchingArmourTech = _current;
                break;
            case "weaponTech":
                researchingWeaponTech = _current;
                break;
            case "rangedTech":
                researchingRangedTech = _current;
                break;
        }
    }

    public bool GetCurrentResearch(string _tech)
    {
        switch (_tech)
        {
            case "meleeDamage":
                return researchingMeleeDam;
            case "meleeDefence":
                return researchingMeleeDef;
            case "rangedDamage":
                return researchingRangedDam;
            case "rangedDefence":
                return researchingRangedDef;

            case "armourTech":
                return researchingArmourTech;
            case "weaponTech":
                return researchingWeaponTech;
            case "rangedTech":
                return researchingRangedTech;
        }

        return true;
    }

    public void ResearchProgress(string _tech, float _progress)
    {
        purchasables.UpdateResearchTimer(_tech, _progress);        
    }
}

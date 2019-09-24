using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Purchasables : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] TechnologyManager playerTechManager;

    [Header("Costs")]

    [Header("Tech")]
    public float levelOneCost = 50.0f;

    public float levelOneResearchTime = 40.0f;

    [Header("Buildings")]
    public int barracksWoodCost = 20;
    public int barracksGoldCost = 0;
    public int archeryWoodCost = 20;
    public int archeryGoldCost = 10;
    public int houseWoodCost = 10;
    public int houseGoldCost = 0;
    public int lumberWoodCost = 0;
    public int lumberGoldCost = 20;
    public int marketWoodCost = 20;
    public int marketGoldCost = 0;
    public int blacksmithWoodCost = 20;
    public int blacksmithGoldCost = 0;

    [Header("Units")]
    public int infantryGoldCost = 10;
    public int spearmanGoldCost = 15;
    public int pikemanGoldCost = 20;
    public int heavyInfantryGoldCost = 25;

    public int archerGoldCost = 15;
    public int crossbowmanGoldCost = 25;

    public int infantryPopCost = 1;
    public int spearmanPopCost = 2;
    public int pikemanPopCost = 2;
    public int heavyInfantryPopCost = 3;

    public int archerPopCost = 1;
    public int crossbowmanPopCost = 3;

    [Header("Building text references")]
    [SerializeField] Text barracksWoodCostText;
    [SerializeField] Text barracksGoldCostText;
    [SerializeField] Text archeryWoodCostText;
    [SerializeField] Text archeryGoldCostText;
    [SerializeField] Text houseWoodCostText;
    [SerializeField] Text houseGoldCostText;
    [SerializeField] Text lumberWoodCostText;
    [SerializeField] Text lumberGoldCostText;
    [SerializeField] Text marketWoodCostText;
    [SerializeField] Text marketGoldCostText;
    [SerializeField] Text blacksmithWoodCostText;
    [SerializeField] Text blacksmithGoldCostText;

    [Header("Unit text references")]
    [SerializeField] Text infantryGoldCostText;
    [SerializeField] Text spearmanGoldCostText;
    [SerializeField] Text pikemanGoldCostText;
    [SerializeField] Text heavyInfantryGoldCostText;

    [SerializeField] Text archerGoldCostText;
    [SerializeField] Text crossbowmanGoldCostText;

    [SerializeField] Text infantryPopCostText;
    [SerializeField] Text spearmanPopCostText;
    [SerializeField] Text pikemanPopCostText;
    [SerializeField] Text heavyInfantryPopCostText;

    [SerializeField] Text archerPopCostText;
    [SerializeField] Text crossbowmanPopCostText;

    [Header("Tech text references")]
    [SerializeField] Text meleeDamCostText;
    [SerializeField] Text meleeDefCostText;
    [SerializeField] Text rangedDamCostText;
    [SerializeField] Text rangedDefCostText;
    [SerializeField] Text weaponTechCostText;
    [SerializeField] Text armourTechCostText;
    [SerializeField] Text rangedTechCostText;

    [Header("Building button references")]
    [SerializeField] Button barracksButton;
    [SerializeField] Button archeryButton;
    [SerializeField] Button houseButton;
    [SerializeField] Button marketButton;
    [SerializeField] Button lumberButton;
    [SerializeField] Button blacksmithButton;

    [Header("Unit button references")]
    [SerializeField] Button infantryButton;
    [SerializeField] Button spearmanButton;
    [SerializeField] Button pikemanButton;
    [SerializeField] Button heavyInfantryButton;

    [SerializeField] Button archerButton;
    [SerializeField] Button crossbowmanButton;

    [Header("Tech button references")]
    [SerializeField] Button meleeDamButton;
    [SerializeField] Button meleeDefButton;
    [SerializeField] Button rangedDamButton;
    [SerializeField] Button rangedDefButton;
    [SerializeField] Button weaponTechButton;
    [SerializeField] Button armourTechButton;
    [SerializeField] Button rangedTechButton;

    // Start is called before the first frame update
    void Start()
    {
        SetCost();
        CheckWealth(playerManager.GetGold(), playerManager.GetWood(), playerManager.GetPopulation(), playerManager.GetPopulationLimit());
    }

    private void SetCost()
    {
        barracksWoodCostText.text = barracksWoodCost.ToString();
        barracksGoldCostText.text = barracksGoldCost.ToString();
        archeryWoodCostText.text = archeryWoodCost.ToString();
        archeryGoldCostText.text = archeryGoldCost.ToString();
        houseWoodCostText.text = houseWoodCost.ToString();
        houseGoldCostText.text = houseGoldCost.ToString();
        lumberWoodCostText.text = lumberWoodCost.ToString();
        lumberGoldCostText.text = lumberGoldCost.ToString();
        marketWoodCostText.text = marketWoodCost.ToString();
        marketGoldCostText.text = marketGoldCost.ToString();
        blacksmithWoodCostText.text = blacksmithWoodCost.ToString();
        blacksmithGoldCostText.text = blacksmithGoldCost.ToString();

        infantryGoldCostText.text = infantryGoldCost.ToString();
        spearmanGoldCostText.text = spearmanGoldCost.ToString();
        pikemanGoldCostText.text = pikemanGoldCost.ToString();
        heavyInfantryGoldCostText.text = heavyInfantryGoldCost.ToString();

        archerGoldCostText.text = archerGoldCost.ToString();
        crossbowmanGoldCostText.text = crossbowmanGoldCost.ToString();

        infantryPopCostText.text = infantryPopCost.ToString();
        spearmanPopCostText.text = spearmanPopCost.ToString();
        pikemanPopCostText.text = pikemanPopCost.ToString();
        heavyInfantryPopCostText.text = heavyInfantryPopCost.ToString();

        archerPopCostText.text = archerPopCost.ToString();
        crossbowmanPopCostText.text = crossbowmanPopCost.ToString();

        meleeDamCostText.text = levelOneCost.ToString();
        meleeDefCostText.text = levelOneCost.ToString();
        rangedDamCostText.text = levelOneCost.ToString();
        rangedDefCostText.text = levelOneCost.ToString();
        weaponTechCostText.text = levelOneCost.ToString();
        armourTechCostText.text = levelOneCost.ToString();
        rangedTechCostText.text = levelOneCost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckWealth(int _gold, int _wood, int _pop, int _maxPop)
    {
        if (_gold >= barracksGoldCost && _wood >= barracksWoodCost)
            barracksButton.interactable = true;
        else
            barracksButton.interactable = false;
        if (_gold >= archeryGoldCost && _wood >= archeryWoodCost)
            archeryButton.interactable = true;
        else
            archeryButton.interactable = false;
        if (_gold >= houseGoldCost && _wood >= houseWoodCost)
            houseButton.interactable = true;
        else
            houseButton.interactable = false;
        if (_gold >= lumberGoldCost && _wood >= lumberWoodCost)
            lumberButton.interactable = true;
        else
            lumberButton.interactable = false;
        if (_gold >= marketGoldCost && _wood >= marketWoodCost)
            marketButton.interactable = true;
        else
            marketButton.interactable = false;
        if (_gold >= blacksmithGoldCost && _wood >= blacksmithWoodCost)
            blacksmithButton.interactable = true;
        else
            blacksmithButton.interactable = false;

        if (_gold >= infantryGoldCost && infantryPopCost + _pop <= _maxPop)
            infantryButton.interactable = true;
        else
            infantryButton.interactable = false;
        if (_gold >= spearmanGoldCost && spearmanPopCost + _pop <= _maxPop && playerTechManager.GetTechLevel("weaponTech") > 1 && playerTechManager.GetTechLevel("armourTech") > 1)
            spearmanButton.interactable = true;
        else
            spearmanButton.interactable = false;
        if (_gold >= pikemanGoldCost && pikemanPopCost + _pop <= _maxPop && playerTechManager.GetTechLevel("weaponTech") > 2 && playerTechManager.GetTechLevel("armourTech") > 1)
            pikemanButton.interactable = true;
        else
            pikemanButton.interactable = false;
        if (_gold >= heavyInfantryGoldCost && heavyInfantryPopCost + _pop <= _maxPop && playerTechManager.GetTechLevel("weaponTech") > 2 && playerTechManager.GetTechLevel("armourTech") > 2)
            heavyInfantryButton.interactable = true;
        else
            heavyInfantryButton.interactable = false;

        if (_gold >= archerGoldCost && archerPopCost + _pop <= _maxPop)
            archerButton.interactable = true;
        else
            archerButton.interactable = false;
        if (_gold >= crossbowmanGoldCost && crossbowmanPopCost + _pop <= _maxPop && playerTechManager.GetTechLevel("rangedTech") > 1 && playerTechManager.GetTechLevel("armourTech") > 1)
            crossbowmanButton.interactable = true;
        else
            crossbowmanButton.interactable = false;

        CheckTechWealth(_gold, _wood);
    }

    public void CheckTechWealth(int _gold, int _wood)
    {
        float meleeDam = playerTechManager.GetTechLevel("meleeDamage") * levelOneCost;
        meleeDamCostText.text = meleeDam.ToString();
        float meleeDef = playerTechManager.GetTechLevel("meleeDefence") * levelOneCost;
        meleeDefCostText.text = meleeDef.ToString();
        float rangedDam = playerTechManager.GetTechLevel("rangedDamage") * levelOneCost;
        rangedDamCostText.text = rangedDam.ToString();
        float rangedDef = playerTechManager.GetTechLevel("rangedDefence") * levelOneCost;
        rangedDefCostText.text = rangedDef.ToString();
        float weaponTech = playerTechManager.GetTechLevel("weaponTech") * levelOneCost;
        weaponTechCostText.text = weaponTech.ToString();
        float armourTech = playerTechManager.GetTechLevel("armourTech") * levelOneCost;
        armourTechCostText.text = armourTech.ToString();
        float rangedTech = playerTechManager.GetTechLevel("rangedTech") * levelOneCost;
        rangedTechCostText.text = rangedTech.ToString();

        if (_gold >= meleeDam && _wood >= meleeDam && !playerTechManager.GetCurrentResearch("meleeDamage"))
            meleeDamButton.interactable = true;
        else
            meleeDamButton.interactable = false;
        if (_gold >= meleeDef && _wood >= meleeDef && !playerTechManager.GetCurrentResearch("meleeDefence"))
            meleeDefButton.interactable = true;
        else
            meleeDefButton.interactable = false;
        if (_gold >= rangedDam && _wood >= rangedDam && !playerTechManager.GetCurrentResearch("rangedDamage"))
            rangedDamButton.interactable = true;
        else
            rangedDamButton.interactable = false;
        if (_gold >= rangedDef && _wood >= rangedDef && !playerTechManager.GetCurrentResearch("rangedDefence"))
            rangedDefButton.interactable = true;
        else
            rangedDefButton.interactable = false;

        if (_gold >= weaponTech && _wood >= weaponTech && !playerTechManager.GetCurrentResearch("weaponTech"))
            weaponTechButton.interactable = true;
        else
            weaponTechButton.interactable = false;
        if (_gold >= armourTech && _wood >= armourTech && !playerTechManager.GetCurrentResearch("armourTech"))
            armourTechButton.interactable = true;
        else
            armourTechButton.interactable = false;
        if (_gold >= rangedTech && _wood >= rangedTech && !playerTechManager.GetCurrentResearch("rangedTech"))
            rangedTechButton.interactable = true;
        else
            rangedTechButton.interactable = false;
    }

    public List<int> GetBuildingCost(string _building)
    {
        List<int> costs = new List<int>();
        int gold = 0;
        int wood = 0;

        switch (_building)
        {
            case "Barracks(Clone)":
                gold = barracksGoldCost;
                wood = barracksWoodCost;
                break;
            case "Archery(Clone)":
                gold = archeryGoldCost;
                wood = archeryWoodCost;
                break;
            case "House(Clone)":
                gold = houseGoldCost;
                wood = houseWoodCost;
                break;
            case "LumberMill(Clone)":
                gold = lumberGoldCost;
                wood = lumberWoodCost;
                break;
            case "Market(Clone)":
                gold = marketGoldCost;
                wood = marketWoodCost;
                break;
        }

        costs.Add(gold);
        costs.Add(wood);
        return costs;
    }

    public float GetUpgradeCost(int _level)
    {
        return levelOneCost * _level;
    }

    public float GetResearchTime(int _level)
    {
        return levelOneResearchTime * _level;
    }

    public void UpdateResearchTimer(string _tech, float _progress)
    {
        switch (_tech)
        {
            case "meleeDamage":
                meleeDamButton.image.fillAmount = _progress;
                break;
            case "meleeDefence":
                meleeDefButton.image.fillAmount = _progress;
                break;
            case "rangedDamage":
                rangedDamButton.image.fillAmount = _progress;
                break;
            case "rangedDefence":
                rangedDefButton.image.fillAmount = _progress;
                break;

            case "armourTech":
                armourTechButton.image.fillAmount = _progress;
                break;
            case "weaponTech":
                weaponTechButton.image.fillAmount = _progress;
                break;
            case "rangedTech":
                rangedTechButton.image.fillAmount = _progress;
                break;
        }
    }
}

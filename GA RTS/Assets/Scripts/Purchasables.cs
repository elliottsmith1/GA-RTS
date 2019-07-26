using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Purchasables : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;

    [Header("Costs")]

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

    [Header("Building button references")]
    [SerializeField] Button barracksButton;
    [SerializeField] Button archeryButton;
    [SerializeField] Button houseButton;
    [SerializeField] Button marketButton;
    [SerializeField] Button lumberButton;

    [Header("Unit button references")]
    [SerializeField] Button infantryButton;
    [SerializeField] Button spearmanButton;
    [SerializeField] Button pikemanButton;
    [SerializeField] Button heavyInfantryButton;

    [SerializeField] Button archerButton;
    [SerializeField] Button crossbowmanButton;

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

        if (_gold >= infantryGoldCost && infantryPopCost + _pop <= _maxPop)
            infantryButton.interactable = true;
        else
            infantryButton.interactable = false;
        if (_gold >= spearmanGoldCost && spearmanPopCost + _pop <= _maxPop)
            spearmanButton.interactable = true;
        else
            spearmanButton.interactable = false;
        if (_gold >= pikemanGoldCost && pikemanPopCost + _pop <= _maxPop)
            pikemanButton.interactable = true;
        else
            pikemanButton.interactable = false;
        if (_gold >= heavyInfantryGoldCost && heavyInfantryPopCost + _pop <= _maxPop)
            heavyInfantryButton.interactable = true;
        else
            heavyInfantryButton.interactable = false;

        if (_gold >= archerGoldCost && archerPopCost + _pop <= _maxPop)
            archerButton.interactable = true;
        else
            archerButton.interactable = false;
        if (_gold >= crossbowmanGoldCost && crossbowmanPopCost + _pop <= _maxPop)
            crossbowmanButton.interactable = true;
        else
            crossbowmanButton.interactable = false;

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
}

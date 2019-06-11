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
        CheckWealth(playerManager.GetGold(), playerManager.GetWood());
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckWealth(int _gold, int _wood)
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

        if (_gold >= infantryGoldCost)
            infantryButton.interactable = true;
        else
            infantryButton.interactable = false;
        if (_gold >= spearmanGoldCost)
            spearmanButton.interactable = true;
        else
            spearmanButton.interactable = false;
        if (_gold >= pikemanGoldCost)
            pikemanButton.interactable = true;
        else
            pikemanButton.interactable = false;
        if (_gold >= heavyInfantryGoldCost)
            heavyInfantryButton.interactable = true;
        else
            heavyInfantryButton.interactable = false;

        if (_gold >= archerGoldCost)
            archerButton.interactable = true;
        else
            archerButton.interactable = false;
        if (_gold >= crossbowmanGoldCost)
            crossbowmanButton.interactable = true;
        else
            crossbowmanButton.interactable = false;

    }
}

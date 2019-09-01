using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] BuildingManager buildingManager;
    [SerializeField] UnitManager unitManager;
    [SerializeField] Purchasables purchasables;

    private int populationMax = 200;
    private int currentPopulationMax = 20;
    private int population = 0;

    private int gold = 50;
    private int wood = 50;

    public string GetPopulationString()
    {
        population = unitManager.GetArmyPopulation();

        purchasables.CheckWealth(gold, wood, population, currentPopulationMax);

        string pop = population.ToString() + "/" + currentPopulationMax.ToString();
        return pop;
    }

    public void NewHouse()
    {
        currentPopulationMax += 10;

        PlayerSkillManager.instance.SetPopCapped(false);

        if (currentPopulationMax > populationMax)
        {
            currentPopulationMax = populationMax;
        }
    }

    public void DestroyedHouse()
    {
        currentPopulationMax -= 10;
    }

    public int GetPopulation()
    {
        return population;
    }

    public int GetPopulationLimit()
    {
        return currentPopulationMax;
    }

    public int GetWood()
    {
        return wood;
    }

    public Purchasables GetPurchasables()
    {
        return purchasables;
    }

    public int GetGold()
    {
        return gold;
    }

    public void AddGold(int _val)
    {
        gold += _val;
        purchasables.CheckWealth(gold, wood, population, currentPopulationMax);
    }

    public void AddWood(int _val)
    {
        wood += _val;
        purchasables.CheckWealth(gold, wood, population, currentPopulationMax);
    }

    public void NewTech()
    {
        purchasables.CheckWealth(gold, wood, population, currentPopulationMax);
    }

    public void AddPopulation(int _pop)
    {
        population += _pop;

        if (population == currentPopulationMax)
        {
            PlayerSkillManager.instance.SetPopCapped(true);
        }
    }

    public BuildingManager GetBuildingManager()
    {
        return buildingManager;
    }
}

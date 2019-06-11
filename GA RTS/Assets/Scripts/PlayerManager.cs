﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] UnitManager unitManager;

    private int populationMax = 200;
    private int currentPopulationMax = 20;
    private int population = 0;

    private int gold = 0;
    private int wood = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetPopulationString()
    {
        population = unitManager.GetArmyPopulation();

        string pop = population.ToString() + "/" + currentPopulationMax.ToString();
        return pop;
    }

    public void NewHouse()
    {
        currentPopulationMax += 10;

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

    public int GetGold()
    {
        return gold;
    }

    public void AddGold(int _val)
    {
        gold += _val;
    }

    public void AddWood(int _val)
    {
        wood += _val;
    }
}

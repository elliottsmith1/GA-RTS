using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSkillManager : MonoBehaviour
{
    public enum ACTION_TYPES
    {
        NEW_BUILDING,
        NEW_UNIT,
        SELECT_UNITS,
        SELECT_BUILDING,
        MOVE_UNITS,
        WAIT
    }

    public enum BUILDING_TYPE
    {
        MILITARY,
        ECONOMY,
        TECHNOLOGY,
        DEFENSIVE,
        POPULATION
    }

    public static PlayerSkillManager instance;

    private PlayerManager playerManager;

    private float calculationUpdateDelay = 15.0f;

    private float timeStarted = 0.0f;

    [Header("Skill")]
    [SerializeField] private float playerSkill = 0.0f;

    [Header("Performance")]

    //TOTAL
    [SerializeField] private float totalPerformance = 0.0f;

    [Header("APM")]
    //ACTIONS
    //APM
    private ACTION_TYPES lastAction;
    private float actionTimer = 0.0f;
    private float actionDelay = 1.0f;
    private List<ACTION_TYPES> actions = new List<ACTION_TYPES>();
    private List<float> APMs = new List<float>();
    [SerializeField] private float APM = 0.0f;

    [Header("Military")]
    //MILITARY
    [SerializeField] private float militaryPerformance = 0.0f;
    [SerializeField] private float unitLossPM = 0.0f;
    [SerializeField] private float enemyUnitLossPM = 0.0f;
    [SerializeField] private float enemyBuildingLossPM = 0.0f;
    [SerializeField] private float unitProductionPM = 0.0f;   

    private float newUnits = 0;
    private float unitDeaths = 0;
    private float newKill = 0;
    private float newBuildingKill = 0;
    private float newArmyValueLoss = 0.0f;
    private float newEnemyArmyValueLoss = 0.0f;

    [Header("Economy")]
    //ECONOMY
    private float newWealth = 0.0f;

    [Header("Expansion")]
    //EXPANSION
    [SerializeField] private float expansionPerformance = 0.0f;
    [SerializeField] private float buildingConstructionPM = 0.0f;
    [SerializeField] private float buildingLossPM = 0.0f;
    [SerializeField] private float totalTimePopCapped = 0.0f;
    [SerializeField] private float percentageTimePopCapped = 0.0f;

    private bool popCapped = false;
    private float newBuildings = 0;
    private float buildingLost = 0;

    [Header("Value")]

    //TOTAL
    [SerializeField] private float totalValue = 0.0f;

    [Header("Economy")]
    //VALUE
    //ECONOMY
    [SerializeField] private float economyValue = 0.0f;
    [SerializeField] private float totalWealth = 0.0f;
    [SerializeField] private float income = 0.0f;

    [Header("Military")]
    //MILITARY
    [SerializeField] private float militaryValue = 0.0f;
    [SerializeField] private float totalArmyValue = 0.0f;
    [SerializeField] private float effectiveArmyValue = 0.0f;
    [SerializeField] private float armyValueLoss = 0.0f;
    [SerializeField] private float enemyArmyValueLoss = 0.0f;
    [SerializeField] private float effectiveArmyPerformanceValue = 0.0f;

    [Header("Technology")]
    //TECHNOLOGY
    [SerializeField] private float technologyValue = 0.0f;

    [Header("Expansion")]
    //EXPANSION
    [SerializeField] private float expansionValue = 0.0f;
    [SerializeField] private float incomeCapacity = 0.0f;
    [SerializeField] private float unitProductionCapacity = 0.0f;
    [SerializeField] private float defensiveCapacity = 0.0f;
    [SerializeField] private float populationCapacity = 0.0f;
    [SerializeField] private float researchCapacity = 0.0f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();

        timeStarted = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        APMTimer();
    }

    private void APMTimer()
    {
        actionTimer += Time.deltaTime;

        if (popCapped)
        {
            totalTimePopCapped += Time.deltaTime;            
        }

        if (totalTimePopCapped > 0)
            percentageTimePopCapped = totalTimePopCapped / (Time.time - timeStarted);

        if (actionTimer > actionDelay)
        {
            actionTimer = 0.0f;
            CalculateAPM();            
        }
    }

    private void CalculateAPM()
    {
        float action = actions.Count * 60;
        APMs.Add(action);
        actions.Clear();

        lastAction = ACTION_TYPES.WAIT;

        if (APMs.Count > calculationUpdateDelay)
        {
            float sum = 0;

            for (var i = 0; i < APMs.Count; i++)
            {
                sum += APMs[i];
            }

            APM = (((sum / APMs.Count) * (60 / calculationUpdateDelay)) + APM) / 2; 
            APMs.Clear();

            CalculateMilitaryValue();
            CalculateExpansionValue();
            CalculateEconomyValue();
            CalculateTotals();
        }               
    }

    private void CalculateMilitaryValue()
    {
        unitProductionPM = (unitProductionPM + (newUnits * (60 / calculationUpdateDelay))) / 2;
        unitLossPM = (unitLossPM + (unitDeaths * (60 / calculationUpdateDelay))) / 2;
        enemyUnitLossPM = (enemyUnitLossPM + (newKill * (60 / calculationUpdateDelay))) / 2;
        enemyBuildingLossPM = (enemyBuildingLossPM + (newBuildingKill * (60 / calculationUpdateDelay))) / 2;

        armyValueLoss = (armyValueLoss + (newArmyValueLoss * (60 / calculationUpdateDelay))) / 2;
        enemyArmyValueLoss = (enemyArmyValueLoss + (newEnemyArmyValueLoss * (60 / calculationUpdateDelay))) / 2;

        if (armyValueLoss > 0)
            effectiveArmyPerformanceValue = enemyArmyValueLoss / armyValueLoss;
        else
            effectiveArmyPerformanceValue = 0.0f;

        newUnits = 0.0f;
        unitDeaths = 0.0f;
        newKill = 0.0f;
        newArmyValueLoss = 0.0f;
        newEnemyArmyValueLoss = 0.0f;
        newBuildingKill = 0.0f;
    }

    private void CalculateExpansionValue()
    {
        buildingConstructionPM = (buildingConstructionPM + (newBuildings * (60 / calculationUpdateDelay))) / 2;
        buildingLossPM = (buildingLossPM + (buildingLost * (60 / calculationUpdateDelay))) / 2;

        newBuildings = 0;
        buildingLost = 0;
    }

    private void CalculateEconomyValue()
    {
        totalWealth = playerManager.GetGold() + playerManager.GetWood();
        income = (income + (newWealth * (60 / calculationUpdateDelay))) / 2;

        newWealth = 0;
    }

    private void CalculateTotals()
    {
        //value of assets
        economyValue = (income / 10) + (totalWealth / 10);
        militaryValue = totalArmyValue;
        technologyValue = 0.0f;
        expansionValue = defensiveCapacity + incomeCapacity + populationCapacity + researchCapacity + unitProductionCapacity;        

        totalValue = economyValue + militaryValue + technologyValue + expansionValue;

        //player performance
        expansionPerformance = (buildingConstructionPM - buildingLossPM) - percentageTimePopCapped;
        if (expansionPerformance < 0)
            expansionPerformance = 0.0f;

        militaryPerformance = unitProductionPM + enemyBuildingLossPM + effectiveArmyPerformanceValue;

        totalPerformance = (militaryPerformance + expansionPerformance) * 5;

        playerSkill = APM + totalValue + totalPerformance;
    }

    public void NewAction(ACTION_TYPES _action)
    {
        if (_action == lastAction)
            return;

        lastAction = _action;
        actions.Add(lastAction);
    }

    public void newUnit(float _value)
    {
        newUnits++;
        totalArmyValue += _value;
    }

    public void UnitDeath(float _value)
    {
        unitDeaths++;
        newArmyValueLoss += _value;
        totalArmyValue -= _value;
    }

    public void EnemyDeath(float _value)
    {
        newKill++;
        newEnemyArmyValueLoss += _value;
    }

    public void BuildingLost(BUILDING_TYPE _type)
    {
        buildingLost++;

        switch (_type)
        {
            case BUILDING_TYPE.MILITARY:
                unitProductionCapacity -= 3;
                break;
            case BUILDING_TYPE.DEFENSIVE:
                defensiveCapacity -= 1;
                break;
            case BUILDING_TYPE.ECONOMY:
                incomeCapacity -= 2;
                break;
            case BUILDING_TYPE.POPULATION:
                populationCapacity -= 1;
                break;
            case BUILDING_TYPE.TECHNOLOGY:
                researchCapacity -= 4;
                break;
        }
    }

    public void EnemyBuildingKill()
    {
        newBuildingKill++;
    }

    public float GetAPM()
    {
        return APM;
    }

    public void SetPopCapped(bool _cap)
    {
        popCapped = _cap;
    }

    public void Income(float _inc)
    {
        newWealth += _inc;
    }

    public void NewBuilding(BUILDING_TYPE _type)
    {
        newBuildings++;

        switch (_type)
        {
            case BUILDING_TYPE.MILITARY:
                unitProductionCapacity += 3;
                break;
            case BUILDING_TYPE.DEFENSIVE:
                defensiveCapacity += 1;
                break;
            case BUILDING_TYPE.ECONOMY:
                incomeCapacity += 2;
                break;
            case BUILDING_TYPE.POPULATION:
                populationCapacity += 1;
                break;
            case BUILDING_TYPE.TECHNOLOGY:
                researchCapacity += 4;
                break;
        }
    }

    public float GetPlayerSkill()
    {
        return playerSkill;
    }
}

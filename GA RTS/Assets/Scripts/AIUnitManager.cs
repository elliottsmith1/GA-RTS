using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnitManager : MonoBehaviour
{
    private AIManager aiManager;

    private List<GameObject> allUnits = new List<GameObject>();
    private List<Unit> allUnitsU = new List<Unit>();
    private List<Unit> selectedUnits = new List<Unit>();

    private List<List<GameObject>> attackWaves = new List<List<GameObject>>();

    //army control
    private int maxWaveUnitNum = 2;
    private int maxWaves = 2;

    // Start is called before the first frame update
    void Start()
    {
        aiManager = GetComponent<AIManager>();   

        for (int i = 0; i < maxWaves; i++)
        {
            List<GameObject> wave = new List<GameObject>();
            attackWaves.Add(wave);
        }
    }

    // Update is called once per frame
    void Update()
    {
        allUnits.RemoveAll(item => item == null);
        allUnitsU.RemoveAll(item => item == null);
        selectedUnits.RemoveAll(item => item == null);

        foreach(List<GameObject> wave in attackWaves)
        {
            if (wave.Count > 0)
            {
                wave.RemoveAll(item => item == null);
            }
        }
    }

    private void SendAttackWave()
    {
        Vector3 attackPos = aiManager.GetPlayerBuildings()[0].transform.position;

        List<GameObject> remainingUnits = allUnits;

        foreach (List<GameObject> wave in attackWaves)
        {
            if (wave.Count > 0)
            {
                foreach(GameObject unit in wave)
                {
                    remainingUnits.Remove(unit);
                }
            }
        }

        foreach (List<GameObject> wave in attackWaves)
        {            
            if (wave.Count < 1)
            {
                for (int i = 0; i < maxWaveUnitNum; i++)
                {
                    Unit soldier = remainingUnits[i].GetComponent<Unit>();
                    soldier.NewFinalDestination(attackPos);
                    wave.Add(soldier.gameObject);
                }

                break;
            }
        }        
    }

    public void NewUnit(GameObject _soldier, Unit _unit)
    {
        allUnits.Add(_soldier);
        allUnitsU.Add(_unit);

        int remainingUnits = allUnits.Count;

        foreach(List<GameObject> wave in attackWaves)
        {
            if (wave.Count > 0)
            {
                remainingUnits -= wave.Count;
            }
        }

        if (remainingUnits >= maxWaveUnitNum)
        {
            SendAttackWave();
        }
    }

    public int GetMaxWaveNum()
    {
        return maxWaveUnitNum;
    }

    public void SetMaxWaveNum(int _num)
    {
        maxWaveUnitNum = _num;
    }

    public List<GameObject> GetUnits()
    {
        return allUnits;
    }

    public int GetMaxUnitPrep()
    {
        return maxWaveUnitNum * maxWaves;
    }

    public int GetMaxWaves()
    {
        return maxWaves;
    }

    public void SetMaxWaves(int _num)
    {
        maxWaves = _num;

        if (attackWaves.Count < maxWaves)
        {
            for (int i = attackWaves.Count; i < maxWaves; i++)
            {
                List<GameObject> wave = new List<GameObject>();
                attackWaves.Add(wave);
            }
        }
    }
}

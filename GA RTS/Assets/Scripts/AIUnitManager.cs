using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnitManager : MonoBehaviour
{
    private enum ATTACKWAVEBEHAVIOUR
    {
        WAITING,
        ATTACKING,
        RECRUITING
    }

    private AIManager aiManager;

    private List<GameObject> allUnits = new List<GameObject>();
    private List<Unit> allUnitsU = new List<Unit>();
    private List<Unit> selectedUnits = new List<Unit>();

    private List<List<GameObject>> attackWaves = new List<List<GameObject>>();
    private List<ATTACKWAVEBEHAVIOUR> attackWavesBehaviours = new List<ATTACKWAVEBEHAVIOUR>();

    //army control
    private int maxWaveUnitNum = 2;
    private int maxWaves = 2;
    private float waveTimeDelay = 10.0f;
    private float waveAttackTimer = 0.0f;
    private bool canSendWave = true;

    // Start is called before the first frame update
    void Start()
    {
        aiManager = GetComponent<AIManager>();   

        for (int i = 0; i < maxWaves; i++)
        {
            List<GameObject> wave = new List<GameObject>();
            ATTACKWAVEBEHAVIOUR behaviour = ATTACKWAVEBEHAVIOUR.RECRUITING;

            attackWaves.Add(wave);
            attackWavesBehaviours.Add(behaviour);
        }
    }

    // Update is called once per frame
    void Update()
    {
        allUnits.RemoveAll(item => item == null);
        allUnitsU.RemoveAll(item => item == null);
        selectedUnits.RemoveAll(item => item == null);

        for(int i = 0; i < attackWaves.Count; i++)
        {
            if (attackWaves[i].Count > 0)
            {
                attackWaves[i].RemoveAll(item => item == null);

                if (attackWaves[i].Count < 1)
                {
                    attackWavesBehaviours[i] = ATTACKWAVEBEHAVIOUR.RECRUITING;
                }
            }
        }

        SendAttackWave();
    }

    private void SendAttackWave()
    {
        if (!canSendWave)
        {
            waveAttackTimer += Time.deltaTime;

            if (waveAttackTimer > waveTimeDelay)
            {
                waveAttackTimer = 0.0f;
                canSendWave = true;
            }
            return;
        }

        GameObject target = aiManager.GetPlayerBuildings()[0];

        if (aiManager.GetPlayerBuildings().Count > 0)
        {
            foreach(GameObject building in aiManager.GetPlayerBuildings())
            {
                if (building != target)
                {
                    if (Vector3.Distance(aiManager.GetEnemyBuildings()[0].transform.position, building.transform.position) < 
                        Vector3.Distance(aiManager.GetEnemyBuildings()[0].transform.position, target.transform.position))
                    {
                        target = building;
                    }
                }
            }
        }
        else
        {
            return;
        }

        Vector3 attackPos = target.transform.position;

        if (attackWaves.Count > 0)
        {
            for (int i = 0; i < attackWaves.Count; i++)
            {
                bool sendWave = true;

                if (attackWaves[i].Count > 0)
                {
                    if (attackWavesBehaviours[i] != ATTACKWAVEBEHAVIOUR.ATTACKING)
                    {
                        foreach(GameObject unit in attackWaves[i])
                        {
                            if (!unit.activeInHierarchy || unit.GetComponent<Unit>().GetState() != Unit.STATE.IDLE)
                            {
                                sendWave = false;
                                break;
                            }
                        }

                        if (sendWave)
                        {
                            attackWavesBehaviours[i] = ATTACKWAVEBEHAVIOUR.ATTACKING;

                            canSendWave = false;

                            foreach (GameObject unit in attackWaves[i])
                            {
                                Unit soldier = unit.GetComponent<Unit>();
                                soldier.NewFinalDestination(attackPos);
                            }
                        }
                    }
                    else
                    {
                        bool finishedAttacking = true;
                        foreach (GameObject unit in attackWaves[i])
                        {
                            if (unit.GetComponent<Unit>().GetState() != Unit.STATE.IDLE)
                            {
                                finishedAttacking = false;
                                break;
                            }
                        }

                        if (finishedAttacking)
                        {
                            attackWavesBehaviours[i] = ATTACKWAVEBEHAVIOUR.WAITING;
                        }
                    }
                }
            }
        }
    }

    private void SetupAttackWave()
    {
        //Vector3 attackPos = aiManager.GetPlayerBuildings()[0].transform.position;

        List<GameObject> remainingUnits = new List<GameObject>(allUnits);

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
                    //soldier.NewFinalDestination(attackPos);
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
            SetupAttackWave();
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
                ATTACKWAVEBEHAVIOUR behaviour = ATTACKWAVEBEHAVIOUR.RECRUITING;

                attackWaves.Add(wave);
                attackWavesBehaviours.Add(behaviour);
            }
        }
    }
}

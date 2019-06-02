using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    enum STATE
    {
        IDLE,
        WALKING,
        RUNNING,
        FIGHTING
    }

    private Vector3 targetPosition;

    private NavMeshAgent navMeshAgent;
    private UnitManager unitManager;

    // Start is called before the first frame update
    void Start()
    {
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        unitManager.NewUnit(this.gameObject);

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewDestination(Vector3 _pos)
    {
        navMeshAgent.SetDestination(_pos);
    }
}

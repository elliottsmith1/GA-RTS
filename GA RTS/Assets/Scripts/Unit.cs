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

    public enum WEAPONTYPE
    {
        BOW,
        CROSSBOW,
        ONE_HAND,
        POLEARM,
        SPEAR,
        STAFF,
        TWO_HAND
    }

    private Vector3 targetPosition;

    private NavMeshAgent navMeshAgent;
    private UnitManager unitManager;

    [SerializeField] WEAPONTYPE weapon;
    [SerializeField] bool mounted = false;

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

    public WEAPONTYPE GetWeapon()
    {
        return weapon;
    }

    public bool GetMounted()
    {
        return mounted;
    }
}

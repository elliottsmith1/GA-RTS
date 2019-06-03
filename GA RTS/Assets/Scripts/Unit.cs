using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public enum STATE
    {
        IDLE,
        MOVING,
        FIGHTING,
        DEAD
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

    private List<GameObject> nearbyEnemies = new List<GameObject>();

    private GameObject target;

    private NavMeshAgent navMeshAgent;
    private UnitManager unitManager;

    private string enemyTag = "Enemy";

    private bool manualOverride = false;

    private float health = 100.0f;
    [SerializeField] float maxHealth = 100.0f;
    [SerializeField] float weaponDamage = 40.0f;
    [SerializeField] float armour = 15.0f;

    [SerializeField] STATE state = STATE.IDLE;

    [SerializeField] WEAPONTYPE weapon;
    [SerializeField] bool mounted = false;
    [SerializeField] bool melee = true;
    [SerializeField] float range = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();        

        navMeshAgent = GetComponent<NavMeshAgent>();

        if (transform.tag == "Enemy")
        {
            enemyTag = "Friendly";
        }
        else if (transform.tag == "Friendly")
        {
            unitManager.NewUnit(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        nearbyEnemies.RemoveAll(item => item == null);

        if (state != STATE.DEAD)
        {
            State();
            NearestEnemy();
        }
    }

    private void State()
    {
        if (health < 1)
        {
            state = STATE.DEAD;
            Destroy(this.gameObject);
        }

        if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            if (manualOverride)
            {
                //manualOverride = false;
            }
        }

        if (navMeshAgent.velocity.magnitude > 0)
        {
            if (state != STATE.MOVING)
            {
                state = STATE.MOVING;
            }
        }
        else
        {
            if (nearbyEnemies.Count == 0)
            {
                if (state != STATE.IDLE)
                {
                    state = STATE.IDLE;
                }
            }
            else
            {
                if (state != STATE.FIGHTING)
                {
                    state = STATE.FIGHTING;
                }
            }
        }
    }

    public void DamageTarget()
    {
        target.GetComponent<Unit>().TakeDamage(weaponDamage);
    }

    private void NewTarget(GameObject _target)
    {
        if (!manualOverride)
        {
            target = _target;
        }
    }

    private void NearestEnemy()
    {
        if (nearbyEnemies.Count > 0)
        {
            if (!target)
            {
                NewTarget(nearbyEnemies[0]);
            }

            float dist = Vector3.Distance(transform.position, target.transform.position);

            foreach (GameObject enemy in nearbyEnemies)
            {
                if (enemy != target)
                {
                    float newDist = Vector3.Distance(transform.position, target.transform.position);

                    if (dist > newDist)
                    {
                        dist = newDist;
                        NewTarget(enemy);
                    }
                }
            }

            if (melee)
            {
                if (Vector3.Distance(transform.position, target.transform.position) > range)
                {
                    NewDestination(target.transform.position + target.transform.forward, false);
                }
                else
                {
                    NewDestination(transform.position, false);
                }
            }
        }
        else
        {
            if (target)
            {
                target = null;
            }
        }
    }

    public void TakeDamage(float _dam)
    {
        health -= _dam - armour;
    }

    public void NewDestination(Vector3 _pos, bool manualOrder)
    {
        if (!manualOverride)
        {
            navMeshAgent.SetDestination(_pos);
        }

        if (manualOrder)
        {
            manualOverride = true;
            target = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == enemyTag)
        {
            foreach (GameObject enemy in nearbyEnemies)
            {
                if (enemy == other.gameObject)
                {
                    return;
                }
            }

            nearbyEnemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == enemyTag)
        {
            nearbyEnemies.Remove(other.gameObject);
        }
    }

    public WEAPONTYPE GetWeapon()
    {
        return weapon;
    }

    public bool GetMounted()
    {
        return mounted;
    }

    public STATE GetState()
    {
        return state;
    }

    public GameObject GetTarget()
    {
        return target;
    }
}

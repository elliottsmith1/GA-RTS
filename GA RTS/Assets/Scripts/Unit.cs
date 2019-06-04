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

    private List<Unit> nearbyEnemies = new List<Unit>();

    public Unit target;

    private NavMeshAgent navMeshAgent;
    private UnitManager unitManager;
    private UnitAnimator unitAnimator;

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
        unitAnimator = GetComponent<UnitAnimator>();

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
            StateBehaviour();
            NearestEnemy();
        }
    }

    private void State()
    {
        if (health < 1)
        {
            state = STATE.DEAD;
            unitAnimator.SetDamaged(0);
            unitAnimator.SetFighting(false);
            unitAnimator.SetDead(true);
            GetComponent<CapsuleCollider>().enabled = false;
            navMeshAgent.enabled = false;
            this.enabled = false;
            return;
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
            if (nearbyEnemies.Count < 1)
            {
                if (state != STATE.IDLE)
                {
                    state = STATE.IDLE;
                }
            }
            else
            {
                if (target)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < range)
                    {
                        if (state != STATE.FIGHTING)
                        {
                            state = STATE.FIGHTING;
                            unitAnimator.SetFighting(true);
                        }
                    }
                }                
            }
        }
    }

    private void StateBehaviour()
    {
        switch (state)
        {
            case STATE.IDLE:

                break;
            case STATE.MOVING:

                break;
            case STATE.FIGHTING:
                if (nearbyEnemies.Count < 1)
                {
                    state = STATE.IDLE;
                    unitAnimator.SetFighting(false);
                    NewDestination(transform.position, false);
                    return;
                }

                if (target)
                {
                    transform.LookAt(target.transform);
                }
                break;
            case STATE.DEAD:

                break;
        }
    }

    public void DamageTarget()
    {
        if (target)
        {
            target.GetComponent<Unit>().TakeDamage(weaponDamage);
        }
    }

    private void NewTarget(Unit _target)
    {
        if (!manualOverride)
        {
            target = _target;
        }
    }

    private void NearestEnemy()
    {
        if (target)
        {
            if (!target.enabled)
            {
                target = null;
            }
        }        

        if (nearbyEnemies.Count > 0)
        {
            List<int> removeIDs = new List<int>();
            for (int i = 0; i < nearbyEnemies.Count; i++)
            {
                if (!nearbyEnemies[i].enabled)
                {
                    removeIDs.Add(i);
                }
            }

            foreach (int id in removeIDs)
            {
                if (nearbyEnemies[id])
                {
                    nearbyEnemies.RemoveAt(id);
                }
            }

            if (nearbyEnemies.Count < 1)
            {
                unitAnimator.SetFighting(false);
                return;
            }

            if (!target)
            {
                NewTarget(nearbyEnemies[0]);
            }

            float dist = Vector3.Distance(transform.position, target.transform.position);

            foreach (Unit enemy in nearbyEnemies)
            {
                if (enemy.enabled)
                {
                    if (enemy != target)
                    {
                        float newDist = Vector3.Distance(transform.position, enemy.transform.position);

                        if (dist > newDist)
                        {
                            dist = newDist;
                            NewTarget(enemy);
                        }
                    }
                }
            }

            //if (melee)
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
        unitAnimator.SetDamaged(1);
    }

    public void NewDestination(Vector3 _pos, bool manualOrder)
    {
        if (!manualOverride)
        {
            if (navMeshAgent.enabled)
            {
                navMeshAgent.SetDestination(_pos);
            }
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
            foreach (Unit enemy in nearbyEnemies)
            {
                if (enemy.gameObject == other.gameObject)
                {
                    return;
                }
            }

            nearbyEnemies.Add(other.gameObject.GetComponent<Unit>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == enemyTag)
        {
            nearbyEnemies.Remove(other.gameObject.GetComponent<Unit>());
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

    public Unit GetTarget()
    {
        return target;
    }
}

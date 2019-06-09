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
    private Outline outline;

    private string enemyTag = "Enemy";

    private bool manualOverride = false;

    private float health = 100.0f;
    [SerializeField] float maxHealth = 100.0f;
    [SerializeField] float weaponDamage = 40.0f;
    [SerializeField] float armour = 15.0f;

    [SerializeField] float spawnTime = 10.0f;
    [SerializeField] int populationValue = 1;

    [SerializeField] STATE state = STATE.IDLE;

    [SerializeField] WEAPONTYPE weapon;
    [SerializeField] bool mounted = false;
    [SerializeField] bool melee = true;
    [SerializeField] float range = 2.0f;

    [SerializeField] GameObject projectile;
    private Vector3 projectileStartPos;
    private float projectileFlightTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();        

        navMeshAgent = GetComponent<NavMeshAgent>();
        unitAnimator = GetComponent<UnitAnimator>();
        outline = GetComponent<Outline>();
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
            Die();
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
                    if (!melee)
                        projectile.gameObject.SetActive(false);
                    unitAnimator.SetFighting(false);
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

    private void Die()
    {
        if (!melee)
            projectile.gameObject.SetActive(false);
        state = STATE.DEAD;
        unitAnimator.SetDamaged(0);
        unitAnimator.SetFighting(false);
        unitAnimator.SetDead(true);        
        Destroy(outline);
        GetComponent<CapsuleCollider>().enabled = false;
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
        unitManager.RemoveUnit(this.gameObject);
        this.enabled = false;
        Destroy(this);
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

                    if (!melee)
                    {
                        //projectileFlightTime += Time.deltaTime / 1;
                        //projectile.transform.position = Vector3.Lerp(projectileStartPos, target.transform.position, projectileFlightTime);
                        projectile.transform.position = projectile.transform.position + ((target.transform.position - projectile.transform.position).normalized) * Time.deltaTime * 15.0f;
                    }
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
            if (target.enabled)
            {
                target.GetComponent<Unit>().TakeDamage(weaponDamage);

                if (!melee)
                {
                    FireProjectile();
                }
            }
        }
    }

    private void FireProjectile()
    {
        projectile.gameObject.SetActive(true);
        projectileFlightTime = 0.0f;
        projectileStartPos = transform.position;
        projectile.transform.position = transform.position;
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
                unitAnimator.SetFighting(false);
            }
        }        

        if (nearbyEnemies.Count > 0)
        {
            //List<int> removeIDs = new List<int>();
            //for (int i = 0; i < nearbyEnemies.Count; i++)
            //{
            //    if (!nearbyEnemies[i].enabled)
            //    {
            //        removeIDs.Add(i);
            //    }
            //}

            //if (removeIDs.Count > 0)
            //{
            //    foreach (int id in removeIDs)
            //    {
            //        if (nearbyEnemies.Count >= id)
            //        {
            //            nearbyEnemies.RemoveAt(id);
            //        }
            //    }
            //}

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
                unitAnimator.SetFighting(false);
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
        unitAnimator.SetFighting(false);
        unitAnimator.SetDamaged(0);

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

    public float GetSpawnTime()
    {
        return spawnTime;
    }

    public int GetPopulationValue()
    {
        return populationValue;
    }
}

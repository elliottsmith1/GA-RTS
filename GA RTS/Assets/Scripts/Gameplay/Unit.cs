﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private Vector3 finalDestination;

    private List<Unit> nearbyEnemies = new List<Unit>();
    private List<Building> nearbyEnemyBuildings = new List<Building>();

    private Unit target;
    private Building targetBuilding;

    private NavMeshAgent navMeshAgent;
    private UnitManager unitManager;
    private UnitAnimator unitAnimator;
    private Outline outline;

    private string enemyTag = "Friendly";
    private string enemyBuildingTag = "FriendlyBuilding";

    private bool manualOverride = false;
    private float manualTimer = 0.0f;
    private float manualTimerDelay = 1.0f;

    private float health = 100.0f;
    [SerializeField] GameObject healthBarPrefab;
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
    private float attackRange = 20.0f;

    [SerializeField] GameObject projectilePrefab;
    private GameObject activeProjectile;
    private List<Projectile> quiver = new List<Projectile>();

    private Vector3 projectileStartPos;
    private float projectileFlightTime = 0.0f;

    private GameObject healthUI;
    private Image healthBar;

    [SerializeField] Material redMaterial;
    [SerializeField] Material blueMaterial;

    private float stuckTimer = 0.0f;
    private float stuckTimerDelay = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();        

        navMeshAgent = GetComponent<NavMeshAgent>();
        unitAnimator = GetComponent<UnitAnimator>();
        outline = GetComponent<Outline>();

        if (gameObject.tag == "Friendly")
        {
            enemyTag = "Enemy";
            enemyBuildingTag = "EnemyBuilding";
        }

        Vector3 pos = transform.position;
        pos.y += (transform.localScale.y * 2);
        healthUI = Instantiate(healthBarPrefab, pos, Quaternion.identity);
        healthUI.transform.SetParent(transform);
        healthBar = healthUI.transform.Find("foreground").GetComponent<Image>();
        healthUI.SetActive(false);

        if (!melee)
        {
            if (projectilePrefab)
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject projec = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                    quiver.Add(projec.GetComponent<Projectile>());
                    projec.gameObject.SetActive(false);
                    projec.transform.SetParent(this.gameObject.transform);
                }
            }
        }

        NewDestination(new Vector3(transform.position.x + UnityEngine.Random.Range(-5, 5), transform.position.y, transform.position.z + UnityEngine.Random.Range(-5, 5)), false);

        if (enemyTag == "Enemy")
            PlayerSkillManager.instance.newUnit(populationValue);
    }

    // Update is called once per frame
    void Update()
    {
        nearbyEnemies.RemoveAll(item => item == null);
        nearbyEnemyBuildings.RemoveAll(item => item == null);

        if (state != STATE.DEAD)
        {
            State();
            StateBehaviour();

            if (!manualOverride)
            {
                NearestEnemy();
            }
        }
    }

    private void State()
    {
        if (health < 1)
        {
            Die();
            return;
        }

        if (manualOverride)
        {
            manualTimer += Time.deltaTime;

            if (manualTimer > manualTimerDelay)
            {
                if (!navMeshAgent.pathPending)
                {
                    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                    {
                        manualOverride = false;
                        manualTimer = 0.0f;
                    }
                }
            }
        }

        if (navMeshAgent.velocity.magnitude > 0)
        {
            if (state != STATE.MOVING)
            {
                state = STATE.MOVING;
            }
        }

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

        //else if (targetBuilding)
        //{
        //    if (Vector3.Distance(transform.position, hit.position) < range)
        //    {
        //        if (state != STATE.FIGHTING)
        //        {
        //            state = STATE.FIGHTING;
        //            unitAnimator.SetFighting(true);
        //        }
        //    }
        //}
        //else if (targetBuilding)
        //{
        //    RaycastHit hit1;
        //    Debug.DrawLine(transform.position, targetBuilding.transform.position);
        //    if (Physics.Raycast(transform.position, targetBuilding.transform.position - transform.position, out hit1, Mathf.Infinity))
        //    {
        //        if (hit1.collider.gameObject == targetBuilding.gameObject)
        //        {
        //            NavMeshHit hit;
        //            NavMesh.Raycast(transform.position, targetBuilding.transform.position, out hit, NavMesh.AllAreas);
        //            NavMesh.SamplePosition(hit.position, out hit, 20.0f, NavMesh.AllAreas);

        //            Debug.DrawRay(hit.position, Vector3.up, Color.red);

        //            if (Vector3.Distance(transform.position, hit.position) < range)
        //            {
        //                if (state != STATE.FIGHTING)
        //                {
        //                    state = STATE.FIGHTING;
        //                    unitAnimator.SetFighting(true);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            NewDestination(targetBuilding.transform.position, false);
        //        }
        //    }            
        //}
    }

    private void Die()
    {
        healthUI.SetActive(false);
        state = STATE.DEAD;
        unitAnimator.SetDamaged(0);
        unitAnimator.SetFighting(false);
        unitAnimator.SetDead(true);        
        Destroy(outline);
        GetComponent<CapsuleCollider>().enabled = false;
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        //if (gameObject.tag == "Friendly")
        //    unitManager.RemoveUnit(this.gameObject);

        if (enemyTag == "Enemy")
            PlayerSkillManager.instance.UnitDeath(populationValue);
        else
            PlayerSkillManager.instance.EnemyDeath(populationValue);

        this.enabled = false;
        Destroy(this);
    }

    private void StateBehaviour()
    {
        switch (state)
        {
            case STATE.IDLE:

                if (finalDestination != new Vector3(0, 0, 0))
                {
                    if (Vector3.Distance(transform.position, finalDestination) < 1)
                    {
                        finalDestination = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        NewDestination(finalDestination, false);
                    }
                }
                break;
            case STATE.MOVING:
                if (nearbyEnemies.Count < 1 && nearbyEnemyBuildings.Count < 1)
                {
                    if (navMeshAgent.remainingDistance < 2)
                    {
                        stuckTimer += Time.deltaTime;

                        if (stuckTimer > stuckTimerDelay)
                        {
                            stuckTimer = 0.0f;

                            //Debug.Log(navMeshAgent.velocity.magnitude);

                            //Vector3 pos = transform.position - transform.forward;
                            //pos.Normalize();

                            //Debug.Log(transform.position + " / " + pos);

                            NewDestination(transform.position, false);

                            state = STATE.IDLE;
                        }
                    }
                }
                break;
            case STATE.FIGHTING:
                if (nearbyEnemies.Count < 1 && nearbyEnemyBuildings.Count < 1)
                {
                    target = null;
                    targetBuilding = null;

                    state = STATE.IDLE;
                    unitAnimator.SetFighting(false);
                    //NewDestination(transform.position, false);
                    return;
                }

                if (target)
                {
                    transform.LookAt(target.transform);

                    if (!melee)
                    {
                        Vector3 targetPos = target.transform.position;
                        targetPos.y += 1.0f;

                        //projectile.transform.position = projectile.transform.position + ((targetPos - projectile.transform.position).normalized) * Time.deltaTime * 15.0f;

                        //if (Vector3.Distance(targetPos, projectile.transform.position) < 0.2f)
                        //{
                        //    projectile.SetActive(false);
                        //}
                    }
                }
                else if (targetBuilding)
                {
                    transform.LookAt(targetBuilding.transform);

                    //if (!melee)
                    //{
                    //    projectile.transform.position = projectile.transform.position + ((targetBuilding.transform.position - projectile.transform.position).normalized) * Time.deltaTime * 15.0f;
                    //    if (Vector3.Distance(targetBuilding.transform.position, projectile.transform.position) < 0.2f)
                    //    {
                    //        projectile.SetActive(false);
                    //    }
                    //}
                }
                break;
            case STATE.DEAD:

                break;
        }
    }

    public void AttackTarget()
    {
        if (target)
        {
            if (target.enabled)
            {
                if (Vector3.Distance(target.transform.position, transform.position) < range)
                {
                    target.GetComponent<Unit>().TakeDamage(weaponDamage);

                    if (!melee)
                    {
                        FireProjectile(target.gameObject);
                    }
                }
            }
        }
        else if (targetBuilding)
        {
            if (targetBuilding.enabled)
            {
                //if (Vector3.Distance(targetBuilding.transform.position, transform.position) < range)
                {
                    targetBuilding.GetComponent<Building>().TakeDamage(weaponDamage);

                    if (!melee)
                    {
                        FireProjectile(targetBuilding.gameObject);
                    }
                }
            }
        }
    }

    private void FireProjectile(GameObject _target)
    {
        if (quiver.Count > 0)
        {
            foreach (Projectile projec in quiver)
            {
                if (!projec.gameObject.activeInHierarchy)
                {
                    projec.Shoot(_target.transform);
                    activeProjectile = projec.gameObject;
                    activeProjectile.SetActive(true);

                    break;
                }
            }

            projectileFlightTime = 0.0f;

            Vector3 proSpawnPos = transform.position;
            proSpawnPos.y += 1.0f;
            projectileStartPos = proSpawnPos;
            activeProjectile.gameObject.transform.position = proSpawnPos;
        }
    }

    private void NewTarget(Unit _target)
    {
        if (!manualOverride)
        {
            target = _target;
        }
    }

    private void NewTargetBuilding(Building _target)
    {
        if (!manualOverride)
        {
            targetBuilding = _target;
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

            if (Vector3.Distance(transform.position, target.transform.position) > range)
            {
                NewDestination(target.transform.position + target.transform.forward, false);
            }
            else
            {
                NewDestination(transform.position, false);
            }
        }

        else if (nearbyEnemyBuildings.Count > 0)
        {
            if (nearbyEnemyBuildings.Count < 1)
            {
                unitAnimator.SetFighting(false);
                return;
            }

            if (!targetBuilding)
            {
                NewTargetBuilding(nearbyEnemyBuildings[0]);
            }

            float dist = Vector3.Distance(transform.position, targetBuilding.transform.position);

            foreach (Building enemy in nearbyEnemyBuildings)
            {
                if (enemy.enabled)
                {
                    if (enemy != targetBuilding)
                    {
                        float newDist = Vector3.Distance(transform.position, enemy.transform.position);

                        if (dist > newDist)
                        {
                            dist = newDist;
                            NewTargetBuilding(enemy);
                        }
                    }
                }
            }

            RaycastHit hit1;
            if (Physics.Raycast(transform.position, targetBuilding.transform.position - transform.position, out hit1, Mathf.Infinity))
            {
                if (hit1.collider.gameObject == targetBuilding.gameObject)
                {
                    if (melee)
                    {
                        NavMeshHit hit;
                        NavMesh.SamplePosition(hit1.point, out hit, 20.0f, NavMesh.AllAreas);

                        if (Vector3.Distance(transform.position, hit.position) > 1)
                        {
                            NewDestination(hit.position, false);
                        }
                        else
                        {
                            NewDestination(transform.position, false);

                            if (state != STATE.FIGHTING)
                            {
                                state = STATE.FIGHTING;
                                unitAnimator.SetFighting(true);
                            }
                        }
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, targetBuilding.transform.position) > range)
                        {
                            NewDestination(targetBuilding.transform.position, false);
                        }
                        else
                        {
                            NewDestination(transform.position, false);

                            if (state != STATE.FIGHTING)
                            {
                                state = STATE.FIGHTING;
                                unitAnimator.SetFighting(true);
                            }
                        }
                    }
                }
                else
                {
                    NewDestination(targetBuilding.transform.position, false);
                }
            }           
        }

        else 
        {
            if (gameObject.tag == "Enemy")
            {
                if (nearbyEnemies.Count < 1 && nearbyEnemyBuildings.Count < 1)
                {
                    SetLayer(transform, LayerMask.NameToLayer("Enemy"));
                }
            }

            if (target)
            {
                target = null;
                unitAnimator.SetFighting(false);
            }

            if (targetBuilding)
            {
                targetBuilding = null;
                unitAnimator.SetFighting(false);
            }
        }
    }

    public void TakeDamage(float _dam)
    {
        health -= _dam - armour;
        unitAnimator.SetDamaged(1);

        healthUI.SetActive(true);
        healthBar.fillAmount = health / maxHealth;
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
            targetBuilding = null;
        }
    }

    public void NewFinalDestination(Vector3 _pos)
    {
        finalDestination = _pos;
    }

    public void SetColour(string _col)
    {
        Material mat = blueMaterial;

        switch(_col)
        {
            case "red":
                mat = redMaterial;
                break;
        }

        foreach(Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                if (child.GetComponent<SkinnedMeshRenderer>())
                {
                    child.GetComponent<SkinnedMeshRenderer>().material = mat;
                }
                if (child.GetComponent<MeshRenderer>())
                {
                    child.GetComponent<MeshRenderer>().material = mat;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == enemyTag)
        {
            foreach (Unit enemy in nearbyEnemies)
            {
                if (enemy)
                {
                    if (enemy.gameObject == other.gameObject)
                    {
                        return;
                    }
                }
            }

            nearbyEnemies.Add(other.gameObject.GetComponent<Unit>());

            if (gameObject.tag == "Enemy")
            {
                if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    SetLayer(transform, LayerMask.NameToLayer("Default"));
                }
            }
        }

        if (other.transform.tag == enemyBuildingTag)
        {
            foreach (Building enemy in nearbyEnemyBuildings)
            {
                if (enemy)
                {
                    if (enemy.gameObject == other.gameObject)
                    {
                        return;
                    }
                }
            }

            if (other.transform.tag == "EnemyBuilding")
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    other.gameObject.layer = LayerMask.NameToLayer("Default");
                }
            }

            nearbyEnemyBuildings.Add(other.gameObject.GetComponent<Building>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == enemyTag)
        {
            nearbyEnemies.Remove(other.gameObject.GetComponent<Unit>());            
        }

        if (other.transform.tag == enemyBuildingTag)
        {
            nearbyEnemyBuildings.Remove(other.gameObject.GetComponent<Building>());
        }        
    }

    public void SetLayer(Transform root, int layer)
    {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            SetLayer(child, layer);
    }

    public WEAPONTYPE GetWeapon()
    {
        return weapon;
    }

    public bool GetMounted()
    {
        return mounted;
    }

    public bool GetMelee()
    {
        return melee;
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

    public void SetPopulationValue(int _val)
    {
        populationValue = _val;
    }

    public int GetPopulationValue()
    {
        return populationValue;
    }

    public void IncreaseDamage(float _dam)
    {
        weaponDamage += _dam;
    }

    public void IncreaseArmour(float _arm)
    {
        armour += _arm;
    }
}

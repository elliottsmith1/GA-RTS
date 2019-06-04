using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAnimator : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    private Unit unit;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        unit = GetComponent<Unit>();

        SetWeapon(unit.GetWeapon(), unit.GetMounted());
    }

    private void SetWeapon(Unit.WEAPONTYPE _wep, bool _mount)
    {
        if (_mount)
        {
            anim.SetBool("mounted", true);
            switch (_wep)
            {
                case Unit.WEAPONTYPE.ONE_HAND:
                    anim.SetBool("one hand", true);
                    break;
                case Unit.WEAPONTYPE.BOW:
                    anim.SetBool("bow", true);
                    break;
                case Unit.WEAPONTYPE.CROSSBOW:
                    anim.SetBool("crossbow", true);
                    break;
                case Unit.WEAPONTYPE.SPEAR:
                    anim.SetBool("spear", true);
                    break;
                case Unit.WEAPONTYPE.STAFF:
                    anim.SetBool("staff", true);
                    break;
            }

        }
        else
        {
            switch (_wep)
            {
                case Unit.WEAPONTYPE.ONE_HAND:
                    anim.SetBool("one hand", true);
                    break;
                case Unit.WEAPONTYPE.BOW:
                    anim.SetBool("bow", true);
                    break;
                case Unit.WEAPONTYPE.CROSSBOW:
                    anim.SetBool("crossbow", true);
                    break;
                case Unit.WEAPONTYPE.POLEARM:
                    anim.SetBool("polearm", true);
                    break;
                case Unit.WEAPONTYPE.SPEAR:
                    anim.SetBool("spear", true);
                    break;
                case Unit.WEAPONTYPE.STAFF:
                    anim.SetBool("staff", true);
                    break;
                case Unit.WEAPONTYPE.TWO_HAND:
                    anim.SetBool("two hand", true);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent)
        {
            anim.SetFloat("speed", agent.velocity.magnitude);
        }

        if (unit)
        {
            if (unit.GetState() == Unit.STATE.FIGHTING)
            {
                anim.SetBool("fighting", true);
            }
        }
    }

    public void SetFighting(bool _fight)
    {
        if (_fight)
        {
            anim.SetBool("fighting", true);
        }
        else
        {
            anim.SetBool("fighting", false);
        }
    }

    public void SetDamaged(int _dam)
    {
        if (_dam > 0)
        {
            anim.SetBool("damaged", true);
        }
        else
        {
            anim.SetBool("damaged", false);
        }
    }

    public void SetDead(bool _dead)
    {
        if (_dead)
        {
            anim.SetBool("dead", true);
        }
        else
        {
            anim.SetBool("dead", false);
        }
    }
}

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

    private void SetWeapon(Unit.WEAPON _wep, bool _mount)
    {
        if (_mount)
        {
            anim.SetBool("mounted", true);
            switch (_wep)
            {
                case Unit.WEAPON.ONE_HAND:
                    anim.SetBool("one hand", true);
                    break;
                case Unit.WEAPON.BOW:
                    anim.SetBool("bow", true);
                    break;
                case Unit.WEAPON.CROSSBOW:
                    anim.SetBool("crossbow", true);
                    break;
                case Unit.WEAPON.SPEAR:
                    anim.SetBool("spear", true);
                    break;
                case Unit.WEAPON.STAFF:
                    anim.SetBool("staff", true);
                    break;
            }

        }
        else
        {
            switch (_wep)
            {
                case Unit.WEAPON.ONE_HAND:
                    anim.SetBool("one hand", true);
                    break;
                case Unit.WEAPON.BOW:
                    anim.SetBool("bow", true);
                    break;
                case Unit.WEAPON.CROSSBOW:
                    anim.SetBool("crossbow", true);
                    break;
                case Unit.WEAPON.POLEARM:
                    anim.SetBool("polearm", true);
                    break;
                case Unit.WEAPON.SPEAR:
                    anim.SetBool("spear", true);
                    break;
                case Unit.WEAPON.STAFF:
                    anim.SetBool("staff", true);
                    break;
                case Unit.WEAPON.TWO_HAND:
                    anim.SetBool("two hand", true);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", agent.velocity.magnitude);
    }
}

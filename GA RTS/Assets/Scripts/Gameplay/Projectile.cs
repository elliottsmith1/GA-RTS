using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float flightSpeed = 20.0f;

    private Vector3 target;
    private bool fired = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fired)
        {
            transform.position = transform.position + ((target - transform.position).normalized) * Time.deltaTime * flightSpeed;

            if (Vector3.Distance(target, transform.position) < 0.1f)
            {
                this.gameObject.SetActive(false);
                fired = false;
                transform.position = transform.parent.position;
            }
        }
    }

    public void Shoot(Transform _tar)
    {
        Vector3 pos = _tar.position;
        pos.y += 1.0f;
        target = pos;
        fired = true;
    }
}

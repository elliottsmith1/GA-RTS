using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;

    private Vector3 targetRotation; 
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        targetRotation = cam.position - transform.position;

        var rot = Quaternion.LookRotation(targetRotation);
        rot.z = 90;
        rot.y = 90;

        transform.rotation = rot;
    }
}

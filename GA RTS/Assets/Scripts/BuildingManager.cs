using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] GameObject selectedBuilding;
    [SerializeField] GameObject farm;

    [SerializeField] float distanceAllowance = 5.0f;

    public bool holdingObject = false;
    private bool canPlace = false;

    private Camera cam;
    private Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        outline = selectedBuilding.GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        if (holdingObject)
        {
            Vector3 point = new Vector3(0, 0, 0);

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                point = hit.point;
                selectedBuilding.transform.position = point;
                
                NavMeshHit nHit;

                if (NavMesh.FindClosestEdge(point, out nHit, NavMesh.AllAreas))
                {
                    if (nHit.distance > distanceAllowance)
                    {
                        if (hit.transform.tag == "Terrain")
                        {
                            canPlace = true;
                            outline.OutlineColor = Color.green;
                        }
                    }
                    else
                    {
                        canPlace = false;
                        outline.OutlineColor = Color.red;
                    }
                }
                
            }                                  
        }
    }
}



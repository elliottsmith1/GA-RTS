using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] GameObject selectedBuilding;   

    [SerializeField] float distanceAllowance = 5.0f;

    public bool holdingObject = false;
    private bool canPlace = false;

    private Camera cam;
    private Outline outline;

    private Building activeBuilding;

    public enum BUILDINGS
    {
        BARRACKS,
        ARCHERY
    }

    [Header("Building prefabs")]
    [SerializeField] GameObject barracks;
    [SerializeField] GameObject archeryRange;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;        
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
                //point.y = 0.0f;
                //selectedBuilding.transform.position = point;
                
                NavMeshHit nHit;

                if (NavMesh.FindClosestEdge(point, out nHit, NavMesh.AllAreas))
                {
                    if (hit.transform.tag == "Terrain")
                    {
                        selectedBuilding.transform.position = point;

                        if (nHit.distance > distanceAllowance)
                        {
                            canPlace = true;
                            outline.OutlineColor = Color.green;
                        }
                        else
                        {
                            canPlace = false;
                            outline.OutlineColor = Color.red;
                        }
                    }
                    else
                    {
                        canPlace = false;
                        outline.OutlineColor = Color.red;
                    }
                }                
            }          
            
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(selectedBuilding);
                holdingObject = false;
                canPlace = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (canPlace)
                {
                    Building build = selectedBuilding.GetComponent<Building>();
                    build.enabled = true;
                    outline.OutlineColor = Color.white;
                    build.ActivateObject();

                    selectedBuilding = null;
                    holdingObject = false;
                    canPlace = false;
                }
            }
        }
    }

    public void SelectBuilding(string _building)
    {
        holdingObject = true;
        Quaternion rot = new Quaternion(-45, 0, 0, 0);
        switch (_building)
        {
            case "BARRACKS":
                selectedBuilding = Instantiate(barracks, transform);
                break;
            case "ARCHERY":
                selectedBuilding = Instantiate(archeryRange, transform);
                break;
        }
        outline = selectedBuilding.GetComponent<Outline>();
        outline.enabled = true;
    }

    public void SetActiveBuilding(Building _b)
    {
        activeBuilding = _b;
    }

    public Building GetActiveBuilding()
    {
        return activeBuilding;
    }
}



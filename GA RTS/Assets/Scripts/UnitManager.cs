using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public List<GameObject> allUnits = new List<GameObject>();
    public List<Unit> selectedUnits = new List<Unit>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        allUnits.RemoveAll(item => item == null);
        selectedUnits.RemoveAll(item => item == null);

        DetectInput();
    }

    private void DetectInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedUnits.Count > 0)
            {
                int rowNum = (int)Mathf.Sqrt(selectedUnits.Count);
                int rowCounter = 0;

                Vector3 point = new Vector3(0, 0, 0);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    point = hit.point;
                }

                if (hit.transform.tag == "Terrain")
                {
                    float offset = 2.0f;

                    point.z -= (offset * (rowNum / 2));
                    point.x -= (offset * (rowNum / 2));

                    Vector3 newPos = point;
                                      

                    foreach (Unit unit in selectedUnits)
                    {
                        unit.NewDestination(newPos);

                        newPos.x += offset;

                        rowCounter++;

                        if (rowCounter >= rowNum)
                        {
                            rowCounter = 0;
                            newPos.x = point.x;
                            newPos.z += offset;
                        }
                    }
                }
            }
        }
    }

    public void NewUnit(GameObject _unit)
    {
        allUnits.Add(_unit);
    }

    public void SelectUnit(Unit _unit)
    {
        selectedUnits.Add(_unit);
    }

    public List<GameObject> GetAllUnits()
    {
        return allUnits;
    }

    public void DeselectSelection()
    {
        foreach(Unit unit in selectedUnits)
        {
            unit.gameObject.GetComponent<Outline>().enabled = false;
        }

        selectedUnits.Clear();
    }
}

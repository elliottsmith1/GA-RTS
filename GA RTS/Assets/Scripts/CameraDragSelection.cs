using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDragSelection : MonoBehaviour
{
    [SerializeField] UnitManager unitManager;
    //Add all units in the scene to this array
    private List<GameObject> allUnits = new List<GameObject>();
    //The selection square we draw when we drag the mouse to select units
    [SerializeField] RectTransform selectionSquareTrans;
    //To test the square's corners
    private Transform sphere1;
    private Transform sphere2;
    private Transform sphere3;
    private Transform sphere4;
    //The materials
    [SerializeField] Material normalMaterial;
    [SerializeField] Material highlightMaterial;
    [SerializeField] Material selectedMaterial;

    [SerializeField] Camera camMain;

    //We have hovered above this unit, so we can deselect it next update
    //and dont have to loop through all units
    private GameObject highlightThisUnit;

    //To determine if we are clicking with left mouse or holding down left mouse
    private float delay = 0.3f;
    private float clickTime = 0f;
    //The start and end coordinates of the square we are making
    private Vector3 squareStartPos;
    private Vector3 squareEndPos;
    //If it was possible to create a square
    private bool hasCreatedSquare;
    //The selection squares 4 corner positions
    private Vector3 TL, TR, BL, BR;

    void Start()
    {
        camMain = GetComponent<Camera>();
        //Deactivate the square selection image
        selectionSquareTrans.gameObject.SetActive(false);
    }

    void Update()
    {
        //Select one or several units by clicking or draging the mouse
        SelectUnits();
    }

    //Select units with click or by draging the mouse
    void SelectUnits()
    {
        //Are we clicking with left mouse or holding down left mouse
        bool isClicking = false;
        bool isHoldingDown = false;

        //Click the mouse button
        if (Input.GetMouseButtonDown(0))
        {
            squareStartPos = Input.mousePosition;
            clickTime = Time.time;
            unitManager.DeselectSelection();
        }
        //Release the mouse button
        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - clickTime <= delay)
            {
                isClicking = true;
            }

            //Select all units within the square if we have created a square
            if (hasCreatedSquare)
            {
                hasCreatedSquare = false;

                //Deactivate the square selection image
                selectionSquareTrans.gameObject.SetActive(false);

                allUnits = unitManager.GetAllUnits();

                //Select the units
                for (int i = 0; i < allUnits.Count; i++)
                {
                    GameObject currentUnit = allUnits[i];

                    //Is this unit within the square
                    if (IsWithinPolygon(currentUnit.transform.position))
                    {
                        currentUnit.GetComponent<Outline>().enabled = true;

                        unitManager.SelectUnit(currentUnit.GetComponent<Unit>());
                    }
                    //Otherwise deselect the unit if it's not in the square
                    else
                    {
                        currentUnit.GetComponent<Outline>().enabled = false;
                    }
                }
            }

        }
        //Holding down the mouse button
        if (Input.GetMouseButton(0))
        {
            if (Time.time - clickTime > delay)
            {
                isHoldingDown = true;
            }
        }

        if (isClicking)
        {
            unitManager.DeselectSelection();            
        }

        //Drag the mouse to select all units within the square
        if (isHoldingDown)
        {
            //Activate the square selection image
            if (!selectionSquareTrans.gameObject.activeInHierarchy)
            {
                selectionSquareTrans.gameObject.SetActive(true);
            }

            //Get the latest coordinate of the square
            squareEndPos = Input.mousePosition;

            //Display the selection with a GUI image
            DisplaySquare();

            //Highlight the units within the selection square, but don't select the units
            if (hasCreatedSquare)
            {
                allUnits = unitManager.GetAllUnits();

                for (int i = 0; i < allUnits.Count; i++)
                {
                    GameObject currentUnit = allUnits[i];

                    //Is this unit within the square
                    if (IsWithinPolygon(currentUnit.transform.position))
                    {
                        currentUnit.GetComponent<Outline>().enabled = true;
                    }
                    //Otherwise deactivate
                    else
                    {
                        currentUnit.GetComponent<Outline>().enabled = false;
                    }
                }
            }
        }
    }

   
    //Is a unit within a polygon determined by 4 corners
    bool IsWithinPolygon(Vector3 unitPos)
    {
        bool isWithinPolygon = false;

        //The polygon forms 2 triangles, so we need to check if a point is within any of the triangles
        //Triangle 1: TL - BL - TR
        if (IsWithinTriangle(unitPos, TL, BL, TR))
        {
            return true;
        }

        //Triangle 2: TR - BL - BR
        if (IsWithinTriangle(unitPos, TR, BL, BR))
        {
            return true;
        }

        return isWithinPolygon;
    }

    //Is a point within a triangle
    //From http://totologic.blogspot.se/2014/01/accurate-point-in-triangle-test.html
    bool IsWithinTriangle(Vector3 p, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        bool isWithinTriangle = false;

        //Need to set z -> y because of other coordinate system
        float denominator = ((p2.z - p3.z) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.z - p3.z));

        float a = ((p2.z - p3.z) * (p.x - p3.x) + (p3.x - p2.x) * (p.z - p3.z)) / denominator;
        float b = ((p3.z - p1.z) * (p.x - p3.x) + (p1.x - p3.x) * (p.z - p3.z)) / denominator;
        float c = 1 - a - b;

        //The point is within the triangle if 0 <= a <= 1 and 0 <= b <= 1 and 0 <= c <= 1
        if (a >= 0f && a <= 1f && b >= 0f && b <= 1f && c >= 0f && c <= 1f)
        {
            isWithinTriangle = true;
        }

        return isWithinTriangle;
    }

    //Display the selection with a GUI square
    void DisplaySquare()
    {
        //The start position of the square is in 3d space, or the first coordinate will move
        //as we move the camera which is not what we want
        //Vector3 squareStartScreen = camMain.WorldToScreenPoint(squareStartPos);
        Vector3 squareStartScreen = squareStartPos;

        squareStartScreen.z = 0f;

        //Get the middle position of the square
        Vector3 middle = (squareStartScreen + squareEndPos) / 2f;

        //Set the middle position of the GUI square
        selectionSquareTrans.position = middle;

        //Change the size of the square
        float sizeX = Mathf.Abs(squareStartScreen.x - squareEndPos.x);
        float sizeY = Mathf.Abs(squareStartScreen.y - squareEndPos.y);

        //Set the size of the square
        selectionSquareTrans.sizeDelta = new Vector2(sizeX, sizeY);

        //The problem is that the corners in the 2d square is not the same as in 3d space
        //To get corners, we have to fire a ray from the screen
        //We have 2 of the corner positions, but we don't know which,  
        //so we can figure it out or fire 4 raycasts
        TL = new Vector3(middle.x - sizeX / 2f, middle.y + sizeY / 2f, 0f);
        TR = new Vector3(middle.x + sizeX / 2f, middle.y + sizeY / 2f, 0f);
        BL = new Vector3(middle.x - sizeX / 2f, middle.y - sizeY / 2f, 0f);
        BR = new Vector3(middle.x + sizeX / 2f, middle.y - sizeY / 2f, 0f);

        //From screen to world
        RaycastHit hit;
        int i = 0;
        //Fire ray from camera
        if (Physics.Raycast(camMain.ScreenPointToRay(TL), out hit, 200f))
        {
            TL = hit.point;
            i++;
        }
        if (Physics.Raycast(camMain.ScreenPointToRay(TR), out hit, 200f))
        {
            TR = hit.point;
            i++;
        }
        if (Physics.Raycast(camMain.ScreenPointToRay(BL), out hit, 200f))
        {
            BL = hit.point;
            i++;
        }
        if (Physics.Raycast(camMain.ScreenPointToRay(BR), out hit, 200f))
        {
            BR = hit.point;
            i++;
        }

        //Could we create a square?
        hasCreatedSquare = false;

        //We could find 4 points
        if (i == 4)
        {
            //Display the corners for debug
            //sphere1.position = TL;
            //sphere2.position = TR;
            //sphere3.position = BL;
            //sphere4.position = BR;

            hasCreatedSquare = true;
        }
    }
}

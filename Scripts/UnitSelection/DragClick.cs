using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragClick : MonoBehaviour
{
    Camera myCam;

    [SerializeField] RectTransform boxVisual;

    Rect selectionBox;

    Vector2 startPos, endPos;

    void Start()
    {
        myCam = Camera.main;
        startPos = Vector2.zero;
        endPos = Vector2.zero;
        DrawVisual();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            startPos = Input.mousePosition;
            selectionBox = new Rect();
        }
        if(Input.GetMouseButton(1))
        {
            endPos = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }
        if(Input.GetMouseButtonUp(1))
        {
            SelectUnits();
            startPos = Vector2.zero;
            endPos = Vector2.zero;
            DrawVisual();
        }
    }

    void DrawVisual()
    {
        Vector2 boxStart = startPos;
        Vector2 boxEnd = endPos;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;

        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        if(Input.mousePosition.x < startPos.x)
        {
            // dragging Left
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPos.x;
        }
        else
        {
            // dragging Right
            selectionBox.xMin = startPos.x;
            selectionBox.xMax = Input.mousePosition.x;
        }
        if (Input.mousePosition.y < startPos.y)
        {
            // dragging Down
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPos.y;
        }
        else
        {
            // dragging Up
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPos.y;
        }
    }

    void SelectUnits()
    {
        //Loop Through All Clickale Units
        foreach(var unit in UnitSelections.Instance.unitsList)
        {
            // if unit is within bounds of Rect
            if(selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position)))
            {
                // if Any unit is within the selection add them to selection
                UnitSelections.Instance.DragSelect(unit);
            }
        }
    }
}

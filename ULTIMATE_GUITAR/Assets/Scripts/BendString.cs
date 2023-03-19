using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BendString : MonoBehaviour
{
    private Vector3 initialMousePosition;
    private bool isDraggingVertical = false;
    private float verticalDisplacement;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsMouseOverCylinder())
        {
            initialMousePosition = Input.mousePosition;
            isDraggingVertical = true;
        }
        if (Input.GetMouseButton(0) && isDraggingVertical)
        {
            verticalDisplacement = Input.mousePosition.y - initialMousePosition.y;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Vertical displacement: " + verticalDisplacement);
            isDraggingVertical = false;
        }
    }
    private bool IsMouseOverCylinder()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            return hitInfo.collider.gameObject == gameObject;
        }
        return false;
    }
}
//Script only for reference, and not used by any gameobject
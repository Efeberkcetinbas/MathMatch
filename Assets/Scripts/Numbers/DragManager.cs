using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    [SerializeField] private LayerMask numberLayer; // Layer for detecting numbers
    [SerializeField] private float raycastDistance = 5f; // Distance to check for obstacles

    private void Update()
    {
        if (Input.touchCount > 0) // Check if there are active touches
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) // Detect when touch starts
            {
                CheckIfCanMove(touch);
            }
        }
    }

    void CheckIfCanMove(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        // Check if we touched a number object
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, numberLayer))
        {
            NumberDrag numberDrag = hit.collider.GetComponent<NumberDrag>();
            if (numberDrag != null)
            {
                // Raycast forward to check if another number is blocking
                Vector3 rayDirection = numberDrag.transform.forward;
                if (Physics.Raycast(numberDrag.transform.position, rayDirection, raycastDistance, numberLayer))
                {
                    Debug.Log($"Can't move {numberDrag.gameObject.name} because another number is in the way.");
                }
                else
                {
                    Debug.Log($"{numberDrag.gameObject.name} can move forward.");
                }
            }
        }
    }
}

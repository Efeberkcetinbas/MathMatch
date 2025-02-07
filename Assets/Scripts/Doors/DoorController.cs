using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorController : MonoBehaviour
{
    public int doorValue;  // The value we set for this door
    public float raycastDistance = 10f; // Distance for the raycast
    public LayerMask numberLayer; // Layer mask to detect number objects
    public bool matchNumbersValue=false;

    [SerializeField] private TextMeshProUGUI currentSum;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnMoveNumberToGround,OnMoveNumberToGround);
    }


    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnMoveNumberToGround,OnMoveNumberToGround);
    }


    /*private void Update()
    {
        CheckNumberSum();
    }*/

    private void OnMoveNumberToGround()
    {
        CheckNumberSum();
    }

    // Method to check if the sum of numbers matches the door's value
    void CheckNumberSum()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.back), raycastDistance, numberLayer);


        int sum = 0;

        foreach (RaycastHit hit in hits)
        {
            // Get the NumberProp component from the hit object
            NumberProp numberProp = hit.collider.GetComponent<NumberProp>();
            if (numberProp != null)
            {
                sum += numberProp.numberValue; // Add the number's value to the sum
            }

        }

        currentSum.SetText(sum.ToString());

        // Check if the sum matches the door's value
        if (sum == doorValue)
        {
            Debug.Log("The sum of numbers matches the door's value!");
            matchNumbersValue=true;
        }
        else
        {
            Debug.Log("The sum of numbers does NOT match the door's value.");
            matchNumbersValue=false;
        }

    }

    // Draw the ray in the scene for debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * raycastDistance);
    }
}

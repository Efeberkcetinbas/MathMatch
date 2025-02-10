using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorController : MonoBehaviour
{
    public int doorValue;  // The value we set for this door
    public float raycastDistance = 10f; // Distance for the raycast
    public LayerMask numberLayer; // Layer mask to detect number objects
    public LayerMask groundLayer; // Layer mask to detect number objects
    public bool matchNumbersValue=false;
    internal Color color;

    [SerializeField] private TextMeshProUGUI currentSum;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnMoveNumberToGround,OnMoveNumberToGround);
        EventManager.AddHandler(GameEvent.OnGenerateNumbers,OnGenerateNumbers);
    }


    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnMoveNumberToGround,OnMoveNumberToGround);
        EventManager.RemoveHandler(GameEvent.OnGenerateNumbers,OnGenerateNumbers);
    }

    private void Start()
    {
        StartCoroutine(SetMatColors());
    }

    

    private void OnMoveNumberToGround()
    {
        CheckNumberSum();
    }

    private void OnGenerateNumbers()
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

    private IEnumerator SetMatColors()
    {
        yield return new WaitForSeconds(0.25f);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.back), raycastDistance, groundLayer);

        foreach (RaycastHit hit in hits)
        {
            // Get the NumberProp component from the hit object
            Ground ground = hit.collider.GetComponent<Ground>();
            if (ground != null)
            {
                ground.groundRenderer.material.color=color;
            }

        }

    }

    // Draw the ray in the scene for debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * raycastDistance);
    }
}

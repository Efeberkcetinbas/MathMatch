using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGenerator : MonoBehaviour
{
    public LevelDoors levelDoors;
    public List<Transform> groundPositions;

    private void Start()
    {
        GenerateDoors();
    }

    void GenerateDoors()
    {
        if (levelDoors == null || groundPositions.Count == 0) return;

        List<Transform> availablePositions = new List<Transform>(groundPositions);

        for (int i = 0; i < levelDoors.doors.Count && availablePositions.Count > 0; i++)
        {
            LevelDoors.DoorData data = levelDoors.doors[i];

            // Get a random available position
            int randomIndex = Random.Range(0, availablePositions.Count);
            Transform chosenPosition = availablePositions[randomIndex];
            availablePositions.RemoveAt(randomIndex); // Prevent reuse

            GameObject door = Instantiate(data.doorPrefab, chosenPosition.position, chosenPosition.rotation);
            door.GetComponent<DoorProp>().SetDoorChildRotateZero();
            // Set door color
            door.GetComponent<DoorProp>().SetMatColor(data.color);
            door.GetComponent<DoorController>().color=data.color;

            // Set door value using DoorProp
            DoorProp doorProp = door.GetComponent<DoorProp>();
            if (doorProp != null)
            {
                doorProp.SetDoorValue(data.value);
            }

            DoorController doorController=door.GetComponent<DoorController>();
            if(doorController!=null)
            {
                doorController.doorValue=data.value;
            }
        }

       
    }
}

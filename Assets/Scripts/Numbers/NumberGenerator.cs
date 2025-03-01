using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberGenerator : MonoBehaviour
{
    public Number numberConfig;
    public List<Transform> groundPositions;
    public List<Color> colorList;
    public GameObject hexParentPrefab;

    private void Start()
    {
        GenerateNumbers();
    }

    void GenerateNumbers()
    {
        if (numberConfig == null || numberConfig.numbers.Count == 0 || groundPositions.Count == 0) return;

        List<Transform> availablePositions = new List<Transform>(groundPositions);

        for (int i = 0; i < numberConfig.numbers.Count && availablePositions.Count > 0; i++)
        {
            Number.NumberData data = numberConfig.numbers[i];

            // Get a random available position
            int randomIndex = Random.Range(0, availablePositions.Count);
            Transform chosenPosition = availablePositions[randomIndex];
            availablePositions.RemoveAt(randomIndex);
            chosenPosition.GetComponent<Ground>().IsOccupied = true;

            

            // Instantiate HexParent & HexChildren
            GameObject hexParentObj = Instantiate(hexParentPrefab, chosenPosition.position , Quaternion.identity);
            HexParent hexParent = hexParentObj.GetComponent<HexParent>();
            NumberProp numberProp=hexParent.GetComponent<NumberProp>();
            
            if (hexParent != null)
            {
                hexParent.towerValue = data.value;
                hexParent.SetInit();
                //StartCoroutine(hexParent.SpawnHexChildrenWithEffect());
            }

            if(numberProp!=null)
            {
                numberProp.numberValue=data.value;
            }
        }

        EventManager.Broadcast(GameEvent.OnGenerateNumbers);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    public int targetValue;
    public List<Person> peopleInOrder = new List<Person>();
    public TextMeshProUGUI doorText;

    public void Initialize()
    {
        doorText.text = targetValue.ToString();
    }

    public bool IsCorrect()
    {
        int currentValue = 0;

        foreach (var person in peopleInOrder)
        {
            currentValue = person.ApplyOperation(currentValue);
        }

        return currentValue == targetValue;
    }

    public void UpdateValueDisplay()
    {
        doorText.text = $"Target: {targetValue}";
    }
}

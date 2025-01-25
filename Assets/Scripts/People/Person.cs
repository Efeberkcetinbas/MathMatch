using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Person : MonoBehaviour
{
    public string operation;
    public TextMeshProUGUI personText;

    public void Initialize()
    {
        personText.text = operation;
    }

    public int ApplyOperation(int currentValue)
    {
        char op = operation[0];
        int value = int.Parse(operation.Substring(1));

        switch (op)
        {
            case '+': return currentValue + value;
            case '-': return currentValue - value;
            case '*': return currentValue * value;
            case '/': return value == 0 ? currentValue : currentValue / value; // Avoid division by zero
            default: return currentValue;
        }
    }
}

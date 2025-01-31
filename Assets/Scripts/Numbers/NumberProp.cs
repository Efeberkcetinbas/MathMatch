using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberProp : MonoBehaviour
{
    public TextMeshProUGUI numberText;

    public void SetNumberValue(int value)
    {
        if (numberText != null)
        {
            numberText.text = value.ToString();
        }
    }
}

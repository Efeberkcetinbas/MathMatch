using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorProp : MonoBehaviour
{
    public TextMeshProUGUI valueText;

    public void SetDoorValue(int value)
    {
        if (valueText != null)
        {
            valueText.text = value.ToString();
        }
    }
}

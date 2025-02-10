using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorProp : MonoBehaviour
{
    public TextMeshProUGUI valueText;

    [SerializeField] private Transform childObject;
    [SerializeField] private Renderer rend;

    public void SetDoorValue(int value)
    {
        if (valueText != null)
        {
            valueText.text = value.ToString();
        }
    }


    internal void SetDoorChildRotateZero()
    {
        childObject.rotation= Quaternion.identity;
    }

    internal void SetMatColor(Color color)
    {
        rend.material.color=color;
    }
}

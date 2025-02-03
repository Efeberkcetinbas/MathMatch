using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexChild : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    [SerializeField] private Renderer rendererobj;



    public void SetHexChild(int towerValue)
    {
        if (textUI != null)
        {
            textUI.text = towerValue.ToString();
        }
    }

    public void SetHexColor(Color parentColor)
    {
        if (rendererobj != null)
        {
            rendererobj.material.color = parentColor;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexChild : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    [SerializeField] private Renderer rendererobj;

    [SerializeField] private BoxCollider boxCollider;



    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnHoldingNumber, OnHoldingNumber);
        EventManager.AddHandler(GameEvent.OnReleaseNumber, OnReleaseNumber);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnHoldingNumber, OnHoldingNumber);
        EventManager.RemoveHandler(GameEvent.OnReleaseNumber, OnReleaseNumber);
    }


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


    private void OnReleaseNumber()
    {
        ChangeOpacity(1);
        boxCollider.enabled=true;
    }

    private void OnHoldingNumber()
    {
        ChangeOpacity(0.25f);
        boxCollider.enabled=false;
    }

    private void ChangeOpacity(float alpha)
    {
        if (rendererobj == null)
            rendererobj = GetComponent<Renderer>();

        if (rendererobj != null)
        {
            Color color = rendererobj.material.color;
            color.a = Mathf.Clamp01(alpha); // Ensure alpha is between 0 and 1
            rendererobj.material.color = color;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool IsOccupied = false;
    public Renderer groundRenderer;
    private Color defaultColor;

    private void Start()
    {
        defaultColor = groundRenderer.material.color;
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCheckGroundOccupied, OnCheckGroundOccupied);
        EventManager.AddHandler(GameEvent.OnGroundDefaultColor, OnGroundDefaultColor);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCheckGroundOccupied, OnCheckGroundOccupied);
        EventManager.RemoveHandler(GameEvent.OnGroundDefaultColor, OnGroundDefaultColor);
    }

    

    private void OnCheckGroundOccupied()
    {
        defaultColor = groundRenderer.material.color;
        groundRenderer.material.color=IsOccupied ? defaultColor:Color.green;
    }

    private void OnGroundDefaultColor()
    {
        groundRenderer.material.color=defaultColor;
    }
}

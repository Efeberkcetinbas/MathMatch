using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HexParent : MonoBehaviour
{
    public int towerValue;
    public float yInterval = 0.5f;
    public GameObject hexChildPrefab;
    public Color[] valueColors; // Assign colors in Inspector

    [SerializeField] private BoxCollider boxCollider;
    private Color currentColor;
    private List<HexChild> hexChildren = new List<HexChild>(); // Store spawned HexChildren


    

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

    public void SetInit()
    {
        UpdateColor();
        StartCoroutine(SpawnHexChildrenWithEffect());
    }

    void UpdateColor()
    {
        if (valueColors.Length < 2) return;

        float range = 100f / (valueColors.Length - 1);
        float normalizedValue = towerValue / range;
        int lowerIndex = Mathf.FloorToInt(normalizedValue);
        int upperIndex = Mathf.Clamp(lowerIndex + 1, 0, valueColors.Length - 1);
        float lerpFactor = normalizedValue - lowerIndex;

        currentColor = Color.Lerp(valueColors[lowerIndex], valueColors[upperIndex], lerpFactor);
    }

    public IEnumerator SpawnHexChildrenWithEffect()
    {
        int childCount = Mathf.Max(1, towerValue / 5);
        hexChildren.Clear(); // Clear the list before spawning

        for (int i = 0; i < childCount; i++)
        {
            GameObject child = Instantiate(hexChildPrefab, transform);
            child.transform.localPosition = new Vector3(0, i * yInterval, 0);
            child.transform.localScale = Vector3.zero;

            HexChild hexChildScript = child.GetComponent<HexChild>();
            if (hexChildScript != null)
            {
                hexChildScript.SetHexColor(currentColor);
                hexChildScript.SetHexChild(towerValue);
                hexChildren.Add(hexChildScript); // Store reference in the list
            }

            child.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            child.transform.DORotate(new Vector3(0, 360, 0), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);

            yield return new WaitForSeconds(0.1f);
        }

        SetTopTextActive(hexChildren);
    }

     //**New Method: Get list of HexChildren**
    public List<HexChild> GetHexChildren()
    {
        return hexChildren;
    }

    public void SetTopTextActive(List<HexChild> hexChildren)
    {
        // Get the topmost HexChild
        HexChild topHexChild = GetTopHexChild(hexChildren);

        // Loop through all HexChildren and update their Text GameObject
        foreach (HexChild hexChild in hexChildren)
        {
            // Check if this is the topmost hexChild
            if (hexChild == topHexChild)
            {
                // Set the Text GameObject active for the topmost HexChild
                hexChild.textUI.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                // Set all other Text GameObjects inactive
                hexChild.textUI.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private HexChild GetTopHexChild(List<HexChild> hexChildren)
    {
        HexChild topHexChild = null;
        float maxY = float.MinValue; // Set to lowest possible value to ensure any Y value is greater

        // Loop through the list of hexChildren
        foreach (HexChild hexChild in hexChildren)
        {
            // Compare Y positions to find the topmost
            if (hexChild.transform.position.y > maxY)
            {
                maxY = hexChild.transform.position.y;
                topHexChild = hexChild;
            }
        }

        return topHexChild;
    }

    #region Tap Events
    private void OnReleaseNumber()
    {
        boxCollider.enabled=true;
    }

    private void OnHoldingNumber()
    {
        boxCollider.enabled=false;
    }

    
    #endregion
}

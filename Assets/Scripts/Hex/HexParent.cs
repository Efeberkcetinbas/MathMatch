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

    private Color currentColor;

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
            }

            child.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            child.transform.DORotate(new Vector3(0, 360, 0), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);

            yield return new WaitForSeconds(0.1f);
        }
    }
}

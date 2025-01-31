using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NumberDrag : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.5f; // Duration of the move animation

    // Function to move the number upwards on the Y-axis using DoTween
    public void MoveUp()
    {
        // Move the number upwards with DoTween effect
        transform.DOMoveY(transform.position.y + 2f, moveDuration).SetEase(Ease.OutQuad);
    }
}

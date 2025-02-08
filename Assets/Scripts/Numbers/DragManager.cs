using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragManager : MonoBehaviour
{
    [SerializeField] private LayerMask numberLayer; // Layer for detecting numbers
    [SerializeField] private LayerMask groundLayer; // Layer for detecting ground
    [SerializeField] private float moveDuration = 0.5f; // Duration for smooth movement
    [SerializeField] private float liftHeight = 1f; // How high the number moves when selected

    private NumberDrag selectedNumber;
    private NumberDrag previousNumber;
    private Ground previousGround;
    private List<Ground> allGrounds = new List<Ground>(); // All ground objects
    private bool isLifted = false; // Track if a number is lifted
    private WaitForSeconds waitForSeconds;

    private void Start()
    {
        // Find all ground objects in the scene
        Ground[] foundGrounds = FindObjectsOfType<Ground>();
        allGrounds.AddRange(foundGrounds);

        waitForSeconds=new WaitForSeconds(0.25f);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                CheckIfCanMove(touch);
            }
        }
    }

    void CheckIfCanMove(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        // Check if we touched a HexChild
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, numberLayer))
        {
            // If the hit object is a HexChild, get its parent NumberDrag
            HexChild hexChild = hit.collider.GetComponent<HexChild>();
            if (hexChild != null)
            {
                // Get the parent NumberDrag (i.e., the HexParent that owns this HexChild)
                NumberDrag numberDrag = hexChild.GetComponentInParent<NumberDrag>();
                if (numberDrag != null)
                {
                    // If tapping a different number, reset the previous one
                    if (selectedNumber != null && selectedNumber != numberDrag)
                    {
                        ResetNumberPosition();
                    }

                    // If the tapped number is the same and is lifted, reset it
                    if (selectedNumber == numberDrag && isLifted)
                    {
                        numberDrag.IsSelected=true;
                        ResetNumberPosition();
                        return;
                    }

                    // Raycast forward to check if another number is blocking
                    Vector3 rayDirection = numberDrag.transform.forward;

                    SelectNumber(numberDrag);
                    /*if (Physics.Raycast(numberDrag.transform.position, rayDirection, 50, numberLayer))
                    {
                        Debug.Log($"Can't move {numberDrag.gameObject.name} because another number is in the way.");
                    }
                    else
                    {
                        SelectNumber(numberDrag);
                    }*/
                }
            }
        }
        // If we are dragging and we touch the ground, attempt to move the number to that ground
        else if (selectedNumber != null && Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Ground ground = hit.collider.GetComponent<Ground>();
            if (ground != null && !ground.IsOccupied)
            {
                MoveNumberToGround(ground);
            }
        }
    }

    private void SelectNumber(NumberDrag number)
    {
        if (selectedNumber == number && isLifted)
        {
            ResetNumberPosition();
        }
        else
        {
            selectedNumber = number;
            previousGround = GetGroundUnderNumber(number.transform.position);

            // Move the number **up** smoothly
            //selectedNumber.transform.DOMoveY(selectedNumber.transform.position.y + liftHeight, moveDuration);
            isLifted = true;

            EventManager.Broadcast(GameEvent.OnCheckGroundOccupied);
            EventManager.Broadcast(GameEvent.OnHoldingNumber);

        }
    }

    private void MoveNumberToGround(Ground newGround)
    {
        if (selectedNumber == null) return;

        List<HexChild> hexChildren = selectedNumber.GetComponent<HexParent>().GetHexChildren();

        if (hexChildren.Count > 0)
        {
            EventManager.Broadcast(GameEvent.OnReleaseNumber);
            StartCoroutine(MoveHexChildrenWithEffect(hexChildren, newGround.transform.position));
        }
        else
        {
            // Move number smoothly if no hexChildren exist
            EventManager.Broadcast(GameEvent.OnReleaseNumber);
            selectedNumber.transform.DOMove(newGround.transform.position, moveDuration).OnComplete(() =>
            {
                EventManager.Broadcast(GameEvent.OnMoveNumberToGround);
                StartCoroutine(SetCheckNumbersInDoors());
            });
        }

        isLifted = false;

        // Update ground occupancy
        if (previousGround != null)
        {
            previousGround.IsOccupied = false;
        }

        newGround.IsOccupied = true;
        previousGround = newGround;
        
        EventManager.Broadcast(GameEvent.OnGroundDefaultColor);

        // Clear selection
        //selectedNumber = null;
    }

    private IEnumerator MoveHexChildrenWithEffect(List<HexChild> hexChildren, Vector3 targetPosition)
    {
        if (selectedNumber == null)
        {
            Debug.LogError("selectedNumber is null in MoveHexChildrenWithEffect!");
            yield break;
        }

        HexParent hexParent = selectedNumber.GetComponent<HexParent>();

        if (hexParent == null)
        {
            Debug.LogError($"HexParent component is missing on {selectedNumber.gameObject.name}!");
            yield break;
        }

        // Detach all hexChildren from the parent so they don’t move with it
        foreach (var hexChild in hexChildren)
        {
            hexChild.transform.SetParent(null); // Temporarily detach the child
        }

        // Reverse the order (so top becomes bottom)
        hexChildren.Reverse();

        // Get the fixed X and Z position (these shouldn't change)
        float fixedX = targetPosition.x;
        float fixedZ = targetPosition.z;

        // Start moving from the new topmost (previously bottom) HexChild
        for (int i = 0; i < hexChildren.Count; i++)
        {
            HexChild hexChild = hexChildren[i];
            hexChild.ResetHexText();

            // Target Y position based on new index
            float targetY = i * hexParent.yInterval;

            // Animate each child separately
            hexChild.transform.DOJump(new Vector3(fixedX, targetY, fixedZ), 1.5f, 1, 0.5f)
                .SetEase(Ease.OutQuad);

            hexChild.transform.DORotate(new Vector3(360, 0, 0), 0.5f, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuad);

            yield return new WaitForSeconds(0.1f); // Small delay between each jump
        }

        // Move the main parent object AFTER all hexChildren have reached their new positions
        selectedNumber.transform.DOMove(targetPosition, moveDuration).OnComplete(() =>
        {
            // Reattach hexChildren to the parent
            foreach (var hexChild in hexChildren)
            {
                hexChild.transform.SetParent(hexParent.transform); // Reattach to the correct HexParent
            }

            EventManager.Broadcast(GameEvent.OnMoveNumberToGround);
            selectedNumber = null;
            hexParent.SetTopTextActive(hexChildren);
            StartCoroutine(SetCheckNumbersInDoors());
        });
    }

    

    private void ResetNumberPosition()
    {
        if (selectedNumber == null) return;

        // Move the number **down** back to its original Y position
        selectedNumber.transform.DOMoveY(previousGround.transform.position.y, moveDuration);
        isLifted = false;
        selectedNumber.IsSelected=false;
        selectedNumber = null;
        EventManager.Broadcast(GameEvent.OnGroundDefaultColor);
    }

    private IEnumerator SetCheckNumbersInDoors()
    {
        yield return waitForSeconds;
        EventManager.Broadcast(GameEvent.OnCheckNumbersInDoor);
    }

    private Ground GetGroundUnderNumber(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            return hit.collider.GetComponent<Ground>();
        }
        return null;
    }
}
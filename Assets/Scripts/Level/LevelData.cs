using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Level Configuration")]
    public int numberOfDoors; // Number of doors in the level
    public int numberOfPeoplePerDoor; // Number of people for each door

    [Header("Allowed Operations")]
    public List<char> allowedOperators = new List<char> { '+', '-', '*', '/' }; // Operators allowed for this level

    [Header("Value Range for Operations")]
    public int minValue = 1; // Minimum value for operations (e.g., +1, -1)
    public int maxValue = 10; // Maximum value for operations (e.g., +10, -10)
}

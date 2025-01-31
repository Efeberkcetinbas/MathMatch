using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelDoors", menuName = "Level/Doors Config")]
public class LevelDoors : ScriptableObject
{
    [System.Serializable]
    public class DoorData
    {
        public int value;
        public Color color;
        public GameObject doorPrefab;
    }

    public List<DoorData> doors = new List<DoorData>();
}
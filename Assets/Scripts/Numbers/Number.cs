using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewNumberConfig", menuName = "Number System/Number Config")]
public class Number : ScriptableObject
{
    [System.Serializable]
    public class NumberData
    {
        public int value;
        public GameObject numberPrefab;
    }

    public List<NumberData> numbers = new List<NumberData>();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public LevelData currentLevelData;
    public GameObject doorPrefab;
    public GameObject personPrefab;
    public Transform doorParent;
    public Transform personParent;

    private List<Door> doors = new List<Door>();
    private List<Person> allPeople = new List<Person>();
    private Person firstTouchedPerson;
    private Person secondTouchedPerson;

    private void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        // Clear previous doors and people
        foreach (Transform child in doorParent) Destroy(child.gameObject);
        foreach (Transform child in personParent) Destroy(child.gameObject);

        doors.Clear();
        allPeople.Clear();

        // Step 1: Generate doors and assign target values
        for (int i = 0; i < currentLevelData.numberOfDoors; i++)
        {
            GameObject doorObj = Instantiate(doorPrefab, doorParent);
            Door door = doorObj.GetComponent<Door>();

            door.targetValue = Random.Range(20, 100); // Example range for target values
            door.Initialize();

            // Position doors along the X-axis
            door.transform.position = new Vector3(i * 3, 0, 0);

            doors.Add(door);
        }

        // Step 2: Generate and assign people to doors
        GenerateAndAssignPeopleToDoors();
    }

    private void GenerateAndAssignPeopleToDoors()
    {
        List<int> peopleValues = new List<int>();
        foreach (Door door in doors)
        {
            // Generate people operations randomly and match them to the door target value
            List<int> doorOperations = new List<int>();
            int currentValue = 0;

            while (doorOperations.Count < currentLevelData.numberOfPeoplePerDoor)
            {
                int value = Random.Range(currentLevelData.minValue, currentLevelData.maxValue);
                doorOperations.Add(value);
                currentValue += value;
            }

            // Adjust the last value to match the door target value
            int adjustment = door.targetValue - currentValue;
            if (doorOperations.Count > 0)
            {
                doorOperations[doorOperations.Count - 1] += adjustment;
            }

            // Add operations to the list of all people
            foreach (var value in doorOperations)
            {
                peopleValues.Add(value);
            }
        }

        // Step 3: Create people objects and assign their operation values
        foreach (int value in peopleValues)
        {
            GameObject personObj = Instantiate(personPrefab, personParent);
            Person person = personObj.GetComponent<Person>();
            person.operation = $"+{value}";
            person.Initialize();
            allPeople.Add(person);
        }

        // Step 4: Shuffle and assign people to doors
        ShufflePeople();
        AssignPeopleToDoors();
    }

    private void ShufflePeople()
    {
        // Shuffle the people so they are randomly mixed
        for (int i = 0; i < allPeople.Count; i++)
        {
            Person temp = allPeople[i];
            int randomIndex = Random.Range(i, allPeople.Count);
            allPeople[i] = allPeople[randomIndex];
            allPeople[randomIndex] = temp;
        }
    }

    private void AssignPeopleToDoors()
    {
        int index = 0;

        foreach (var door in doors)
        {
            door.peopleInOrder = new List<Person>();

            for (int i = 0; i < currentLevelData.numberOfPeoplePerDoor; i++)
            {
                // Get the person from the shuffled list
                Person person = allPeople[index];

                // Position the person relative to the door
                float x = door.transform.position.x;
                float y = door.transform.position.y;
                float z = door.transform.position.z - (i + 1) * 2; // Z-axis offset

                // Set the position and parent of the person
                person.transform.position = new Vector3(x, y, z);
                person.transform.SetParent(personParent);

                // Add the person to the door's list
                door.peopleInOrder.Add(person);

                index++;
            }

            door.UpdateValueDisplay();
        }
    }

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        // Detect if the player is touching on the screen
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // If we hit a person
                Person touchedPerson = hit.collider.GetComponent<Person>();

                if (touchedPerson != null)
                {
                    // First person touch
                    if (firstTouchedPerson == null)
                    {
                        firstTouchedPerson = touchedPerson;
                    }
                    else if (secondTouchedPerson == null)
                    {
                        secondTouchedPerson = touchedPerson;

                        // Swap positions
                        SwapPeoplePositions(firstTouchedPerson, secondTouchedPerson);

                        // Reset touch tracking
                        firstTouchedPerson = null;
                        secondTouchedPerson = null;
                        CheckLevelSuccess();
                    }
                }
            }
        }
    }

    private void SwapPeoplePositions(Person person1, Person person2)
    {
        // Swap positions of the two persons
        Vector3 tempPosition = person1.transform.position;
        person1.transform.position = person2.transform.position;
        person2.transform.position = tempPosition;

        // Optionally swap their operations too, depending on your needs
        string tempOperation = person1.operation;
        person1.operation = person2.operation;
        person2.operation = tempOperation;
    }

    public void CheckLevelSuccess()
    {
        foreach (var door in doors)
        {
            if (!door.IsCorrect())
            {
                Debug.Log("Level Failed!");
                return;
            }
        }

        Debug.Log("Level Successful!");
    }
}

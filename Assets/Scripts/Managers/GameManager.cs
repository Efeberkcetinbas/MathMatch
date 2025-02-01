using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameData gameData;


    private WaitForSeconds waitForSeconds;

    public List<DoorController> allDoors = new List<DoorController>(); // All door objects


    private void Awake() 
    {
        ClearData(true);
    }

    private void Start()
    {
        waitForSeconds=new WaitForSeconds(2);

        //TEMP
        Invoke("OnGameStart",2);
        
    }

    private void OnGameStart()
    {
        DoorController[] foundDoors = FindObjectsOfType<DoorController>();
        allDoors.AddRange(foundDoors);
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnCheckNumbersInDoor,OnCheckNumbersInDoor);

    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnCheckNumbersInDoor,OnCheckNumbersInDoor);

    }

    private void OnCheckNumbersInDoor()
    {
        bool allDoorsMatch = allDoors.Count > 0 && allDoors.All(door => door.matchNumbersValue);

        if (allDoorsMatch)
        {
            Debug.Log("Game Success! All doors match.");
            // Call success function here
        }
        else
        {
            Debug.Log("Not all doors match. Keep playing!");
            return;
        }
    }
    
    

    private void OnNextLevel()
    {
        ClearData(false);
        
    }

    private void OnRestartLevel()
    {
        ClearData(false);
    }

    
   
    private void OnGameOver()
    {
        gameData.isGameEnd=true;
        StartCoroutine(OpenFail());
    }

    private void ClearData(bool val)
    {
        gameData.isGameEnd=val;
    }

    private IEnumerator OpenSuccess()
    {
        yield return waitForSeconds;
        OpenSuccessPanel();
    }


    private void OpenSuccessPanel()
    {
        EventManager.Broadcast(GameEvent.OnSuccessUI);
    }

    private IEnumerator OpenFail()
    {
        yield return waitForSeconds;
        OpenFailPanel();
    }


    private void OpenFailPanel()
    {
        //effektif
        EventManager.Broadcast(GameEvent.OnFailUI);
    }



    
}

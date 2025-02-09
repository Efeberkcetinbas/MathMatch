using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[Serializable]
public class HexSounds
{
    public AudioClip HexSelectSound;
    public AudioClip HexEffectSound;
    public AudioClip HexParentPositionCompletedSound;
}

[Serializable]
public class GameManagementSounds
{
    public AudioClip SuccessSound,SuccessUISound,RestartSound ,NextLevelSound,StartSound,FailUISound;
}

public class AudioManager : MonoBehaviour
{
    public AudioClip GameLoop,BuffMusic;
    

    AudioSource musicSource,effectSource;

    [SerializeField] private HexSounds hexSounds;
    [SerializeField] private GameManagementSounds gameManagementSounds;


    private void Start() 
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = GameLoop;
        //musicSource.Play();
        effectSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable() 
    {
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.AddHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnGameStart,OnGameStart);

        EventManager.AddHandler(GameEvent.OnHoldingNumber,OnHoldingNumber);
        EventManager.AddHandler(GameEvent.OnHexAnimate,OnHexAnimate);
        EventManager.AddHandler(GameEvent.OnMoveNumberToGround,OnMoveNumberToGround);

    }
    private void OnDisable() 
    {
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.RemoveHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnGameStart,OnGameStart);

        EventManager.RemoveHandler(GameEvent.OnHoldingNumber,OnHoldingNumber);
        EventManager.RemoveHandler(GameEvent.OnHexAnimate,OnHexAnimate);
        EventManager.RemoveHandler(GameEvent.OnMoveNumberToGround,OnMoveNumberToGround);

    }

    
    #region GameManagement

    private void OnSuccess()
    {
        effectSource.PlayOneShot(gameManagementSounds.SuccessSound);
    }

    private void OnSuccessUI()
    {
        effectSource.PlayOneShot(gameManagementSounds.SuccessUISound);
    }


    private void OnRestartLevel()
    {
        effectSource.PlayOneShot(gameManagementSounds.RestartSound);
    }

    private void OnNextLevel()
    {
        effectSource.PlayOneShot(gameManagementSounds.NextLevelSound);
    }

    private void OnGameStart()
    {
        effectSource.PlayOneShot(gameManagementSounds.StartSound);
    }

    private void OnFailUI()
    {
        effectSource.PlayOneShot(gameManagementSounds.FailUISound);
    }

    #endregion

    #region Hex

    private void OnHoldingNumber()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(hexSounds.HexSelectSound);
    }

    private void OnHexAnimate()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(hexSounds.HexEffectSound);
    }

    private void OnMoveNumberToGround()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(hexSounds.HexParentPositionCompletedSound);
    }

    #endregion

   

}

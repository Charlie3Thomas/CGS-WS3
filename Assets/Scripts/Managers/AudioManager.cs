using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Serializable]
    public class sfxLib
    {
        public string Name;
        public EventReference sfxPath;
    }

    [Header("Volume Sliders")]
    public float masterVolume = 0.9f;
    public float bgVolume = 0.5f;
    public float sfxVolume = 0.5f;
    public int testIndex = 2;

    //FMOD Variables
    Bus masterBus;
    //Bus backgroundBus;
    //Bus sfxBus;
    EventInstance ambienceInstance;
    EventInstance musicInstance;
    EventInstance chargeInstance;

    private EventInstance oceanAmbienceInstance;
    private EventInstance volcanoAmbienceInstance;


    [Header("Event References Selector")]
    [Space(20)]

    public UIFmodReferences uiEvents;
    public AmbienceFmodReferences ambienceEvents;


    /*
    public EventReference ambienceEvent;
    public EventReference musicEvent;
    private static string sfxDir = "event:/SFX/";
    public List<sfxLib> sfxObjectsList;
    */
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        masterBus = RuntimeManager.GetBus("Bus:/");
        //backgroundBus = RuntimeManager.GetBus("Bus:/Master/Background");
        //sfxBus = RuntimeManager.GetBus("Bus:/Master/SFX");
         StartAmbience();
         StartMusic();
    }

    private void Update()
    {
        masterBus.setVolume(this.masterVolume);
        //backgroundBus.setVolume(this.bgVolume);
        //sfxBus.setVolume(this.sfxVolume);
        //musicInstance.setParameterByName("Scene", SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeSFXVolume(float newSFXVolume)
    {
        this.sfxVolume = newSFXVolume;
    }

    public void ChangeBGVolume(float newBGVolume)
    {
        this.bgVolume = newBGVolume;
    }

    public void ChangeMasterVolume(float newMasterVolume)
    {
        this.masterVolume = newMasterVolume;
    }

    public void StartOceanAmbience(Transform transform)
    {
        oceanAmbienceInstance = RuntimeManager.CreateInstance(ambienceEvents.oceanAmbienceEvent);
        RuntimeManager.AttachInstanceToGameObject(oceanAmbienceInstance, transform);
        oceanAmbienceInstance.start();
    }

    public void StopOceanAmbience()
    {
        //Note for instances that only end when game over, or quit. Have one function for releasing, and stop the bus ambience bus instead. Once set up. 
        oceanAmbienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        oceanAmbienceInstance.release();
    }

    public void StartVolcanoAmbience(Transform transform)
    {
        volcanoAmbienceInstance = RuntimeManager.CreateInstance(ambienceEvents.volcanoAmbienceEvent);
        RuntimeManager.AttachInstanceToGameObject(volcanoAmbienceInstance, transform);
        volcanoAmbienceInstance.start();
    }

    public void StopVolcanoAmbience()
    {
        //Note for instances that only end when game over, or quit. Have one function for releasing, and stop the bus ambience bus instead. Once set up. 
        volcanoAmbienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        volcanoAmbienceInstance.release();
    }
   
    
    
    public void StartAmbience()
    {
        ambienceInstance = RuntimeManager.CreateInstance(ambienceEvents.windAmbienceEvent);
        ambienceInstance.start();
        
    }
    
    public void StartMusic()
    {
        musicInstance = RuntimeManager.CreateInstance(ambienceEvents.musicEvent);
        musicInstance.start();
        
    }
    
    //Handle any continous 2D ambience events here
    public void ReleaseAmbience()
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();
    }
   
    public void ReleaseMusic()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }
    /*
    public void Resume()
    {
        masterBus.setPaused(false);
    }

    public void Pause()
    {
        masterBus.setPaused(true);
    }
    */
    public void OnDestroy()
    {
        StopOceanAmbience();
        StopVolcanoAmbience();
        ReleaseAmbience();
        ReleaseMusic();
    }
}
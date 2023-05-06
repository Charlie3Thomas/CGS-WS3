using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using UnityEngine;
using CT.Lookup;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

   
    EventInstance ambienceInstance;
    EventInstance musicInstance;
    EventInstance chargeInstance;

    private EventInstance oceanAmbienceInstance;
    private EventInstance volcanoAmbienceInstance;


    [Header("Event References Selector")]
    [Space(20)]

    public UIFmodReferences uiEvents;
    public AmbienceFmodReferences ambienceEvents;

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
        FmodRouting.SetUpBuses();
        StartAmbience();
        StartMusic();
    }

    public void StartDisasterAudio(CTDisasters disaster, float intensity)
    {
        if(intensity != -1f && disaster != CTDisasters.None) //Check charlies no disaster -1 val to not set param 
        {
            FmodParameters.SetGlobalParamByName("Intensity", intensity);
            FmodParameters.SetParamByLabelName(musicInstance, "Play", "Play");
        }

        Debug.Log("Audio disaster:" + disaster + " Intensity: " + intensity);
        switch(disaster)
        {
            case (CTDisasters.Earthquake):
            AudioPlayback.PlayOneShot(ambienceEvents.earthquakeDisaster, null);
            AudioPlayback.PlayOneShot(ambienceEvents.screamingEvent, null);
            StartCoroutine("TenseMusicTimer");
            break;

            case (CTDisasters.Tsunami):
            AudioPlayback.PlayOneShot(ambienceEvents.tsunamiDisaster, null);
            AudioPlayback.PlayOneShot(ambienceEvents.screamingEvent, null);
            StartCoroutine("TenseMusicTimer");
            break;

            case (CTDisasters.Volcano):
            AudioPlayback.PlayOneShot(ambienceEvents.volcanoDisaster, null);
            AudioPlayback.PlayOneShot(ambienceEvents.screamingEvent, null);
            StartCoroutine("TenseMusicTimer");
            break;
            
            case (CTDisasters.Tornado):
            AudioPlayback.PlayOneShot(ambienceEvents.tornadoDisaster, null);
            AudioPlayback.PlayOneShot(ambienceEvents.screamingEvent, null);
            StartCoroutine("TenseMusicTimer");
            break;

            default:
            //No disaster
            break;
        }
    }

    private IEnumerator TenseMusicTimer()
    {
        yield return new WaitForSeconds(20f);
        FmodParameters.SetParamByLabelName(musicInstance, "Play", "Stop");

        //yield return new WaitForSeconds(WaitTime);

        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
        
        yield return new WaitForSeconds(3f);

        FmodParameters.SetGlobalParamByName("Intensity", 0f);
        StartMusic();
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
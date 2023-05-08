using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class MenuAudioManager : MonoBehaviour
{
    public static MenuAudioManager Instance => m_instance;
    private static MenuAudioManager m_instance;

    public MainMenuAuidioRefs mainMenuRefs {get; private set;}

    EventInstance menuMusicInstance;
    EventInstance computerAmbienceInstance;

    void Awake()
    {
        m_instance = this;
        mainMenuRefs = GetComponent<MainMenuAuidioRefs>();
        AudioPlayback.PlayOneShot(mainMenuRefs.mainMenuStartEvent, null);

    }
    // Start is called before the first frame update
    void Start()
    {
        FmodRouting.SetUpBuses();

        StartMenuMusic();
    }

    public void StartMenuMusic()
    {
        menuMusicInstance = RuntimeManager.CreateInstance(mainMenuRefs.mainMenuMusicEvent);
        menuMusicInstance.start();

        computerAmbienceInstance = RuntimeManager.CreateInstance(mainMenuRefs.menuComputerEvent);
        computerAmbienceInstance.start();
        
    }

    public void ReleaseMenuMusic()
    {
        menuMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        menuMusicInstance.release();
    }

    void OnDestroy()
    {
        ReleaseMenuMusic();

    }
    

    

    
}

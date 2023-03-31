using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class MenuAudioManager : MonoBehaviour
{
    public static MenuAudioManager instance => m_instance;
    private static MenuAudioManager m_instance;

    public MainMenuAuidioRefs mainMenuRefs {get; private set;}

     EventInstance menuMusicInstance;

    
    // Start is called before the first frame update
    void Start()
    {
        m_instance = this;
        mainMenuRefs = GetComponent<MainMenuAuidioRefs>();

        FmodRouting.SetUpBuses();

        StartMenuMusic();
    }

    public void StartMenuMusic()
    {
        menuMusicInstance = RuntimeManager.CreateInstance(mainMenuRefs.mainMenuMusicEvent);
        menuMusicInstance.start();
        
    }

    public void ReleaseMenuMusic()
    {
        menuMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        menuMusicInstance.release();
    }

    

    
}

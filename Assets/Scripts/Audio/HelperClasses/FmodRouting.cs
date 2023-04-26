using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public static class FmodRouting
{
    //Bus Variables 
    public static Bus masterBus;
    public static Bus musicBus;
    public static Bus sfxBus;
    public static Bus uiBus;

    // Update is called once per frame
    public static void SetUpBuses()
    {
        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        uiBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/UI");
    }

    public static void ChangeBusVolume(Bus bus, float newVal)
    {
        bus.setVolume(newVal);
    }

   
}

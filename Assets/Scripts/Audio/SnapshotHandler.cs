using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class SnapshotHandler : MonoBehaviour
{
    public static SnapshotHandler instance => m_instace;
    private static SnapshotHandler m_instace;

    public EventReference optionsSnapshot;
    public EventReference panSnapshot;

    public EventInstance optionsSnapShotinstance {get; private set;}
    public EventInstance panSnapShotinstance {get; private set;}
    
    // Start is called before the first frame update
    private void Start()
    {
        m_instace = this;
    }

    public void StartOptionsSnapShot()
    {
        
        optionsSnapShotinstance = RuntimeManager.CreateInstance(optionsSnapshot);
        optionsSnapShotinstance.start();
    }

    public void StartCameraPanSnapShot()
    {
        panSnapShotinstance = RuntimeManager.CreateInstance(panSnapshot);
        panSnapShotinstance.start();
    }

    // Update is called once per frame
    public void StopSnapShot(EventInstance instance)
    {
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        instance.release();
    }
}

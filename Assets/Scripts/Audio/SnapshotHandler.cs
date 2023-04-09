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
    EventInstance snapShotinstance;
    
    // Start is called before the first frame update
    private void Start()
    {
        m_instace = this;
    }

    public void StartOptionsSnapShot( )
    {
        
        snapShotinstance = RuntimeManager.CreateInstance(optionsSnapshot);
        snapShotinstance.start();
    }

    // Update is called once per frame
    public void StopSnapShot()
    {
        snapShotinstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        snapShotinstance.release();
    }
}

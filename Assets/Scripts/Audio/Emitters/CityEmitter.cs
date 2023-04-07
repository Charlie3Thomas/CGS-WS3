using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public enum CityDistricts {DISTRICT1, DISTRICT2, DISTRICT3};

public class CityEmitter : MonoBehaviour
{
    [SerializeField] private CityDistricts district;
    EventInstance ambienceInstance;

    // Start is called before the first frame update
    void Start()
    {
        //Create event instance
        ambienceInstance = RuntimeManager.CreateInstance(AudioManager.Instance.ambienceEvents.cityAmbienceEvent);
        StartCityAmbience(district);
    }

    void StartCityAmbience(CityDistricts district)
    {
        //Set param value based on what district has been chosen for emitter in the inspector
        string param = "";
        switch(district)
        {
            case CityDistricts.DISTRICT1:
            param = "FirstDistrict";
            break;

            case CityDistricts.DISTRICT2:
            param = "SecondDistrict";
            break;

            case CityDistricts.DISTRICT3:
            param = "ThirdDistrict";
            break;
        }
        
        //Set the instances param state
        FmodParameters.SetParamByLabelName(ambienceInstance, "CityState", param);

        //Aattach to this objects transform and start the 3D city ambience event
        RuntimeManager.AttachInstanceToGameObject(ambienceInstance, this.transform);
        ambienceInstance.start();
    }

    void OnDestroy()
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();
    }
}

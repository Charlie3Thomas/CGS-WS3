using CT.Lookup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DisasterEffectManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineImpulseSource impulseSrouce;
    [SerializeField]
    private CinemachineVirtualCamera mapVCam;
    [SerializeField]
    private GameObject tornado;
    [SerializeField]
    private GameObject tsunamiWave;

    void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
        }
    }

    private void ShowDisasterEffect(Disaster disaster)
    {
        float intensity;
        switch (disaster.type)
        {
            case CTDisasters.Earthquake:
                intensity = 2f * disaster.intensity;
                Shake(intensity);
                break;
            case CTDisasters.Tsunami:
                intensity = 1.2f * disaster.intensity;
                Shake(intensity);
                break;
            case CTDisasters.Volcano:
                intensity = 1.5f * disaster.intensity;
                Shake(intensity);
                break;
            case CTDisasters.Tornado:
                intensity = 1.2f * disaster.intensity;
                Shake(intensity);
                break;
            case CTDisasters.None:
                break;
            default:
                break;
        }
    }

    private void Shake(float intensity)
    {
        impulseSrouce.GenerateImpulse(intensity);
    }
}

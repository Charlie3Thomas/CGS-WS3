using CT.Lookup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterEffectManager : MonoBehaviour
{
    [SerializeField]
    private CameraShake cameraShake;
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
            //cameraShake.Shake(5f, 0.5f);
            //setSeaLevelHigh(1f);
        }
    }

    private void ShowDisasterEffect(Disaster disaster)
    {
        float duration;
        float intensity;
        switch (disaster.type)
        {
            case CTDisasters.Earthquake:
                duration = disaster.intensity > 4 ? 7 : 5;
                intensity = disaster.intensity * 0.07f;
                cameraShake.Shake(duration, intensity);
                break;
            case CTDisasters.Tsunami:
                duration = disaster.intensity > 4 ? 7 : 5;
                intensity = disaster.intensity * 0.03f;
                cameraShake.Shake(duration, intensity);
                break;
            case CTDisasters.Volcano:
                duration = disaster.intensity > 4 ? 7 : 5;
                intensity = disaster.intensity * 0.07f;
                cameraShake.Shake(duration, intensity);
                break;
            case CTDisasters.Tornado:
                duration = disaster.intensity > 4 ? 7 : 5;
                intensity = disaster.intensity * 0.04f;
                cameraShake.Shake(duration, intensity);
                break;
            case CTDisasters.None:
                break;
            default:
                break;
        }
    }
}

using CT.Lookup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterEffectManager : MonoBehaviour
{
    [SerializeField]
    private CameraShake cameraShake;
    [SerializeField]
    private GameObject pineLeavesPrefab;
    [SerializeField]
    private GameObject sea;
    [SerializeField]
    private TsunamiObjectsHolder tsunamiHolder;

    private float seaAmp1, seaAmp2, seaAmp3;
    private float seaHighAmp1 = 1.5f;
    private float seaHighAmp2 = 0.8f;
    private float seaHighAmp3 = 2f;

    private Material seaMaterial;
    
    void Start()
    {
        seaMaterial = sea.GetComponent<Renderer>().sharedMaterial;
        seaAmp1 = seaMaterial.GetFloat("_Amplitude1");
        seaAmp2 = seaMaterial.GetFloat("_Amplitude2");
        seaAmp3 = seaMaterial.GetFloat("_Amplitude3");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            //cameraShake.Shake(5f, 0.5f);
            //setSeaLevelHigh(1f);
        }
    }

    private void showDisasterEffect(Disaster disaster)
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
                setSeaLevelHigh(disaster.intensity);
                tsunamiHolder.PlayTsunamiVFX(disaster.intensity);
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

    private void setSeaLevelHigh(float intensity)
    {
        seaMaterial.SetFloat("_Amplitude1", Mathf.Lerp(seaAmp1, seaHighAmp1, intensity*0.1f));
        seaMaterial.SetFloat("_Amplitude2", Mathf.Lerp(seaAmp2, seaHighAmp2, intensity * 0.1f));
        seaMaterial.SetFloat("_Amplitude3", Mathf.Lerp(seaAmp3, seaHighAmp3, intensity * 0.1f));

        Invoke(nameof(resetSeaLevel), 7f);
    }

    private void resetSeaLevel()
    {
        seaMaterial.SetFloat("_Amplitude1", seaAmp1);
        seaMaterial.SetFloat("_Amplitude2", seaAmp2);
        seaMaterial.SetFloat("_Amplitude3", seaAmp3);
    }
}

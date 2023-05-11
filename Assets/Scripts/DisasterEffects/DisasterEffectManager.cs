using CT.Lookup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using CT;

public class DisasterEffectManager : MonoBehaviour
{
    public static DisasterEffectManager instance;

    public GameObject currentEffect;
    [SerializeField]
    private CinemachineImpulseSource impulseSrouce;
    [SerializeField]
    private CinemachineVirtualCamera mapVCam;

    // Tornado
    [SerializeField]
    private GameObject tornado;

    // Tsunami
    [SerializeField]
    private GameObject tsunamiWave;
    [SerializeField]
    private Transform tsunamiTransform;

    // Volcano
    [SerializeField]
    private ParticleSystem volcano;

    public float testIntensity = 1f;
    public float duration = 10f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            Debug.LogError("Multiple DisasterEffectManager Instances!");
        }
        else
            instance = this;
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void ShowDisasterEffect(CTDisasters type, float intensity)
    {
        if(currentEffect != null)
        {
            Destroy(currentEffect);
            CinemachineImpulseManager.Instance.Clear();
        }    

        switch (type)
        {
            case CTDisasters.Earthquake:
                StartCoroutine(EarthquakeEffect(intensity));
                break;
            case CTDisasters.Tsunami:
                StartCoroutine(TsunamiEffect(intensity));
                break;
            case CTDisasters.Volcano:
                StartCoroutine(VolcanoEffect(intensity));
                Shake(intensity);
                break;
            case CTDisasters.Tornado:
                StartCoroutine(TornadoEffect(intensity));
                break;
            case CTDisasters.None:
                break;
            default:
                break;
        }
    }

    private IEnumerator TsunamiEffect(float _intensity)
    {
        float intensity = 1.3f * _intensity;
        Shake(intensity);
        GameObject tGO = Instantiate(tsunamiWave, tsunamiTransform.position, tsunamiTransform.rotation);
        currentEffect = tGO;
        tGO = currentEffect;
        float size = RAUtility.Remap(_intensity, 1, 10, 0.5f, 1.0f);
        tGO.transform.GetChild(0).localScale = new Vector3(tGO.transform.GetChild(0).localScale.x * size,
            tGO.transform.GetChild(0).localScale.y * size, tGO.transform.GetChild(0).localScale.z * size);
        // Affect buildings (Sweep along with water)
        yield return new WaitForSeconds(duration);
        currentEffect = null;
        Destroy(tGO);
        yield return null;
    }

    private IEnumerator EarthquakeEffect(float _intensity)
    {
        float intensity = 2f * _intensity;
        Shake(intensity);
        // Affect buildings (Sink down ground)
        yield return null;
    }

    private IEnumerator VolcanoEffect(float _intensity)
    {
        float intensity = 1.5f * _intensity;
        Shake(intensity);
        // Play effect
        volcano.Play();

        // Affect buildings (Blow them up)
        yield return null;
    }

    private IEnumerator TornadoEffect(float _intensity)
    {
        float intensity = 1.2f * _intensity;
        Shake(intensity);
        // Play effect and destroy if necessary

        // Affect buildings (Sweep them up the screen)
        yield return null;
    }

    private void Shake(float intensity)
    {
        impulseSrouce.GenerateImpulse(intensity);
    }
}

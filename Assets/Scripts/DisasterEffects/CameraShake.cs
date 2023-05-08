using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	private Transform camTransform;

	private float shakeDuration = 0f;
	private float intensity = 0.3f;

	private bool isActive = false;
	private float durationFraction = 0f;
	private float fadeInFactor = 0f;
	private float totalDuration = 0f;

	Vector3 originalPos;

	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
	}

	void Update()
	{
		if (shakeDuration > 0)
		{
			durationFraction = shakeDuration / totalDuration;
			if (durationFraction > 0.6f)
				fadeInFactor = 1 - durationFraction;
			else
				fadeInFactor = durationFraction;
			camTransform.localPosition = originalPos + Random.insideUnitSphere * intensity * fadeInFactor;
			shakeDuration -= Time.deltaTime;
		}
		else if(isActive)
		{
			shakeDuration = 0f;
			camTransform.localPosition = originalPos;
			isActive = false;
		}

	}

	public void Shake(float duration, float intensity)
    {
		isActive = true;
		originalPos = camTransform.localPosition;
		shakeDuration = totalDuration = duration;
		this.intensity = intensity;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanEmitter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.StartOceanAmbience(this.transform);
    }

   
}

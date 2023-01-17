using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoEmitter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.StartVolcanoAmbience(this.transform);
    }

    
}

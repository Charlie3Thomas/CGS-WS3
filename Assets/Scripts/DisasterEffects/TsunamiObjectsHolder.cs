using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TsunamiObjectsHolder : MonoBehaviour
{
    private VisualEffect[] vfxArray;

    void Start()
    {
        vfxArray = transform.GetComponentsInChildren<VisualEffect>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            foreach (var v in vfxArray)
                v.SendEvent("OnPlay");
        }
    }

    public void PlayTsunamiVFX(float intensity)
    {
        for(int i=0; i<vfxArray.Length; i++)
        {
            if(intensity < 3)
            {
                if (i % 3 == 0)
                {
                    vfxArray[i].SendEvent("OnPlay");
                }
            }
            else if(intensity < 6)
            {
                if (i % 2 == 0)
                {
                    vfxArray[i].SendEvent("OnPlay");
                }
            }
            else
                vfxArray[i].SendEvent("OnPlay");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles fire and forget events, i.e oneshots. It handles basic oneshot firing without parameters,  //
// as well oneshot playback with parameter value, it could be extended to handle rigidbody and velocity handling //

public static class AudioPlayback
{
    //Use to play basic one shot with no param values, can make 3D by passing gameobj as argument, or leave argument as null if 2D
    public static void PlayOneShot(FMODUnity.EventReference fmodEvent, GameObject objToAttachTo)
    {
        FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);

        //Check if position has been given to attach event to that position and make 3D
        if (objToAttachTo != null)
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, objToAttachTo.transform);
        }

        instance.start();
        instance.release();

    }

    //This is a genric function that has a generic param type 'value', when calling this function you will need to cast 'value' to the desired type
    // and replace <T> with desired type
    public static void PlayOneShotWithParameters<T>(FMODUnity.EventReference fmodEvent, GameObject objToAttachTo, params (string name, T value)[] parameters)
    {
        FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);

        foreach (var (name, value) in parameters)
        {
            //Dynamic used so we can cast value function param input when this function is called
            dynamic paramVal = value;

            //If param value is of type string, set as labeled param 
            if (paramVal.GetType() == typeof(string))
            {
                instance.setParameterByNameWithLabel(name, paramVal);  
            }

            //If param value is of type float or int, set param as continous or discrete 
            else if (paramVal.GetType() == typeof(float) || paramVal.GetType() == typeof(int))
            {
                instance.setParameterByName(name, paramVal);
            }
        }

        //Check if position has been given to attach event to that position and make 3D
        if (objToAttachTo != null)
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, objToAttachTo.transform); 
        }

        instance.start();
        instance.release();

    }
}

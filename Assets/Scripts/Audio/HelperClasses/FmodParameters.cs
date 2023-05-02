using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* This is an FmodParameters class and is part of the SamSquanch fmod helper library, it is responsible for setting params, each method contains the implementation of every way to set a param using the FMOD API.
 * To use this script reference this class on a script, then if you are using discrete or continous params 
 * you will call the 'SetParamByName' method, this is a generic method, containing three arguments, one of which is of a generic data type,
 * this allows you to send a param value of any data type. The first argument of this method is an fmod event instance 
 * and will be supplied by a part of the audio engine, which is responsible for controlling the event where parameter changes are desired.
 * The second argument is of string data type and will be the parameter name, with the third being the param value.
 * The 'SetParamByLabelName will allow values to be set of params that are of the label type, taking three arguments: event instance, the label param name and the param state
 * Author: S Scott
 * */

public static class FmodParameters
{
    /***************************** LOCAL PARAMETERS **********************************************************************************************************************/
    
    //Used to set state value of fmod labled params
    public static void SetParamByLabelName(FMOD.Studio.EventInstance eventInstance, string labelParamName, string labelParamState)
    {
        eventInstance.setParameterByNameWithLabel(labelParamName, labelParamState);
    }

    public static float GetParamByName(FMOD.Studio.EventInstance eventInstance, string labelParamName)
    {
        float value;
        float finalValue;

        eventInstance.getParameterByName(labelParamName, out value, out finalValue);

        return finalValue;
    }

    //Used to set value of fmod discrete and continous parameter types
    public static void SetParamByName<T>(FMOD.Studio.EventInstance eventInstance, string paramName, T paramValue)
    {
        dynamic value = paramValue;                          /*Generic <T> cannot be used with fmodAPI, therefore dynamic type is required for use when setting param value.
                                                             NOTE: this may cause: error CS0656: Missing compiler required member 'Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create', please install the required reference
                                                             Please navigate to Edit > Project Settings > Player > Other Settings > Configuration > API Compatibility Level and change from .NET Standard 2.0 to .NET, or  4.x .NET Framework*/
        eventInstance.setParameterByName(paramName, value);
    }
    /***************************** GLOBAL PARAMETERS*********************************************************************************************************************/

    //Used to set fmod global parameters of labled type
    public static void SetGlobalParamByLabelName(string labelParamName, string labelParamState)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByNameWithLabel(labelParamName, labelParamState);
    }

    //Used to set fmod global parameters of continous and discrete param types
    public static void SetGlobalParamByName<T>(string globalParamName, T globalParamValue)
    {
        dynamic value = globalParamValue;                          /*Generic <T> cannot be used with FMODAPI, therefore dynamic type is required for use when setting param value.
                                                                   NOTE: this may cause: error CS0656: Missing compiler required member 'Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create', please install the required reference
                                                                   Please navigate to Edit > Project Settings > Player > Other Settings > Configuration > API Compatibility Level and change from .NET Standard 2.0 to .NET, or  4.x .NET Framework*/

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(globalParamName, value);
    }

   
}

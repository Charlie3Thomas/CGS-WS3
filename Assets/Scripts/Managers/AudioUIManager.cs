using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Enum for slider types
public enum AudioSliders {master, music, sfx, ui}

public class AudioUIManager : MonoBehaviour
{
    [SerializeField] private GameObject audioPanel;
    //Button references for showing / hiding audio settings
    [SerializeField] private Button popExitButton;

    //References to sliders
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider uiVolumeSlider;

    //Will be used for clamping incoming slider values, incase slider overload happens
    private float minValue = 0f;
    private float maxValue = 1f;
    private float newVal;
   

    // Start is called before the first frame update
    void Start()
    {
        //Listeners for buttons (to currently just hide pop up)
        popExitButton.onClick.AddListener(delegate {HidePopUp(); });

        //Set up listeners for sliders, providing enum val and changed float value for new slider val through the AudioSliderChanged delegate
        masterVolumeSlider.onValueChanged.AddListener( delegate {AudioSliderChanged(AudioSliders.master, masterVolumeSlider.value); });
        musicVolumeSlider.onValueChanged.AddListener( delegate {AudioSliderChanged(AudioSliders.music, musicVolumeSlider.value); });
        sfxVolumeSlider.onValueChanged.AddListener( delegate {AudioSliderChanged(AudioSliders.sfx, sfxVolumeSlider.value); });
        uiVolumeSlider.onValueChanged.AddListener( delegate {AudioSliderChanged(AudioSliders.ui, uiVolumeSlider.value); });
         
    }
    
    private void HidePopUp()
    {
        audioPanel.SetActive(false);      
    }
    
    private void AudioSliderChanged(AudioSliders sliders, float newVal)
    {
        //Switch case to determine which slider has been moved
        switch(sliders)
        {
        
        case AudioSliders.master:
        newVal = ClampSliderValue(masterVolumeSlider.value);
        FmodRouting.ChangeBusVolume(FmodRouting.masterBus, newVal);
        break;

        case AudioSliders.music:
        newVal = ClampSliderValue(musicVolumeSlider.value);
        FmodRouting.ChangeBusVolume(FmodRouting.musicBus, newVal);
        break;

        case AudioSliders.sfx:
        newVal = ClampSliderValue(sfxVolumeSlider.value);
        FmodRouting.ChangeBusVolume(FmodRouting.sfxBus, newVal);
        break;

        case AudioSliders.ui:
        newVal = ClampSliderValue(uiVolumeSlider.value);
        FmodRouting.ChangeBusVolume(FmodRouting.uiBus, newVal);
        break;


        }


    }

    private float ClampSliderValue(float val)
    {
        //Clamp value, even though slider ranges are set this is to remove the possibility of audio glitches with slider values
        val = Mathf.Clamp(val, minValue, maxValue);

       return val;
    }
}

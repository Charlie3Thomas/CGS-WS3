using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Enum for slider types
public enum AudioSliders {master, music, sfx, ui}

public class AudioUIManager : MonoBehaviour
{
    //Reference to the panel containing all the audio settings UI
    [SerializeField] private GameObject audioPanel;

    //Button references for showing / hiding audio settings
    [SerializeField] private Button popExitButton;
    [SerializeField] private Button popUpShowButton;

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
        popUpShowButton.onClick.AddListener(delegate {ShowPopUp(); });

        //Set up listeners for sliders, providing enum val and changed float value for new slider val through the AudioSliderChanged delegate
        masterVolumeSlider.onValueChanged.AddListener( delegate {AudioSliderChanged(AudioSliders.master, masterVolumeSlider.value); });
        musicVolumeSlider.onValueChanged.AddListener( delegate {AudioSliderChanged(AudioSliders.music, musicVolumeSlider.value); });
        sfxVolumeSlider.onValueChanged.AddListener( delegate {AudioSliderChanged(AudioSliders.sfx, sfxVolumeSlider.value); });
        uiVolumeSlider.onValueChanged.AddListener( delegate {AudioSliderChanged(AudioSliders.ui, uiVolumeSlider.value); });

        LoadPlayerPrefAudioSettings();
         
    }
    
    private void ShowPopUp()
    {
        audioPanel.SetActive(true);
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
           PlayerPrefs.SetFloat("MasterBusVol", newVal);
           break;

           case AudioSliders.music:
           newVal = ClampSliderValue(musicVolumeSlider.value);
           FmodRouting.ChangeBusVolume(FmodRouting.musicBus, newVal);
           PlayerPrefs.SetFloat("MusicBusVol", newVal);
           break;

           case AudioSliders.sfx:
           newVal = ClampSliderValue(sfxVolumeSlider.value);
           FmodRouting.ChangeBusVolume(FmodRouting.sfxBus, newVal);
           PlayerPrefs.SetFloat("SFXBusVol", newVal);
           break;

           case AudioSliders.ui:
           newVal = ClampSliderValue(uiVolumeSlider.value);
           FmodRouting.ChangeBusVolume(FmodRouting.uiBus, newVal);
           PlayerPrefs.SetFloat("UIBusVol", newVal);
           break;

           PlayerPrefs.Save();


        }


    }

    private void LoadPlayerPrefAudioSettings()
    {
        //Load slider values from player prefs and set fmod bus values after they are set
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterBusVol");
        FmodRouting.ChangeBusVolume(FmodRouting.masterBus, masterVolumeSlider.value);

        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicBusVol");
        FmodRouting.ChangeBusVolume(FmodRouting.musicBus, musicVolumeSlider.value);

        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXBusVol");
        FmodRouting.ChangeBusVolume(FmodRouting.sfxBus, sfxVolumeSlider.value);

        uiVolumeSlider.value = PlayerPrefs.GetFloat("UIBusVol");
        FmodRouting.ChangeBusVolume(FmodRouting.uiBus, uiVolumeSlider.value);


    }

    private float ClampSliderValue(float val)
    {
        //Clamp value, even though slider ranges are set this is to remove the possibility of audio glitches with slider values
        val = Mathf.Clamp(val, minValue, maxValue);

       return val;
    }
}

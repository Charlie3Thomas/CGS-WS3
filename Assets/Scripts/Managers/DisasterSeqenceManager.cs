using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CT;
using CT.Lookup;


public class DisasterSeqenceManager : MonoBehaviour
{
    public static DisasterSeqenceManager Instance => m_instance;
    private static DisasterSeqenceManager m_instance;
    
    [SerializeField] private GameObject warningObject;

    public bool isDisasterActive { get; private set; } = false;
   
    // Start is called before the first frame update
    void Start()
    {
        m_instance = this;

        warningObject.SetActive(false);
    }

    public void StartDisasterWarningSequence()
    {
        isDisasterActive = true;
        //Invoke repeating for text flash
        warningObject.SetActive(true);

        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.warningSFXEvent, null);
        if (GameManager._INSTANCE.GetDisasterIntensityAtTurn(GameManager._INSTANCE.current_turn) != -1f && GameManager._INSTANCE.CheckDisasterInTurn() != CTDisasters.None) //Check charlies no disaster -1 val to not set param 
        {
            FmodParameters.SetGlobalParamByName("Intensity", GameManager._INSTANCE.GetDisasterIntensityAtTurn(GameManager._INSTANCE.current_turn));
            FmodParameters.SetParamByLabelName(AudioManager.Instance.musicInstance, "Play", "Play");
        }
        InvokeRepeating("FlashTextTrigger", 0f, 0.3f);
        StartCoroutine("StartSequenceTimer");
    }

    public void StartDisasterSequence()
    {

    }

    public void StartDisasterEndSquence()
    {

    }

    ///////////// Warning Sequence ////////////////
    void FlashTextTrigger()
    {
        StartCoroutine("FlashText");
    }
    IEnumerator FlashText()
    {
        warningObject.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        warningObject.SetActive(true);
    }

    IEnumerator StartSequenceTimer()
    {
          
        yield return new WaitForSeconds(5f);

        CancelInvoke("FlashTextTrigger");
        warningObject.SetActive(false);
        StartCoroutine("DisasterSequenceTimer");
       
    }

    IEnumerator DisasterSequenceTimer()
    {
        DisasterEffectManager.instance.ShowDisasterEffect(GameManager._INSTANCE.CheckDisasterInTurn(), GameManager._INSTANCE.GetDisasterIntensityAtTurn(GameManager._INSTANCE.current_turn));
        AudioManager.Instance.StartDisasterAudio(GameManager._INSTANCE.CheckDisasterInTurn(), GameManager._INSTANCE.GetDisasterIntensityAtTurn(GameManager._INSTANCE.current_turn));
        

        yield return new WaitForSeconds(18f);
        ResetFlag();

    }

    void ResetFlag()
    {
        isDisasterActive = false;
    }


    public bool GetDisasterFlag()
    {
        bool _isDisaster = isDisasterActive;

        return _isDisaster;
    }
}

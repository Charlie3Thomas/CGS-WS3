using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CT;
using CT.Lookup;
using TMPro;


public class DisasterSeqenceManager : MonoBehaviour
{
    public static DisasterSeqenceManager Instance => m_instance;
    private static DisasterSeqenceManager m_instance;
    
    [SerializeField] private GameObject warningObject;
    [SerializeField] private GameObject resolvedObject;
    [SerializeField] private TMP_Text warningText;
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
        
        resolvedObject.SetActive(true);

        ResolveTextFlashTrigger();
        StartCoroutine("ResovleRoutine");

        
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
          
        yield return new WaitForSeconds(7f);

        CancelInvoke("FlashTextTrigger");
        StartCoroutine("DisasterSequenceTimer");
        warningObject.SetActive(false);
    }

    IEnumerator DisasterSequenceTimer()
    {
        warningObject.SetActive(false);//Sometimes doesnt get set to false, just double checking it is set to false to avoid this
        DisasterEffectManager.instance.ShowDisasterEffect(GameManager._INSTANCE.CheckDisasterInTurn(), GameManager._INSTANCE.GetDisasterIntensityAtTurn(GameManager._INSTANCE.current_turn));
        AudioManager.Instance.StartDisasterAudio(GameManager._INSTANCE.CheckDisasterInTurn(), GameManager._INSTANCE.GetDisasterIntensityAtTurn(GameManager._INSTANCE.current_turn));
        

        yield return new WaitForSeconds(18f);
        

    }
    void ResolveTextFlashTrigger()
    {
        InvokeRepeating("ResolveFlash", 0f, 0.5f);
    }
    IEnumerator ResolveFlash()
    {
        resolvedObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        resolvedObject.SetActive(true);
    }
    IEnumerator ResovleRoutine()
    {
        yield return new WaitForSeconds(11f);
        resolvedObject.SetActive(false);
        CancelInvoke("ResolveFlash");

        yield return new WaitForSeconds(0.5f);
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

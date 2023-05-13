using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CT;
using CT.Lookup;
using TMPro;

//Sam: last min implementation: diasters can trigger if spamming, so added this as a form of sequence, to also help with audio transitioning. Bit of a rushed implementation, so sorry...
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
       // warningObject.SetActive(true);

        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.warningSFXEvent, null);
        if (GameManager._INSTANCE.GetDisasterIntensityAtTurn(GameManager._INSTANCE.current_turn) != -1f && GameManager._INSTANCE.CheckDisasterInTurn() != CTDisasters.None) //Check charlies no disaster -1 val to not set param 
        {
            FmodParameters.SetGlobalParamByName("Intensity", GameManager._INSTANCE.GetDisasterIntensityAtTurn(GameManager._INSTANCE.current_turn));
            FmodParameters.SetParamByLabelName(AudioManager.Instance.musicInstance, "Play", "Play");
        }
        InvokeRepeating("FlashTextTrigger", 0f, 0.3f);
        StartCoroutine("StartSequenceTimer");
    }

    public void StartDisasterEndSquence()
    {
        
        resolvedObject.SetActive(true);

        InvokeRepeating("ResolveTextFlashTrigger", 0f, 0.5f);
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
        yield return new WaitForSeconds(3f);
        CancelInvoke("FlashTextTrigger");
        warningObject.SetActive(true);
        yield return new WaitForSeconds(3f);

        warningObject.SetActive(false);
        StartCoroutine("DisasterSequenceTimer");
      
    }

    IEnumerator DisasterSequenceTimer()
    {
        //warningObject.SetActive(false);//Sometimes doesnt get set to false, just double checking it is set to false to avoid this
        DisasterEffectManager.instance.ShowDisasterEffect(GameManager._INSTANCE.CheckDisasterInTurn(), GameManager._INSTANCE.GetDisasterIntensityAtTurn(GameManager._INSTANCE.current_turn));
        AudioManager.Instance.StartDisasterAudio(GameManager._INSTANCE.CheckDisasterInTurn(), GameManager._INSTANCE.GetDisasterIntensityAtTurn(GameManager._INSTANCE.current_turn));
        

        yield return new WaitForSeconds(0.1f);
        

    }
    void ResolveTextFlashTrigger()
    {
        StartCoroutine("ResolveFlash");
    }
    IEnumerator ResolveFlash()
    {
        resolvedObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        resolvedObject.SetActive(true);
    }
    IEnumerator ResovleRoutine()
    {
        yield return new WaitForSeconds(2f); //Let text flash for few seconds
        CancelInvoke("ResolveTextFlashTrigger");
        resolvedObject.SetActive(true);

        yield return new WaitForSeconds(5f); //Make text solid for remaineder of flag time
        resolvedObject.SetActive(false);

        yield return new WaitForSeconds(0.3f); //Reset flag small time after whole sequence has finished 
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

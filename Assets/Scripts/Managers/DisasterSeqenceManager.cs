using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DisasterSeqenceManager : MonoBehaviour
{
    public static DisasterSeqenceManager Instance => m_instance;
    private static DisasterSeqenceManager m_instance;
    
    [SerializeField] private GameObject warningObject;
   
    // Start is called before the first frame update
    void Start()
    {
        m_instance = this;

        warningObject.SetActive(false);
    }

    public void StartDisasterWarningSequence()
    {
        //Invoke repeating for text flash
        warningObject.SetActive(true);
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.warningSFXEvent, null);
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
          
        yield return new WaitForSeconds(3.5f);

        CancelInvoke("FlashTextTrigger");
        warningObject.SetActive(false);
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class ScoreBoardAudio : MonoBehaviour
{ 
    public static ScoreBoardAudio Instance => m_instance;
    private static ScoreBoardAudio m_instance;

    [SerializeField] EventReference scoreShowReference;
    [SerializeField] EventReference scoreRiseReference;

    EventInstance scoreShowInstance;
    EventInstance scoreRiseInstance;

   void Awake()
   {
       m_instance = this;
   }

   public void PlayShowLeaderboardAudio()
   {
       scoreShowInstance =RuntimeManager.CreateInstance(scoreShowReference);
       scoreShowInstance.start();
       scoreShowInstance.release();

   }

   public void PlayScoreRiseAudio()
   {
       scoreRiseInstance =RuntimeManager.CreateInstance(scoreRiseReference);
       scoreRiseInstance.start();
       scoreRiseInstance.release();
   }
   
}

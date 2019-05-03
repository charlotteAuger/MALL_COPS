using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    public AudioSource musicSource;
    public AudioSource[] SFXSources;

    [Header("AudioFiles")]
    public AudioClip SFX_MallAmbiance;
    public AudioClip SFX_ChocFoule;
    public AudioClip SFX_LoopEmbrouille;
    public AudioClip SFX_PlaquageLaunch;
    public AudioClip SFX_PlaquageReceptionEmpty;
    public AudioClip SFX_PlaquageReceptionFull;
    public AudioClip SFX_RunBoucle;
    public AudioClip SFX_TheftAlarm;
    public AudioClip SFX_TimerEndJingle;
    public AudioClip SFX_WinJingle;
    public AudioClip SFX_LooseJingle;
    public AudioClip Music_PlayTheme;
    public AudioClip Music_MenuTheme;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartRunningSFX() {
      //  SFXSources[1].Stop();
        SFXSources[1].PlayOneShot(SFX_RunBoucle);
    }

    public void LaunchPlaquageSFX()
    {
      //  SFXSources[1].Stop();
        SFXSources[1].PlayOneShot(SFX_PlaquageLaunch);
    }

    public void EndPlaquageEmptySFX()
    {
       // SFXSources[1].Stop();
        SFXSources[1].PlayOneShot(SFX_PlaquageReceptionEmpty);
    }

    public void EndPlaquageFullSFX()
    {
        //SFXSources[1].Stop();
        SFXSources[1].PlayOneShot(SFX_PlaquageReceptionFull);
    }

    public void InnocentPlaquageSFX()
    {
        //SFXSources[2].Stop();
        SFXSources[2].PlayOneShot(SFX_ChocFoule);
    }

    public void TheftAlarmSFX()
    {
        //SFXSources[2].Stop();
        SFXSources[2].PlayOneShot(SFX_TheftAlarm);
    }

    public void InnocentAngrySFX()
    {
        SFXSources[0].Stop();
        SFXSources[0].PlayOneShot(SFX_LoopEmbrouille);
    }

    public void EndTimerSFX()
    {
        //SFXSources[1].Stop();
        SFXSources[1].PlayOneShot(SFX_RunBoucle);
    }

    //public void MallAmbianceSFX()
    //{
    //    SFXSources[0].Stop();
    //    SFXSources[0].PlayOneShot(SFX_MallAmbiance);
    //}

    public void LoseJingleSFX()
    {
        musicSource.Stop();
        SFXSources[1].Stop();
        SFXSources[1].PlayOneShot(SFX_LooseJingle);
    }

    public void WinJingleSFX()
    {
        musicSource.Stop();
        SFXSources[1].Stop();
        SFXSources[1].PlayOneShot(SFX_WinJingle);
    }

    public void PlayMenuMusic()
    {
        musicSource.Stop();
        musicSource.PlayOneShot(Music_MenuTheme);
    }

    public void PlayThemeMusic()
    {
        musicSource.Stop();
        musicSource.PlayOneShot(Music_PlayTheme);
    }
}

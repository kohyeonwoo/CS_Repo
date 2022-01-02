using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; //음악 이름
    public AudioClip clip; //음악
}

public class SoundManager : MonoBehaviour
{

    static public SoundManager instacne;

    #region singleton
    private void Awake()
    {
        if (instacne == null)
        {
            instacne = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }
    #endregion singleton

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    public string[] playSoundName;
   
    public Sound[] effectSounds;
    public Sound bgmSounds;

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }

    public void PlaySE(string name)
    {
        for(int i =0; i < effectSounds.Length;i++)
        {
            if(name == effectSounds[i].name)
            {
                for(int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if(!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 기용 AudioSource가 사용중입니다");
                return;
            }
        }
        Debug.Log(name + " 사운드가 SoundManager에 등록되지 않았습니다");
    }

    public void playBgmSound()
    {
        audioSourceBgm.UnPause();
    }

    public void stopBgmSound()
    {
        audioSourceBgm.Pause();
    }

    public void StopAllSE()
    {
        for(int i =0; i < audioSourceEffects.Length; i++)
        {
           //audioSourceEffects[i].Stop();       
           audioSourceEffects[i].volume = 0.0f;       
        }
    }

    public void PlayAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].volume = 1.0f;
        }
    }

    public void StopSE(string name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if(playSoundName[i] == name)
            {
                audioSourceEffects[i].Stop();
                return;
            }           
        }
        Debug.Log("재생 중인" + name + " 사운드가 없습니다.");
    }
}

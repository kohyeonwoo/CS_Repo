using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{

    [SerializeField]
    private int soundCount = 0;

    private void Update()
    {
       DataController.Instance.gameData.EffectSound = soundCount;
    }

    public void TurnOnSoundEffect()
    {
        Debug.Log("현재 효과음 있음");
        SoundManager.instacne.PlayAllSE();
        soundCount = 0;
        DataController.Instance.SaveGameData();
    }

    public void TurnOffSoundEffect()
    {
        Debug.Log("현재 효과음 없음");
        SoundManager.instacne.StopAllSE();
        soundCount = 1;
        DataController.Instance.SaveGameData();
    }
}

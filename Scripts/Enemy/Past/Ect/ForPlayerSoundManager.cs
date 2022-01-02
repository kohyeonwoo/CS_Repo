using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForPlayerSoundManager : MonoBehaviour
{
    [SerializeField]
    private string humanWalkSound;
    [SerializeField]
    private string eagleAttackSound;
    [SerializeField]
    private string whaleAttackSound;
    [SerializeField]
    private string eagleIdleandWalkSound;
    [SerializeField]
    private string whaleWalkSound;
    [SerializeField]
    private string blueMonsterWalkSound;
    [SerializeField]
    private string blueMonsterAttackSound;
    [SerializeField]
    private string playerWhenHitSound;
    [SerializeField]
    private string playerDeadSound;

    public void PlayHumanWalkSound()
    {
        SoundManager.instacne.PlaySE(humanWalkSound);
    }

    public void PlayEagleIdleAndWalkSound()
    {
        SoundManager.instacne.PlaySE(eagleIdleandWalkSound);
    }

    public void PlayWhaleWalkSound()
    {
        SoundManager.instacne.PlaySE(whaleWalkSound);
    }

    public void PlayEagleAttackSound()
    {
        SoundManager.instacne.PlaySE(eagleAttackSound);
    }

    public void PlayWhaleAttackSound()
    {
        SoundManager.instacne.PlaySE(whaleAttackSound);
    }

    public void PlayBMWalkSound()
    {
        SoundManager.instacne.PlaySE(blueMonsterWalkSound);
    }

    public void PlayBMAttackSound()
    {
        SoundManager.instacne.PlaySE(blueMonsterAttackSound);
    }

}

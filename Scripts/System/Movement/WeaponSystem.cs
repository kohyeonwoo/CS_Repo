using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{

    public Animator outfits;

    [Header("플레이어 몬스터")]
    [Header("독수리")]
    public GameObject eagleWeapon;
    [Header("걸음고래")]
    public GameObject whaleWeapon;
    [Header("하늘색괴물")]
    public GameObject[] blueMonsterWeapon;

    ////////////////////////////////////
    [Header("적")]
    [SerializeField]
    private GameObject dogKnightWeapon; //강아지 기사
    [SerializeField]
    private GameObject mimicWeapon; //미믹

    ////////////////////////////////////
    [Header("보스")]
    [SerializeField]
    private GameObject fallenKnightWeapon; //보스 --> 타락 기사
    [SerializeField]
    private GameObject flagDoctorWeapon; //보스 --> 역병 의사
    [SerializeField]
    private GameObject[] titanWeapons; //보스 --> 타이탄

    private void Awake()
    {
        outfits = GetComponent<Animator>();
    }

    /////////////////////
    public void AttackCallBack()
    {
        outfits.SetBool("isAttack", false);
    }

    //////////////////////

    /////////////////////////////////////

    ///독수리 무기 활성화 부분
    public void EagleWeaponActiveTrue()
    {
        eagleWeapon.SetActive(true);
    }

    public void EagleWeaponActiveFalse()
    {
        eagleWeapon.SetActive(false);
    }
    ///독수리 무기 활성화 부분

    ///////////////////////////////////

    ///걸음고래 무기 활성화 부분
    public void WhaleWeaponActiveTrue()
    {
        whaleWeapon.SetActive(true);
    }

    public void WhaleWeaponActiveFalse()
    {
        whaleWeapon.SetActive(false);
    }
    ///걸음고래 무기 활성화 부분

    ///////////////////////////////////

    ///////////////////////////////////

    ///걸음고래 무기 활성화 부분
    public void BlueMonsterWeaponActiveTrue()
    {
        for (int i = 0; i < blueMonsterWeapon.Length; i++)
        {
            blueMonsterWeapon[i].SetActive(true);
        }
    }

    public void BlueMonsterWeaponActiveFalse()
    {
        for (int i = 0; i < blueMonsterWeapon.Length; i++)
        {
            blueMonsterWeapon[i].SetActive(false);
        }
    }
    ///걸음고래 무기 활성화 부분

    ///////////////////////////////////

    ///////////////////////적 부분///////////////////////////

    //일반 몬스터
    // 강아지 기사 무기 관련 부분
    public void DogKnightWeaponActiveTrue()
    {
        dogKnightWeapon.SetActive(true);
    }

    public void DogKnightWeaponActiveFalse()
    {
        dogKnightWeapon.SetActive(false);
    }


    //미믹 무기 관련 부분
    public void MimicWeaponActiveTrue()
    {
        mimicWeapon.SetActive(true);
    }

    public void MimicWeaponActiveFalse()
    {
        mimicWeapon.SetActive(false);
    }

    //보스 몬스터
    //타락 기사 무기 관련 부분
    public void FallenKnightWeaponActiveTrue()
    {
        fallenKnightWeapon.SetActive(true);
    }

    public void FallenKnightWeaponActiveFalse()
    {
        fallenKnightWeapon.SetActive(false);
    }

    // 역병 의사 무기 관련 부분
    public void FlagDoctorWeaponActiveTrue()
    {
        flagDoctorWeapon.SetActive(true);
    }

    public void FlagDoctorWeaponActiveFalse()
    {
        flagDoctorWeapon.SetActive(false);
    }


    // 타이탄 무기 관련 부분 

    public void TitanWeaponsActiveTrue()
    {
        for (int i = 0; i < titanWeapons.Length; i++)
        {
            titanWeapons[i].SetActive(false);
        }
    }

    public void TitanWeaponsActiveFalse()
    {
        for (int i = 0; i < titanWeapons.Length; i++)
        {
            titanWeapons[i].SetActive(true);
        }
    }

    ///////////////////////////////////////////////////////////










































    //public void WeaponActiveTrue()
    //{
    //    eagleWeapon.SetActive(true);
    //    whaleWeapon.SetActive(true);
    //
    //    for (int i = 0; i < blueMonsterWeapon.Length; i++)
    //    {
    //        blueMonsterWeapon[i].SetActive(false);
    //    }
    //}

    //public void WeaponActiveFalse()
    //{
    //    /////////////////////////////////
    //    //for(int i =0; i < whaleWeapon.Length; i++)
    //    // {
    //    //     whaleWeapon[i].SetActive(false);
    //    // }


    //    eagleWeapon.SetActive(false);       
    //    whaleWeapon.SetActive(false);

    //    for (int i = 0; i < blueMonsterWeapon.Length; i++)
    //    {
    //        blueMonsterWeapon[i].SetActive(false);
    //    }
    //}
}

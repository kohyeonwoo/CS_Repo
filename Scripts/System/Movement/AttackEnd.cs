using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnd : MonoBehaviour
{
    public Animator outfits;

    [SerializeField]
    public GameObject whaleWeapon;
    [SerializeField]
    public GameObject[] blueMonsterWeapon;
    [SerializeField]
    public GameObject eagleWeapon;

    private void Awake()
    {
        outfits = GetComponent<Animator>();    
    }

    public void AttackCallBack()
    {
        outfits.SetBool("isAttack", false); //공격 애니메이션 상태를 끝내주는 것
    }

    public void WeaponActiveFalse()
    {
        eagleWeapon.SetActive(false);
        /////////////////////////////////
        //for(int i =0; i < whaleWeapon.Length; i++)
        // {
        //     whaleWeapon[i].SetActive(false);
        // }

        whaleWeapon.SetActive(false);

        for (int i = 0; i < blueMonsterWeapon.Length; i++)
        {
            blueMonsterWeapon[i].SetActive(false);
        }
    }
}

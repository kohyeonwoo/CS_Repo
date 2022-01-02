using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("플레이어 체력 및 피격효과 관련 UI")]
    [SerializeField]
    private TextMeshProUGUI textHp; //플레이어 체력 출력 text
    [SerializeField]
    private Image imageBloodScreen; //플레이어가 공격 받았을 경우 나오는 image\
    [SerializeField]
    private AnimationCurve curveBloodScreen; 

    private void UpdateHPUHD(int previous, int current)
    {
        textHp.text = "HP " + current;
     
        if(previous - current > 0)
        {
            StopCoroutine("OnBloodScreen");
            StartCoroutine("OnBloodScreen");
        }

    }

    private IEnumerator OnBloodScreen()
    {
        float percent = 0;

        while(percent < 1)
        {
            percent += Time.deltaTime;

            Color color = imageBloodScreen.color;
            color.a = Mathf.Lerp(1, 0, curveBloodScreen.Evaluate(percent));
            imageBloodScreen.color = color;

            yield return null;
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class GameData 
{
    //게임 데이터
    public int enemyKillCount = 0; //적을 죽인 숫자
    public int moneyCount = 0; //현재 현금 보유량 숫자
    public int dnaCount = 0; //생체 물질 보유량 숫자 

    //게임 캐릭터 관련
    public int characterCount = 0; //현재 선택한 캐릭터
    public int abilityCount = 0; //현재 선택한 능력

    public bool bDefault = true; //기본 캐릭터
    public bool bRemy = false; //금발 머리 캐릭터
    public bool bTatoo = false; //타투 캐릭터
    public bool bBlackMask = false; //특수 요원

    //게임 변신 능력 관련
    public bool bEagle = false; //독수리
    public bool bWalkWhale = false; //걸음 고래
    public bool bSkyBlueFrog = false; // 하늘색 개구리 괴물
    public bool bDragon = false; //용
    public bool bFrogMan = false; //개구리맨

    //게임 시스템 관련
    public int BGMSound = 0; //뒷배경 소리
    public int EffectSound = 0; //효과음 소리
    public int Vibration = 0; //진동 효과
}

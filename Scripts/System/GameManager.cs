using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("게임 시작 3개의 버튼")]
    //게임 시작 화면 상의 3개의 버튼, 시작 옵션 끝
    [SerializeField]
    private List<GameObject> buttons;
    //////////////////////////

    [Header("게임 UI 모음집")]
    ///////////////////////////////
    [SerializeField]
    private GameObject[] menuCollection; //UI에 관련된 리스트

    [Header("캐릭터 & 능력 이용 표시 모음집")]
    [SerializeField]
    private GameObject[] characterTurnOn;
    [SerializeField]
    private GameObject[] abilityTurnOn;

    [Header("게임 스테이지 모음집")]
    [SerializeField]
    private GameObject[] stageCollection; //Stage 선택시 나올 해당 리스트
    /////////////////////////////

    [Header("게임 시스템")]
    public bool bStart; //게임 내 전투 시작 관련 bool 변수

    ////////////////////// 게임 시간 내 관련
    [Header("게임 내 시간 시스템")]
    [SerializeField]
    private float gameTime = 0;
    [SerializeField]
    private Text text_Timer;
    [SerializeField]
    private Text text_BackToMenu;

    [Header("게임 데이터")]
    //////////////////////// 데이터 관련 파트
    [SerializeField]
    private Text status_PlayerKillCount;
    [SerializeField]
    private Text status_MoneyCount;
    [SerializeField]
    private Text status_DnaCount;

    [SerializeField]
    private int playerKillCount;
    [SerializeField]
    private int playerMoneyCount;
    [SerializeField]
    private int playerDnaCount;
    ////////////////////////

    [Header("플레이어")]
    /////////////////// 플레이어
    [SerializeField]
    private GameObject thePlayer;
    [SerializeField]
    private GameObject playerPosition;

    [Header("백업 카메라")]
    [SerializeField]
    private GameObject backUpCamera;

    public bool isPlayerDead;

    [Header("인게임 내 리소스 모음집")]
    [SerializeField]
    private GameObject[] landscapes; //맵
    [SerializeField]
    private GameObject inGameUi; //인게임 ui

    [Header("보스 몬스터")]
    [SerializeField]
    private GameObject firstStageBoss;

    [Header("일반 몬스터")]
    [SerializeField]
    private GameObject dogKnight;
    [SerializeField]
    private GameObject mimic;

    static public GameManager instacne;

    /////////////////////////
    [Header("캐릭터 개방")]
    [SerializeField]
    public bool bOne; //첫번쨰 디폴트 캐릭터
    [SerializeField]
    public bool bTwo; //두번째 remy 캐릭터
    [SerializeField]
    public bool bThree; //세번째 tatoo 캐릭터
    [SerializeField]
    public bool bFour; //네번째 blackMask 캐릭터

    /////////////////////////
    [Header("능력 개방")]
    [SerializeField]
    private bool bAOne; //독수리
    [SerializeField]
    private bool bATwo; //걸음고래
    [SerializeField]
    private bool bAThree; //하늘색 괴물
    [SerializeField]
    private bool bAFour; //용
    [SerializeField]
    private bool bAFive; //개구리맨

    [SerializeField]
    private bool bEagleOpenTrue;
    [SerializeField]
    private bool bWhaleOpenTrue;

    [Header("능력 개방 시 이미지 관련")]
    [SerializeField]
    private GameObject theFirstAbilityImage;
    [SerializeField]
    private GameObject theFirstAbilityCoverImage;
    [SerializeField]
    private GameObject theSecondAbilityImage;
    [SerializeField]
    private GameObject theSecondAbilityCoverImage;


    //보스의 죽음 관련
    [Header("적 보스 생사 관련")]
    public bool theBossDead = false;

    [Header("적 보스 사망 시 Ragdoll 시스템 작용 관련")]
    [SerializeField]
    private Rigidbody spine;
    [SerializeField]
    private GameObject currentBody;
    [SerializeField]
    private GameObject deadBody;

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

    private void Start()
    {
        buttons = new List<GameObject>();
        bStart = false;
        isPlayerDead = false;
        bOne = true;
        SoundManager.instacne.playBgmSound();
    }

    private void Update()
    {
        if (bStart)
        {
            gameTime += Time.deltaTime;
            text_Timer.text = "Time : " + Mathf.Round(gameTime);
            text_Timer.gameObject.SetActive(true);
            text_BackToMenu.gameObject.SetActive(false);
        }
        if(gameTime >= 20 || isPlayerDead == true)
        {
            bStart = false;
            gameTime = 0;
            text_BackToMenu.gameObject.SetActive(true);
            text_Timer.gameObject.SetActive(false);
            StageEnd();
        }
    }

    private void LateUpdate()
    {
        playerKillCount = DataController.Instance.gameData.enemyKillCount;
        playerMoneyCount = DataController.Instance.gameData.moneyCount;
        playerDnaCount = DataController.Instance.gameData.dnaCount;

        //인간 캐릭터의 활성화 여부에 대한 것들
        DataController.Instance.gameData.bDefault = bOne;
        DataController.Instance.gameData.bRemy = bTwo;
        DataController.Instance.gameData.bTatoo = bThree;
        DataController.Instance.gameData.bBlackMask = bFour;

        //변신 능력의 활성화 여부에 대한 것들
        DataController.Instance.gameData.bEagle = bAOne;
        DataController.Instance.gameData.bWalkWhale = bATwo;
        DataController.Instance.gameData.bSkyBlueFrog = bAThree;
        DataController.Instance.gameData.bDragon = bAFour;
        DataController.Instance.gameData.bFrogMan = bAFive;

        PlayerStatusInform();
    }


    /// Menu UI 옵션 창
    //첫화면--> 장비 혹은 능력치 선택창으로 간다
    public void GoChooseScene()
    {
        menuCollection[0].SetActive(false);
        menuCollection[2].SetActive(true);
    }

    //첫화면 --> 옵션 창으로 간다
    public void GoOptionScene()
    {
        menuCollection[0].SetActive(false);
        menuCollection[1].SetActive(true);
    }

    //장비 혹은 능력치 선택창--> 첫 장면으로 돌아간다
    public void ChooseToStartMenu()
    {
        menuCollection[0].SetActive(true);
        menuCollection[2].SetActive(false);
    }

    //옵션 --> 첫 장면으로 돌아간다
    public void OptionToStartMenu()
    {
        menuCollection[0].SetActive(true);
        menuCollection[1].SetActive(false);
    }

    //선택창 --> 게임 스테이지 선택창으로
    public void GoToStageChoose()
    {
        menuCollection[2].SetActive(false);
        menuCollection[3].SetActive(true);
    }

    //게임 스테이지 선택창 --> 선택창으로
    public void StageChooseToBackToChoose()
    {
        menuCollection[2].SetActive(true);
        menuCollection[3].SetActive(false);
    }

    //선택창 --> 현재 자원 보유창
    public void GoToStatus()
    {
        menuCollection[2].SetActive(false);
        menuCollection[4].SetActive(true);
    }

    //자원 보유 창 --> 선택창
    public void StatusGoToChoose()
    {
        menuCollection[2].SetActive(true);
        menuCollection[4].SetActive(false);
    }

    //선택창 --> 능력 선택창
    public void StatusGoToAbilityCollection()
    {
        menuCollection[7].SetActive(true);
        menuCollection[2].SetActive(false);
    }

    //능력 선택창 --> 선택창
    public void AbilityCollectionGoToStatus()
    {
        menuCollection[2].SetActive(true);
        menuCollection[7].SetActive(false);
    }

    //선택창 --> 캐릭터 선택창
    public void StatusGoToCharacterCollection()
    {
        menuCollection[2].SetActive(false);
        menuCollection[8].SetActive(true);
    }

    //캐릭터 선택창 --> 선택창
    public void CharacterCollectionGoToStatus()
    {
        menuCollection[2].SetActive(true);
        menuCollection[8].SetActive(false);
    }


    //킬 카운트 돈으로 바꾸기
    public void KillCountChangeToMoney()
    {
        playerMoneyCount += playerKillCount;

        DataController.Instance.gameData.moneyCount += playerMoneyCount;
        DataController.Instance.gameData.enemyKillCount -= playerKillCount;  
        DataController.Instance.SaveGameData();
    }

    //돈으로 Dna 사기
    public void BuyDna()
    {
        if(playerMoneyCount >= 10)
        {
            playerDnaCount += 1;

            DataController.Instance.gameData.dnaCount += playerDnaCount;

            playerMoneyCount -= 10;

            DataController.Instance.gameData.moneyCount = playerMoneyCount;
            DataController.Instance.SaveGameData();
        }
       
    }

    //캐릭터 해방
    public void ReleaseCharacter()
    {
        if(playerDnaCount > 0)
        { 
            
        }
    }

    //능력 개방
    public void ReleaseEagleAbility()
    {
        if (playerDnaCount >= 1)
        {
            theFirstAbilityCoverImage.SetActive(false);
            theFirstAbilityImage.SetActive(true);
            Debug.Log("독수리 개방");
        }
        else
        {
            Debug.Log("dna도 없는데 어쩌라고");
        }
    }

    public void ReleaseWhaleAbility()
    {
        if (playerDnaCount >= 1)
        {
            theSecondAbilityCoverImage.SetActive(false);
            theSecondAbilityImage.SetActive(true);
            Debug.Log("걸음고래 개방");
        }
        else
        {
            Debug.Log("dna도 없는데 어쩌라고");
        }
    }


    //해당 캐릭터의 순번 설정
    public void SetOpenCharacterNumber(int setNumber)
    {  
        if(setNumber == 0)
        {
            bOne = true;
            bTwo = false;
            bThree = false;
            bFour = false;
            
            DataController.Instance.gameData.bDefault = bOne;
            DataController.Instance.gameData.bRemy = bTwo;
            DataController.Instance.gameData.bTatoo = bThree;
            DataController.Instance.gameData.bBlackMask = bFour;
            DataController.Instance.gameData.characterCount = setNumber;
            
            characterTurnOn[0].SetActive(true);
            characterTurnOn[1].SetActive(false);
            characterTurnOn[2].SetActive(false);
            characterTurnOn[3].SetActive(false);

            Debug.Log("현재 setNumber : " + setNumber);

            DataController.Instance.SaveGameData();
        }
        else if (setNumber == 1)
        {
            bOne = false;
            bTwo = true;
            bThree = false;
            bFour = false;
            
            DataController.Instance.gameData.bDefault = bOne;
            DataController.Instance.gameData.bRemy = bTwo;
            DataController.Instance.gameData.bTatoo = bThree;
            DataController.Instance.gameData.bBlackMask = bFour;
            DataController.Instance.gameData.characterCount = setNumber;
            
            characterTurnOn[0].SetActive(false);
            characterTurnOn[1].SetActive(true);
            characterTurnOn[2].SetActive(false);
            characterTurnOn[3].SetActive(false);

            Debug.Log("현재 setNumber : " + setNumber);

            DataController.Instance.SaveGameData();
        }
        else if (setNumber == 2)
        {
            bOne = false;
            bTwo = false;
            bThree = true;
            bFour = false;
            
            DataController.Instance.gameData.bDefault = bOne;
            DataController.Instance.gameData.bRemy = bTwo;
            DataController.Instance.gameData.bTatoo = bThree;
            DataController.Instance.gameData.bBlackMask = bFour;
            DataController.Instance.gameData.characterCount = setNumber;
            
            characterTurnOn[0].SetActive(false);
            characterTurnOn[1].SetActive(false);
            characterTurnOn[2].SetActive(true);
            characterTurnOn[3].SetActive(false);

            Debug.Log("현재 setNumber : " + setNumber);

            DataController.Instance.SaveGameData();
        }
        else if (setNumber == 3)
        {
            bOne = false;
            bTwo = false;
            bThree = false;
            bFour = true;
           
            DataController.Instance.gameData.bDefault = bOne;
            DataController.Instance.gameData.bRemy = bTwo;
            DataController.Instance.gameData.bTatoo = bThree;
            DataController.Instance.gameData.bBlackMask = bFour;
            DataController.Instance.gameData.characterCount = setNumber;
           
            characterTurnOn[0].SetActive(false);
            characterTurnOn[1].SetActive(false);
            characterTurnOn[2].SetActive(false);
            characterTurnOn[3].SetActive(true);

            Debug.Log("현재 setNumber : " + setNumber);

            DataController.Instance.SaveGameData();
        }
    }

    //해당 능력의 순번 설정
    //public void SetOpenAbilityNumber(int setANumber)
    //{
    //    if(bAOne == true)
    //    {

    //    }else if (bATwo == true)
    //    {

    //    }
    //    else if (bAThree == true)
    //    {

    //    }
    //    else if (bAFour == true)
    //    {

    //    }
    //    else if (bAFive == true)
    //    {

    //    }
    //} 보류

    ///////////////////////////////////////////////////

    /// 게임 스테이지 선택 --> 미믹
    public void GetInToMimicStage()
    {
        //게임 UI 관련
        menuCollection[6].SetActive(false);
        backUpCamera.SetActive(false);
        inGameUi.SetActive(true);

        //게임 스테이지 관련  
        landscapes[0].SetActive(true); 
        landscapes[1].SetActive(true);
       
        //플레이어 관련
        thePlayer.SetActive(true);
        bStart = true;
        isPlayerDead = false;


        TurnOnCharacterAndAbilities();
    }

    /// 게임 스테이지 선택 --> 기사들
    public void GetInToDogKnightStage()
    {
        //게임 UI 관련
        menuCollection[6].SetActive(false);
        backUpCamera.SetActive(false);
        inGameUi.SetActive(true);

        //게임 스테이지 관련  
        landscapes[0].SetActive(true);
        landscapes[2].SetActive(true);  

        //플레이어 관련
        thePlayer.SetActive(true);
        bStart = true;
        isPlayerDead = false;

        TurnOnCharacterAndAbilities();
    }


    //게임 종료
    public void StageEnd()
    {
        menuCollection[6].SetActive(true);
        inGameUi.SetActive(false);
        backUpCamera.SetActive(true);
        text_BackToMenu.gameObject.SetActive(true);
        //UI관련

        landscapes[0].SetActive(false);
        landscapes[1].SetActive(false);
        landscapes[2].SetActive(false);
        //스테이지 관련
       
        isPlayerDead = false;
        thePlayer.SetActive(false);
        playerPosition.transform.position = Vector3.zero;
        PlayerMovement.instacne.HpRecovery();

        //여기서 해당 캐릭터들의 활성화 유무를 가려준다

        if(bOne == true)
        {
            PlayerMovement.instacne.human.gameObject.SetActive(true);
            PlayerMovement.instacne.eagle.gameObject.SetActive(false);
            PlayerMovement.instacne.whale.gameObject.SetActive(false);
            PlayerMovement.instacne.characterBody = PlayerMovement.instacne.human;
        }else if (bTwo == true)
        {
            PlayerMovement.instacne.remy.gameObject.SetActive(true);
            PlayerMovement.instacne.eagle.gameObject.SetActive(false);
            PlayerMovement.instacne.whale.gameObject.SetActive(false);
            PlayerMovement.instacne.characterBody = PlayerMovement.instacne.remy;
        }
        else if (bThree == true)
        {
            PlayerMovement.instacne.tatoo.gameObject.SetActive(true);
            PlayerMovement.instacne.eagle.gameObject.SetActive(false);
            PlayerMovement.instacne.whale.gameObject.SetActive(false);
            PlayerMovement.instacne.characterBody = PlayerMovement.instacne.tatoo;
        }
        else if (bFour == true)
        {
            PlayerMovement.instacne.blackMask.gameObject.SetActive(true);
            PlayerMovement.instacne.eagle.gameObject.SetActive(false);
            PlayerMovement.instacne.whale.gameObject.SetActive(false);
            PlayerMovement.instacne.characterBody = PlayerMovement.instacne.blackMask;
        }
        //플레이어 관련
    }

    /////////////////////////////////////// 
    //Mimic(Clone) --> 이 복제품들을 find로 지울 수 있을까??

    public void PlayerStatusInform()
    {
        status_PlayerKillCount.text = "Kill Count :    " + playerKillCount;
        status_MoneyCount.text = "Money :    " + playerMoneyCount;
        status_DnaCount.text = "Dna Count :    " + playerDnaCount;
        DataController.Instance.SaveGameData();
    }

    public void TurnOnCharacterAndAbilities()
    {
        if(bOne == true)
        {
            PlayerMovement.instacne.human.gameObject.SetActive(true);
            PlayerMovement.instacne.remy.gameObject.SetActive(false);
            PlayerMovement.instacne.tatoo.gameObject.SetActive(false);
            PlayerMovement.instacne.blackMask.gameObject.SetActive(false);
            PlayerMovement.instacne.characterBody = PlayerMovement.instacne.human;
            PlayerMovement.instacne.playerHp = 15;
        }
        else if(bTwo == true)
        {
            PlayerMovement.instacne.human.gameObject.SetActive(false);
            PlayerMovement.instacne.remy.gameObject.SetActive(true);
            PlayerMovement.instacne.tatoo.gameObject.SetActive(false);
            PlayerMovement.instacne.blackMask.gameObject.SetActive(false);
            PlayerMovement.instacne.characterBody = PlayerMovement.instacne.remy;
            PlayerMovement.instacne.playerHp = 18;
        }
        else if (bThree == true)
        {
            PlayerMovement.instacne.human.gameObject.SetActive(false);
            PlayerMovement.instacne.remy.gameObject.SetActive(false);
            PlayerMovement.instacne.tatoo.gameObject.SetActive(true);
            PlayerMovement.instacne.blackMask.gameObject.SetActive(false);
            PlayerMovement.instacne.characterBody = PlayerMovement.instacne.tatoo;
            PlayerMovement.instacne.playerHp = 20;
        }
        else if (bFour == true)
        {
            PlayerMovement.instacne.human.gameObject.SetActive(false);
            PlayerMovement.instacne.remy.gameObject.SetActive(false);
            PlayerMovement.instacne.tatoo.gameObject.SetActive(false);
            PlayerMovement.instacne.blackMask.gameObject.SetActive(true);
            PlayerMovement.instacne.characterBody = PlayerMovement.instacne.blackMask;
            PlayerMovement.instacne.playerHp = 25;
        }
    }

    //////////////////////////////////////////////
    //렉돌 시스템 관련 파트

    public void ChangeBody()
    {
        CopyCharacterTransformToRagdoll(currentBody.transform, deadBody.transform);

        currentBody.SetActive(false);
        deadBody.SetActive(true);

        spine.AddForce(new Vector3(0.0f, 0.0f, 150.0f), ForceMode.Impulse);
    }

    private void CopyCharacterTransformToRagdoll(Transform origin, Transform ragdoll)
    {
        for (int i = 0; i < origin.childCount; i++)
        {
            if (origin.childCount != 0)
            {
                CopyCharacterTransformToRagdoll(origin.GetChild(i), ragdoll.GetChild(i));
            }
            ragdoll.GetChild(i).localPosition = origin.GetChild(i).localPosition;
            ragdoll.GetChild(i).localRotation = origin.GetChild(i).localRotation;
        }
    }
    //////////////////////////////////////////////


    public void EndGame()
    {
        Application.Quit();
        Debug.Log("앱 종료");
    }
}


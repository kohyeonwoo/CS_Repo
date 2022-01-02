using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("카메라와 그 따라가는 대상")]
    [SerializeField]
    public Transform characterBody;
    [SerializeField]
    private Transform cameraArm;

    [Header("변신상태들")]
    [SerializeField]
    public Transform human;
    [SerializeField]
    public Transform remy;
    [SerializeField]
    public Transform tatoo;
    [SerializeField]
    public Transform blackMask;
    [SerializeField]
    public Transform eagle;
    [SerializeField]
    public Transform whale;
    [SerializeField]
    public Transform blueMonster;
    ////////////////////////////////

    [Header("애니메이터")]
    [SerializeField]
    public Animator animator;
    private Animator eagleAnimator;
    private Animator whaleAnimator;
    private Animator blueMonsterAnimator;
    /////////////////////////////

    //public GameObject whaleWeapon;
    //public GameObject[] blueMonsterWeapon;
    //public GameObject eagleWeapon;

    ////////////////////////////////
    
   
    [Header("사운드 부분")]
    //사운드 부분
    [SerializeField]
    private string changeSound;
    [SerializeField]
    private string eagleSound;
    [SerializeField]
    private string whaleSound;
    [SerializeField]
    private string blueMonsterSound;
    [SerializeField]
    private string playerHitSound;
    [SerializeField]
    private string playerWhenHitByKnightSound;
    //////////////////////////////////////////

    [Header("조작 부분 및 변신 및 시스템 부분")]
    private VirtualJoyStick joyStick;
    [SerializeField]
    private int changeCount;

    public GameObject particle;
    public GameObject attackButton;

    private bool isMove;
    public bool whileOnAttack; //공격중에 움직이지 못하게 하는 것
    private Vector2 moveInput;
    /////////////////////////////////////////

    [Header("플레이어 체력 파트")]
    [SerializeField]
    public float playerHp;
    [SerializeField]
    private Text playerHptxt;

    static public PlayerMovement instacne;

    [Header("피격 시 화면")]
    [SerializeField]
    private Image bloodScreen;

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
        if(GameManager.instacne.bOne == true)
        {
            characterBody = human;
            playerHp = 15;
            animator = characterBody.GetComponent<Animator>();
        }
        if(GameManager.instacne.bTwo == true)
        {
            characterBody = remy;
            playerHp = 18;
            animator = characterBody.GetComponent<Animator>();
        }
        else if (GameManager.instacne.bThree == true)
        {
            characterBody = tatoo;
            playerHp = 20;
            animator = characterBody.GetComponent<Animator>();
        }
        else if (GameManager.instacne.bFour == true)
        {
            characterBody = blackMask;
            playerHp = 25;
            animator = characterBody.GetComponent<Animator>();
        }
   
        whileOnAttack = false;
        
        //playerHp = 15;

        //이거 하나만 해결하고 영상 찍고 입사지원서 내보자
        //문제는 저 characterBody를 여기서 안 바꿔준데서의 문제인 듯 하다
        //여기서 if문 하나씩 써서 조건 맞춰주거나, 및에 함수를 Update에서 호출시켜보자
    }

    private void Update()
    {
       
        animator = characterBody.GetComponent<Animator>();
        characterBody.transform.position = new Vector3(this.transform.position.x,0,this.transform.position.z);
        //blueMonster.transform.position = this.transform.position;

        if (GameManager.instacne.isPlayerDead == false)
            playerHptxt.text = "HP : " + Mathf.Round(playerHp);
        else
        {
            playerHptxt.text = "HP : " + 0;
            playerHp = 0;
        }
            

    }

    public void Move(Vector2 inputDirection)
    {
        moveInput = inputDirection;
        // bool isMove = moveInput.magnitude != 0;
        isMove = moveInput.magnitude != 0;
        animator.SetBool("isMove", isMove);

        if (isMove && !whileOnAttack)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;

            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;

            //  Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            //characterBody.forward = lookforward; --> 이럴 경우 항상 앞을 보기에 뒷걸음 따로 옆걸음 따로 필요
            characterBody.forward = moveDir;
             transform.position += moveDir * Time.deltaTime * 5f;
            
            //this.transform.position = new Vector3(
            //characterBody.transform.position.x, 0, characterBody.transform.position.z);
        }      
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x,
            camAngle.y + mouseDelta.x, camAngle.z);
    }

    public void ChangeOutfit()
    {
        switch (changeCount)
        {      
             case 0:               
            
            if(GameManager.instacne.bOne == true)
            {
               characterBody = eagle;
               eagle.transform.rotation = human.transform.rotation;
               human.gameObject.SetActive(false);
            }
            else if (GameManager.instacne.bTwo == true)
            {
                characterBody = eagle;
                eagle.transform.rotation = remy.transform.rotation;
                remy.gameObject.SetActive(false);
            }
            else if (GameManager.instacne.bThree == true)
            {
                characterBody = eagle;
                eagle.transform.rotation = tatoo.transform.rotation;
                tatoo.gameObject.SetActive(false);
            }
            else if (GameManager.instacne.bFour == true)
            {
                characterBody = eagle;
                eagle.transform.rotation = blackMask.transform.rotation;
                blackMask.gameObject.SetActive(false);
            }

            SoundManager.instacne.PlaySE(changeSound);
            SoundManager.instacne.PlaySE(eagleSound);
                   
            eagle.gameObject.SetActive(true);
            whale.gameObject.SetActive(false);
            
            particle.SetActive(true);
            attackButton.SetActive(true);
            playerHp = 25;
            Debug.Log("독수리로 변신");
            break;
           
            case 1:
             
            characterBody = whale;
            whale.transform.rotation = eagle.transform.rotation;
            SoundManager.instacne.PlaySE(changeSound);
            SoundManager.instacne.PlaySE(whaleSound);
            human.gameObject.SetActive(false);
            eagle.gameObject.SetActive(false);
            whale.gameObject.SetActive(true);
            particle.SetActive(true);
            attackButton.SetActive(true);
            playerHp = 35;
            Debug.Log("고래로 변신");
            break;
            
            default:

                if (GameManager.instacne.bOne == true)
                {
                    characterBody = human;
                    human.transform.rotation = whale.transform.rotation;
                    human.gameObject.SetActive(true);
                }
                else if (GameManager.instacne.bTwo == true)
                {
                    characterBody = remy;
                    remy.transform.rotation = whale.transform.rotation;
                    remy.gameObject.SetActive(true);
                }
                else if (GameManager.instacne.bThree == true)
                {
                    characterBody = tatoo;
                    tatoo.transform.rotation = whale.transform.rotation;
                    tatoo.gameObject.SetActive(true);
                }
                else if (GameManager.instacne.bFour == true)
                {
                    characterBody = blackMask;
                    blackMask.transform.rotation = whale.transform.rotation;
                    blackMask.gameObject.SetActive(true);
                }

            SoundManager.instacne.PlaySE(changeSound);
            eagle.gameObject.SetActive(false);
            whale.gameObject.SetActive(false);
     
            particle.SetActive(true);
            attackButton.SetActive(false);
            playerHp = 15;
            changeCount = -1;
            Debug.Log("인간 형태");
            break;         
        }
    }

    public void PlusChangeCount() //플레이어의 변신 시 해당 변신 카운트에 대한 조정
    {
        ++changeCount;
        Debug.Log("점수 올라간다 : " + changeCount);
    }

    public void Attack() //플레이어의 공격 시 조건 처리
    {
        animator.SetBool("isAttack", true);
        whileOnAttack = true;
        Invoke("AttackEnd", 2.0f);
    }

    private void AttackEnd() //공격이 끝나고, 플레이어를 움직이게 해준다
    {
        whileOnAttack = false;
    }

    private void PlayerDead() //플레이어의 사망 시에 대한 상황
    {
        characterBody.gameObject.SetActive(false);
        particle.SetActive(true);
        GameManager.instacne.isPlayerDead = true;
        changeCount = 0;
        bloodScreen.color = Color.clear;
    }

    public void HpRecovery() //게임을 새롭게 시작했을 경우, 플레이어의 상태 초기화
    {
        changeCount = 0;
        characterBody.gameObject.SetActive(true);
       
        if(GameManager.instacne.bOne == true)
        {
            playerHp = 15;
            characterBody = human;

            GameManager.instacne.bTwo = false;
            GameManager.instacne.bThree = false;
            GameManager.instacne.bFour = false;
        }
        else if (GameManager.instacne.bTwo == true)
        {
            playerHp = 18;
            characterBody = remy;

            GameManager.instacne.bOne = false;
            GameManager.instacne.bThree = false;
            GameManager.instacne.bFour = false;
        }
        else if (GameManager.instacne.bThree == true)
        {
            playerHp = 20;
            characterBody = tatoo;

            GameManager.instacne.bOne = false;
            GameManager.instacne.bTwo = false;
            GameManager.instacne.bFour = false;
        }
        else if (GameManager.instacne.bFour == true)
        {
            playerHp = 25;
            characterBody = blackMask;

            GameManager.instacne.bOne = false;
            GameManager.instacne.bTwo = false;
            GameManager.instacne.bThree = false;
        }
    }

    private void OnTriggerEnter(Collider other) //적들에게 맞았을 경우의 상황 처리
    {
        if(other.tag == "Enemy_DogKnight_Weapon")
        {
            Debug.Log("개가 날 팬다");
            playerHp -= 2;
            StartCoroutine(ShowBloodScreen());
            SoundManager.instacne.PlaySE(playerHitSound);
        }
        else if (other.tag == "Enemy_Knight_Weapon")
        {
            Debug.Log("기사가 날 팬다");
            playerHp -= 5;
            StartCoroutine(ShowBloodScreen());
            SoundManager.instacne.PlaySE(playerWhenHitByKnightSound);
        }
        else if (other.tag == "Mimic_HeadWeapon")
        {
            Debug.Log("미믹이 날 팬다");
            playerHp -= 2;
            StartCoroutine(ShowBloodScreen());
            SoundManager.instacne.PlaySE(playerHitSound);
        }
        if (playerHp <= 0)
        {
            PlayerDead();
        }
    }

    IEnumerator ShowBloodScreen() //피격 시 화면이 빨개진다, 플레이어 타격 시 적용
    {
        bloodScreen.color = new Color(1, 0, 0, UnityEngine.Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);
        bloodScreen.color = Color.clear;
    }
}

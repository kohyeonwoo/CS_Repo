using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TheEnemyState
{ 
  None = -1,
  Idle = 0,
  Wander,
  Pursuit,
  Attack,
}

public class TheEnemyAI : MonoBehaviour
{
    [Header("추적 관련")]
    [SerializeField]
    private float targetRGRange = 5.0f; //타겟 인식 범위
    [SerializeField]
    private float psLimitRange = 7.0f; //추격 범위

    [Header("공격 관련")]
    [SerializeField]
    private float atRange = 3.0f; //공격 범위
    [SerializeField]
    private float attackSpeed = 1.0f; //공격 속도

    public TheEnemyState theES = TheEnemyState.None; //현재 적의 행동 상태
    public float lastAttackTime = 2.0f; //공격 주기 계산용 변수

    private Status _status;
    private NavMeshAgent _nmAgent;

    [SerializeField]
    private Transform _target;
    private Animator _animator;

    [SerializeField]
    private string eagleHitSound;
    [SerializeField]
    private string whaleHitSound;
    [SerializeField]
    private string attackSound;

    private bool isDead;

    private Material _mat;

    [Header("사망효과 용 파티클")]
    [SerializeField]
    private GameObject deadParticle;

    private void Awake()
    {
        _status = GetComponent<Status>();
        _nmAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _nmAgent.updateRotation = false;
    }

    private void OnEnable()
    {
        //적이 활성화 될 때의 적의 상태를 대기 상태로 한다
        ChangeState(TheEnemyState.Idle);
    }

    private void OnDisable()
    {
        //적이 비활성화 될 때 현재 재생중인 상태를 종료하고, 그 상태를 "None"으로 한다
        StopCoroutine(theES.ToString());
        theES = TheEnemyState.None;
    }

    public void ChangeState(TheEnemyState newState)
    {
        //현재 재생 중인 상태와 바꾸려고 하는 상태가 같으면 바꿀 필요가 없기에 return
        if (theES == newState) { return; }

        //이전에 재생중이던 상태 종료
        StopCoroutine(theES.ToString());
        //현재 적의 상태를 newState로 설정
        theES = newState;
        //새로운 상태 재생
        StartCoroutine(theES.ToString());
    }

    private IEnumerator Idle()
    {
        //n초 후에 "배회" 상태로 변경하는 코루틴 실행
        StartCoroutine("AutoChangeFromIdleToWander");

        while (true)
        {
            //"대기" 상태일 때 하는 행동
            //타겟과의 거리에 따라 행동 선택 (배회, 추격, 원거리 공격 혹은 근접 공격)
            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    private IEnumerator AutoChangeFromIdleToWander()
    {
        //1~4초 시간 대기
        int changeTime = Random.Range(1, 5);

        yield return new WaitForSeconds(changeTime);

        //상태를 "배회"로 변경
        ChangeState(TheEnemyState.Wander);
    }

    private IEnumerator Wander()
    {
        float currentTime = 0;
        float maxTime = 10;

        //이동 속도 설정
        _nmAgent.speed = _status.WalkSpeed;

        //목표 위치 설정
        _nmAgent.SetDestination(CalculateWanderPosition());

        //목표 위치로 회전
        Vector3 to = new Vector3(_nmAgent.destination.x, 0, _nmAgent.destination.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);

        _animator.SetBool("isMove", true);
        _animator.SetBool("isAttack", false);
        while (true)
        {
            currentTime += Time.deltaTime;

            //목표위치에 근접하게 도달하거나 너무 오랜시간동안 배회하기 상태에 머물러 있으면
            to = new Vector3(_nmAgent.destination.x, 0, _nmAgent.destination.z);
            from = new Vector3(transform.position.x, 0, transform.position.z);

            if ((to - from).sqrMagnitude < 0.01f || currentTime >= maxTime)
            {
                //상태를 "대기"로 변경
                ChangeState(TheEnemyState.Idle);
            }

            //타겟과의 거리에 따라 행동 선택(배회, 추격, 원거리 공격 혹은 근거리)
            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    private Vector3 CalculateWanderPosition()
    {
        float wanderRadius = 10; //현재 위치를 원점으로 하는 원의 반지름
        int wanderJitter = 0; // 선택된 각도(wanderJitterMin ~ wanderJitterMax)
        int wanderJitterMin = 0; //최소 각도
        int wanderJitterMax = 360; //최대 각도

        //현재 적 캐릭터가 있는 월드의 중심 위치와 크기(구역을 벗어난 행동을 하지 않도록)
        Vector3 rangePosition = Vector3.zero;
        Vector3 rangeScale = Vector3.one * 100.0f;

        //자신의 위치를 중심으로 반지름(wanderRadius) 거리, 선택된 각도(wanderJitter)에 위치한 좌표를 목표지점으로 설정
        wanderJitter = Random.Range(wanderJitterMin, wanderJitterMax);
        Vector3 targetPosition = transform.position + SetAngle(wanderRadius, wanderJitter);

        //생성된 목표위치가 자신의 이동구역을 벗어나지 않게 조절
        targetPosition.x = Mathf.Clamp(targetPosition.x, rangePosition.x - rangeScale.x * 0.5f, rangePosition.x + rangeScale.x * 0.5f);
        targetPosition.y = 0.0f;
        targetPosition.z = Mathf.Clamp(targetPosition.z, rangePosition.z - rangeScale.z * 0.5f, rangePosition.z + rangeScale.z * 0.5f);

        return targetPosition;
    }

    private Vector3 SetAngle(float radius, int angle)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }

    private IEnumerator Pursuit()
    {
        while (true)
        {
            //이동 속도 설정(배회할 때는 걷는 속도로 이동, 추적할 때는 뛰는 속도로 이동)
            _nmAgent.speed = _status.RunSpeed;

            //목표 위치를 현재 플레이어의 위치로 설정
            _nmAgent.SetDestination(_target.position);

            //타겟 방향을 계속 주시하도록 함
            LookRotationToTarget();

            _animator.SetBool("isAttack", false);
            _animator.SetBool("isMove", true);

            //타겟과의 거리에 따라 행동 선택(배회, 추격, 원거리 혹은 근거리 공격)
            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        //공격 시 이동을 멈추도록 설정
        _nmAgent.ResetPath();

        while (true)
        {
            //타겟 방향 주시
            LookRotationToTarget();

            //타겟과의 거리에 따라 행동 선택(배회, 추격, 원거리 혹은 근거리 공격)
            CalculateDistanceToTargetAndSelectState();

            if (Time.time - lastAttackTime > atRange)
            {
                //공격 주기가 되어야 공격할 수 있도록 하기 위해 현재 시간 저장
                lastAttackTime = Time.time;
                _animator.SetBool("isAttack", true);
                _animator.SetBool("isMove", false);
                //발사체 생성
                // GameObject clone = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                // clone.GetComponent<EnemyProjectile>().SetUp(target.position);
            }
            yield return null;
        }
    }

    private void LookRotationToTarget()
    {
        //목표 위치
        Vector3 to = new Vector3(_target.position.x, 0, _target.position.z);
        //내 위치
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        //바로 돌기
        transform.rotation = Quaternion.LookRotation(to - from);
        //서서히 돌기
        // Quaternion rotation = Quaternion.LookRotation(to - from);
        //transform.rotation = Quaternion.Slerp(transform.rotation, RotationDriveMode, 0.01f);
    }

    private void CalculateDistanceToTargetAndSelectState()
    {
        if (_target == null) { return; }

        //플레이어(Target)와 적의 거리 계산 후 거리에 따라 행동 선택
        float distance = Vector3.Distance(_target.position, transform.position);

        if (distance <= atRange)
        {
            ChangeState(TheEnemyState.Attack);
        }
        else if (distance <= targetRGRange)
        {
            ChangeState(TheEnemyState.Pursuit);
        }
        else if (distance >= psLimitRange)
        {
            ChangeState(TheEnemyState.Wander);
        }
    }

    private void OnDrawGizmos()
    {
        //"배회" 상태일 때 이동할 경로 표시
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, _nmAgent.destination - transform.position);

        //목표 인식 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRGRange);

        //추적 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, psLimitRange);

        //공격 범위
        Gizmos.color = new Color(0.39f, 0.04f, 0.04f);
        Gizmos.DrawWireSphere(transform.position, atRange);
    }

    public void TakeDamage(int damage)
    {
        isDead = _status.DecreaseHP(damage);
    }

    private void Erase()
    {
        if (isDead == true)
        {
            this.gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee_Eagle")
        {
            SoundManager.instacne.PlaySE(eagleHitSound);
          
            isDead = true;
        }
        else if (other.tag == "Melee_Whale")
        {
            SoundManager.instacne.PlaySE(whaleHitSound);
        
            isDead = true;
        }

        if (isDead == true)
        {
            Erase();
            deadParticle.SetActive(true);
            deadParticle.transform.position = this.transform.position;
            DataController.Instance.gameData.enemyKillCount += 1;
            DataController.Instance.SaveGameData();
        }
    }

}

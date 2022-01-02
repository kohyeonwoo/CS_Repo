using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum CurrentState { idle, trace, attack, dead};
    public CurrentState curState = CurrentState.idle;

    private Transform _transform;
    private Transform targetTransform;
    private NavMeshAgent nvAgent;
    private Animator _animator;

    public float traceDist = 15.0f; //추격권
    public float attackDist = 3.2f; //공격 사정권

    public bool isDead = false; //생사여부

    [SerializeField]
    private string strikeSound;
    [SerializeField]
    private string eagleHitSound;
    [SerializeField]
    private string whaleHitSound;
    [SerializeField]
    private GameObject enemyWeapon;

    ///////////////////////////////
    ///
    [SerializeField]
    private Rigidbody spine;
    [SerializeField]
    private GameObject currentBody;
    [SerializeField]
    private GameObject deadBody;


    private void Awake()
    {
        enemyWeapon.SetActive(false);
        _transform = this.gameObject.GetComponent<Transform>();
        targetTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        _animator = this.gameObject.GetComponent<Animator>();
      
        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }

    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector3.Distance(targetTransform.position, _transform.position);

            if (dist <= attackDist) { curState = CurrentState.attack; }
            else if (dist <= traceDist) { curState = CurrentState.trace; }
            else { curState = CurrentState.idle; }
        }

        if (isDead) { ChangeBody(); }
    }

    IEnumerator CheckStateForAction()
    {
        while(!isDead)
        {
            switch (curState)
            {
                case CurrentState.idle:
                    nvAgent.Stop();
                    _animator.SetBool("isMove", false);
                    enemyWeapon.SetActive(false);
                    break;
                case CurrentState.trace:
                    nvAgent.destination = targetTransform.position;
                    nvAgent.Resume();
                    _animator.SetBool("isMove", true);
                    _animator.SetBool("isAttack", false);
                    enemyWeapon.SetActive(false);
                    break;
                case CurrentState.attack:
                    nvAgent.Stop();
                    this._transform.LookAt(targetTransform.position);
                    _animator.SetBool("isMove", false);
                    _animator.SetBool("isAttack", true);
                    enemyWeapon.SetActive(true);
                    break;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
      if(other.tag == "Melee_Eagle")
      {
            SoundManager.instacne.PlaySE(eagleHitSound);
            isDead = true;
            Debug.Log("독수리 타격");
      }
      else if (other.tag == "Melee_Whale")
      {
          SoundManager.instacne.PlaySE(whaleHitSound);
            isDead = true;
            Debug.Log("고래 타격");
       }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackDist);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, traceDist);
    }

    public void ChangeBody()
    {
        CopyCharacterTransformToRagdoll(currentBody.transform, deadBody.transform);

        currentBody.SetActive(false);
        deadBody.SetActive(true);

        spine.AddForce(new Vector3(0.0f, 0.0f, 300.0f), ForceMode.Impulse);

        Invoke("ClearDeadBody", 5.0f);
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

    private void ClearDeadBody()
    {
        Destroy(deadBody);
        Destroy(currentBody);
    }

}

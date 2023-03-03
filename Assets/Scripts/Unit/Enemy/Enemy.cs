using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Unit
{
    public enum Type
    {
        Mousey,
        Doozy
    }

    public enum State
    {
        IDLE,
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }

    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;
    private Transform enemyTransform;
    private Transform playerTransform;

    [Header("General")]
    [SerializeField]
    private Type type;
    [SerializeField]
    private State state;
    [SerializeField]
    private EnemyData enemyData;
    [SerializeField]
    private ParticleSystem hitEffect;
    [SerializeField]
    private AudioClip hitClip;
    [SerializeField]
    private AudioClip deathClip;
    [SerializeField]
    [Range(0f, 200f)]
    private float damage = 20.0f;
    [SerializeField]
    private float traceDist;
    [SerializeField]
    private float attackDist;

    [Header("Mousey")]
    [SerializeField]
    private Sword sword;
    [SerializeField]
    private float timeBetAttack = 0.5f;
    private float lastAttackTime;
    [Header("AttackRange")]
    [SerializeField]
    private bool showAttackGizmos;
    [SerializeField]
    private float attackRange;
    [SerializeField, Range(0f, 360f)]
    private float attackAngle;

    [Header("Archer")]
    [SerializeField]
    private Transform firePosition;
    [SerializeField]
    private GameObject Arrow;
    [SerializeField]
    private float nextFire = 0.0f;
    [SerializeField]
    private float fireRate = 1.0f;

    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashDie = Animator.StringToHash("Die");

    public void SetUP(EnemyData enemyData)
    {
        initHP = enemyData.hp;
        hp = enemyData.hp;
        damage = enemyData.damage;
        agent.speed = enemyData.speed;
        traceDist = enemyData.traceDist;
        attackDist = enemyData.attackDist;
    }

    private void Awake()
    {
        enemyTransform = GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(UpdateStateCoroutine());
        StartCoroutine(ChangeStateCoroutine());
    }

    private void Update()
    {
        if (agent.remainingDistance >= 2.0f)
        {
            Vector3 direction = agent.desiredVelocity;

            if (direction.sqrMagnitude >= 0.1f * 0.1f)
            {
                // 회전 각도 계산
                Quaternion rot = Quaternion.LookRotation(direction);
                // 선형보간 함수 -> 자연스러운 회전
                enemyTransform.rotation = Quaternion.Slerp(
                    enemyTransform.rotation, rot, Time.deltaTime * 10.0f);
            }
        }
    }

    private IEnumerator UpdateStateCoroutine()
    {
        while (!dead)
        {
            yield return new WaitForSeconds(0.3f);

            if (state == State.DIE)
                yield break;

            float dist = Vector3.Distance(playerTransform.position, enemyTransform.position);

            if (dist <= attackDist)
            {
                state = State.ATTACK;
            }

            else if (dist <= traceDist)
            {
                state = State.TRACE;
            }

            else
            {
                state = State.PATROL;
            }
        }
    }

    private IEnumerator ChangeStateCoroutine()
    {
        while (!dead)
        {
            switch (type)
            {
                case Type.Mousey:
                    switch (state)
                    {
                        case State.IDLE:
                            IdleState();
                            break;

                        case State.TRACE:
                            TraceState();
                            break;

                        case State.ATTACK:
                            Attack();
                            break;

                        case State.DIE:
                            Die();
                            break;
                    }
                    yield return new WaitForSeconds(0.3f);
                    break;
                case Type.Doozy:
                    switch (state)
                    {
                        case State.IDLE:
                            IdleState();
                            break;

                        case State.TRACE:
                            TraceState();
                            break;

                        case State.ATTACK:
                            Shot();
                            break;

                        case State.DIE:
                            Die();
                            break;
                    }
                    yield return new WaitForSeconds(0.3f);
                    break;
            }
        }
    }

    private void IdleState()
    {
        agent.isStopped = true;
        animator.SetBool(hashTrace, false);
    }

    private void TraceState()
    {
        agent.SetDestination(playerTransform.position);
        agent.isStopped = false;
        animator.SetBool(hashTrace, true);
        animator.SetBool(hashAttack, false);
    }

    #region Mousey
    private void Attack()
    {
        agent.isStopped = true;
        animator.SetTrigger(hashAttack);
    }

    public void OnAttackStart()
    {
        Debug.Log("공격 시작!");
        sword.EnableCollider();
    }

    public void OnAttackHit()
    {
        Debug.Log("휘두름!");

        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 dirToTarget =
                (colliders[i].transform.position - transform.position).normalized;
            Vector3 rightDir =
                AngleToDir(transform.eulerAngles.y + attackAngle * 0.5f);

            if (Vector3.Dot(transform.forward, dirToTarget) >
                Vector3.Dot(transform.forward, rightDir))
            {
                if (colliders[i].gameObject.name == "Player")
                {
                    Unit target = colliders[i].GetComponent<Unit>();

                    if (target != null)
                    {
                        Vector3 hitPoint =
                            colliders[i].ClosestPoint(transform.position);
                        Vector3 hitNormal =
                            transform.position - colliders[i].transform.position;

                        target.OnDamage(enemyData.damage, hitPoint, hitNormal);
                    }
                }
            }
        }
    }

    public void OnAttackEnd()
    {
        Debug.Log("공격 끝!");
        sword.DisableCollider();
    }
    #endregion

    #region Doozy
    private void Shot()
    {
        agent.isStopped = true;

        if (Time.time >= nextFire)
        {
            Fire();

            nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
        }
    }

    private void Fire()
    {
        animator.SetTrigger(hashAttack);

        GameObject arrow = Instantiate(Arrow, firePosition.position, firePosition.rotation);
        Destroy(arrow, 3.0f);
    }
    #endregion

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            animator.SetTrigger(hashHit);

            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            audioSource.PlayOneShot(hitClip);
        }

        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        base.Die();

        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        agent.isStopped = true;
        agent.enabled = false;

        animator.SetTrigger(hashDie);
        audioSource.PlayOneShot(deathClip);
        Debug.Log("죽음!");
    }

    private void OnTriggerStay(Collider other)
    {
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            Unit target = other.GetComponent<Unit>();

            if (target != null)
            {
                lastAttackTime = Time.time;

                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;

                target.OnDamage(damage, hitPoint, hitNormal);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (showAttackGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Vector3 rightDir = AngleToDir(transform.eulerAngles.y + attackAngle * 0.5f);
            Vector3 leftDir = AngleToDir(transform.eulerAngles.y - attackAngle * 0.5f);
            Debug.DrawRay(transform.position, rightDir * attackRange, Color.blue);
            Debug.DrawRay(transform.position, leftDir * attackRange, Color.blue);
        }
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }
}

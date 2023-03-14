using System.Collections;
using System.Collections.Generic;
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
        Idle,
        Trace,
        Attack,
        Die
    }

    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;
    private Transform enemyTransform;
    private Transform playerTransform;
    private ProjectileAttack projectileAttack;

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
    private float traceDist;
    [SerializeField]
    private float attackDist;
    private float damage;

    [Header("Arrow")]
    private float nextFire = 0.0f;
    private float fireRate = 1.0f;

    private readonly List<Animation> animList = new List<Animation>();

    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashDie = Animator.StringToHash("Die");

    private void Awake()
    {
        enemyTransform = GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetUP(EnemyData enemyData)
    {
        initHP = enemyData.hp;
        hp = enemyData.hp;
        damage = enemyData.damage;
        agent.speed = enemyData.speed;
        traceDist = enemyData.traceDist;
        attackDist = enemyData.attackDist;
    }

    private void Start()
    {
        StartCoroutine(UpdateStateCoroutine());
        StartCoroutine(ChangeStateCoroutine());
    }

    private void Update()
    {
        if (!dead)
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
    }

    private IEnumerator UpdateStateCoroutine()
    {
        while (!dead)
        {
            yield return new WaitForSeconds(0.3f);

            if (state == State.Die)
                yield break;

            var offset = playerTransform.position - transform.position;
            float dist = offset.sqrMagnitude;

            if (dist <= attackDist * attackDist)
            {
                state = State.Attack;
            }

            else if (dist <= traceDist * traceDist)
            {
                state = State.Trace;
            }

            else
            {
                state = State.Idle;
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
                        case State.Idle:
                            IdleState();
                            break;

                        case State.Trace:
                            TraceState();
                            break;

                        case State.Attack:
                            Attack();
                            break;
                    }
                    yield return new WaitForSeconds(0.3f);
                    break;
                case Type.Doozy:
                    switch (state)
                    {
                        case State.Idle:
                            IdleState();
                            break;

                        case State.Trace:
                            TraceState();
                            break;

                        case State.Attack:
                            Shot();
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
        animator.SetBool(hashAttack, true);
    }
    #endregion

    #region Doozy
    private void Shot()
    {
        agent.isStopped = true;

        if (Time.time >= nextFire)
        {
            Fire();
            projectileAttack.CreateArrow();

            nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
        }
    }

    private void Fire()
    {
        animator.SetBool(hashAttack, true);
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

        animator.SetTrigger(hashDie);

        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        agent.isStopped = true;
        agent.enabled = false;

        audioSource.PlayOneShot(deathClip);
        Debug.Log("죽음!");
    }
}

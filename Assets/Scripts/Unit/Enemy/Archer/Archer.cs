using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Archer : Unit
{
    public enum ArcherState
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
    public ArcherState state;

    [Header("Archer")]
    [SerializeField]
    private EnemyData enemyData;
    [SerializeField]
    private Transform firePosition;
    [SerializeField]
    private GameObject Arrow;
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
    private float traceDist = 30.0f;
    [SerializeField]
    private float attackDist = 20.0f;
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

            if (state == ArcherState.DIE)
                yield break;

            float dist = Vector3.Distance(playerTransform.position, enemyTransform.position);

            if (dist <= attackDist)
            {
                state = ArcherState.ATTACK;
            }

            else if (dist <= traceDist)
            {
                state = ArcherState.TRACE;
            }

            else
            {
                state = ArcherState.PATROL;
            }
        }
    }

    private IEnumerator ChangeStateCoroutine()
    {
        while (!dead)
        {
            switch (state)
            {
                case ArcherState.IDLE:
                    agent.isStopped = false;
                    animator.SetBool(hashTrace, true);
                    break;

                case ArcherState.TRACE:
                    agent.SetDestination(playerTransform.position);
                    agent.isStopped = false;
                    animator.SetBool(hashTrace, true);
                    animator.SetBool(hashAttack, false);
                    break;

                case ArcherState.ATTACK:
                    Shot();
                    break;

                case ArcherState.DIE:
                    Die();

                    yield return new WaitForSeconds(3.0f);

                    hp = 100;
                    dead = false;
                    state = ArcherState.IDLE;
                    gameObject.SetActive(false);
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
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

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            animator.SetTrigger(hashHit);
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
    }

}

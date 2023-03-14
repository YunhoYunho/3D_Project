using UnityEngine;

public class MeeleAttack : MonoBehaviour
{
    [SerializeField]
    private Enemy enemy;
    [SerializeField]
    private EnemyData enemyData;
    [SerializeField]
    private Sword sword;
    [SerializeField]
    [Range(0f, 200f)]
    private float damage = 20.0f;
    public float timeBetAttack = 0.5f;
    public float lastAttackTime;

    [Space]

    [Header("AttackRange")]
    [SerializeField]
    private bool showAttackGizmos;
    [SerializeField]
    private float attackRange;
    [SerializeField, Range(0f, 360f)]
    private float attackAngle;

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

    private void OnTriggerStay(Collider other)
    {
        if (!enemy.dead && Time.time >= lastAttackTime + timeBetAttack)
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
}

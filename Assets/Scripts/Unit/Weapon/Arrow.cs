using UnityEngine;

public class Arrow : MonoBehaviour
{
    private new Rigidbody rigidbody;

    [SerializeField]
    private EnemyData enemyData;
    [SerializeField]
    private float force = 1500.0f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward * force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Wall")
        {
            Destroy(gameObject, 10f);
        }

        if (other.gameObject.name == "Player")
        {
            Unit target = other.GetComponent<Unit>();

            if (target != null)
            {
                Vector3 hitPoint =
                    other.ClosestPoint(transform.position);
                Vector3 hitNormal =
                    transform.position - other.transform.position;

                target.OnDamage(enemyData.damage, hitPoint, hitNormal);
            }
        }
    }
}

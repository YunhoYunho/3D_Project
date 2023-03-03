using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float hp = 100f;
    public float damage = 20.0f;
    public float speed = 2.0f;
    public float traceDist = 30.0f;
    public float attackDist = 20.0f;
}

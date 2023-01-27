using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Item/HealthPack")]
public class HealthPackData : ScriptableObject
{
    public new string name;
    [TextArea(1, 2)]
    public string description;
    public GameObject prefab;
    public Sprite icon;

    [SerializeField]
    [Range(0, 50)]
    public int healthPoint;
}

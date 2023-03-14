using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Item/AmmoPack")]
public class AmmoPackData : ScriptableObject
{
    public new string name;
    [TextArea(1, 2)]
    public string description;
    public GameObject prefab;
    public Sprite icon;

    [SerializeField]
    [Range(0, 30)]
    public int ammoCount;
}

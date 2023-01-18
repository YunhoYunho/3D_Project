using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float hp = 200f;
    public float speed = 2f;
}

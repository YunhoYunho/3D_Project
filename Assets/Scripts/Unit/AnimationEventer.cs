using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventer : MonoBehaviour
{
    public UnityEvent OnAttackStart;
    public UnityEvent OnAttackHit;
    public UnityEvent OnAttackEnd;

    public void AttackStart()
    {
        OnAttackStart?.Invoke();
    }

    public void AttackHit()
    {
        OnAttackHit?.Invoke();
    }

    public void AttackEnd()
    {
        OnAttackEnd?.Invoke();
    }
}

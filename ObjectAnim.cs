using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectAnim 
{
    [SerializeField] private float minToMaxDuration;
    [SerializeField] private float maxValue;
    [SerializeField] private AnimationCurve curve;

    [HideInInspector] public float elapsed;
    [HideInInspector] public int dir = 1;


    public float GetCurrentValue()
    {
        return Mathf.Sign(elapsed) * curve.Evaluate(Mathf.Abs(elapsed)) * maxValue;
    }

    public void AddElapsed (float t) 
    {
        elapsed += dir * t / minToMaxDuration;
    }
}

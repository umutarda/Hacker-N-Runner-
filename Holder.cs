using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{

    [SerializeField] ObjectAnim rotationAnim;
    [SerializeField] ObjectAnim ropeAnim;

    [SerializeField] Transform hook;
    [SerializeField] LineRenderer rope;
    [SerializeField] float hookColliderRadius;

    HolderStates holderStates;
    HolderState currentState;

    HackObject currentHackObj;

    void Start()
    {
        holderStates = new HolderStates();
        holderStates.AddState("rotator", new Rotator());
        holderStates.AddState("catch", new Catch());
        holderStates.AddState("hack", new Hack());

        currentState = GetState("rotator");
        rope = FindObjectOfType<LineRenderer>();
        rope.SetPosition(0, Vector3.zero);
    }

    void Update()
    {
        currentState = currentState.Perform(this);
    }

    public HolderState GetState (string name) 
    {
        return holderStates.GetState(name); 
    }

    public ObjectAnim GetRotationAnim()
    {
        return rotationAnim;
    }

    public ObjectAnim GetRopeAnim()
    {
        return ropeAnim;
    }

    public Transform GetHook() 
    {
        return hook;
    }

    public LineRenderer GetRope() 
    {
        return rope;
    }

    public HackObject GetHackObject() 
    {
        return currentHackObj;
    }

    public void SetHackObject (HackObject hackObject) 
    {
        currentHackObj = hackObject;
    }

    public float GetRopeColliderRadius () 
    { 
        return hookColliderRadius; 
    }

}


public struct HolderStates
{
    Dictionary<string, HolderState> states;
    public void AddState(string name, HolderState state) 
    {
        if (states == null)
            states = new Dictionary<string, HolderState>();

        states.Add(name, state);
    }

    public HolderState GetState(string name) 
    {
        return states[name];
    }
}

public abstract class HolderState
{
    public abstract HolderState Perform(Holder holder);
}

public class Rotator : HolderState
{
    ObjectAnim rotationAnim;
    public override HolderState Perform(Holder holder)
    {
        if (rotationAnim == null)
            rotationAnim = holder.GetRotationAnim();

        if (Input.GetButtonDown("Fire1"))
            return holder.GetState("catch");

        rotationAnim.AddElapsed(Time.deltaTime);
        holder.transform.eulerAngles = Vector3.up * rotationAnim.GetCurrentValue();

        if (Mathf.Clamp(rotationAnim.elapsed, -1, 1) != rotationAnim.elapsed)
        {
            rotationAnim.dir *= -1;
            rotationAnim.elapsed = Mathf.Clamp(rotationAnim.elapsed, -1, 1);
        }


        return this;
    }

}

public class Catch : HolderState
{
    ObjectAnim ropeAnim;

    Transform hook;
    Vector3 hookOriginal;
    RaycastHit hitObject;
    Ray ray;

    public override HolderState Perform(Holder holder)
    {
        if (ropeAnim == null)
            ropeAnim = holder.GetRopeAnim();

        if (hook == null)
        {
            hook = holder.GetHook();
            hookOriginal = hook.position;
            ray = new Ray(hookOriginal, hook.forward);
        }

        ropeAnim.AddElapsed(Time.deltaTime);
        hook.position = hookOriginal + hook.forward * ropeAnim.GetCurrentValue();
        holder.GetRope().SetPosition(1, holder.GetRope().transform.InverseTransformPoint(hook.position));

        
        if (Physics.Raycast(ray, out hitObject, ropeAnim.GetCurrentValue() + holder.GetRopeColliderRadius()))
        {
            ropeAnim.dir = -1;

            holder.SetHackObject(hitObject.transform.GetComponent<HackObject>());
            if ( holder.GetHackObject() != null) 
            {
                return holder.GetState("hack");
            }
            
        }

        if (ropeAnim.elapsed >= 1)
        {
            ropeAnim.dir = -1;
        }

        if (ropeAnim.dir == -1 && ropeAnim.elapsed <= 0)
        {
            hook.position = hookOriginal;
            holder.GetRope().SetPosition(1, Vector3.zero);

            Reset();
            return holder.GetState("rotator");
        }

        return this;
    }

    public void Reset()
    {
        hook = null;

        if (ropeAnim != null) 
        {
            ropeAnim.elapsed = 0;
            ropeAnim.dir = 1;
        }
    }

}

public class Hack : HolderState 
{
    IEnumerator hackStage;
    public override HolderState Perform(Holder holder) 
    {
        if (hackStage == null) 
        {
            hackStage = holder.GetHackObject().HackStage();
            holder.StartCoroutine(hackStage);
        }

        if (hackStage.Current != null) 
        {
            if ((int)hackStage.Current == 0)
            {
                hackStage = null;
                return holder.GetState("catch");
            }
        }
            
        return this;
    }
}




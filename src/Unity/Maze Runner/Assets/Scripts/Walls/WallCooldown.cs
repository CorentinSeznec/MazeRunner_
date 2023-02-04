using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCooldown : MonoBehaviour
{
    protected float lastActivation;
    protected float lastCoolDown;

    void Start()
    {
        lastActivation = Time.fixedTime;
    }

    public bool IsOnCoolDown()
    {
        return (lastActivation + lastCoolDown >= Time.fixedTime);
    }

    public bool IsNotOnCoolDown()
    {
        return (lastActivation + lastCoolDown < Time.fixedTime);
    }

    virtual public void SetOnCooldown(float coolDown)
    {
        lastActivation = Time.fixedTime;
        lastCoolDown = coolDown;
    }
}

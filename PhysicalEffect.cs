using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalEffect : MonoBehaviour 
{
    protected string m_name;
    public string Name { get { return m_name; } set { m_name = value; } }

    protected int m_amount = 3;
    public int Amount { get { return m_amount; } set { m_amount = value; } }

    protected int m_duration = 3;
    public int Duration { get { return m_duration; } set { m_duration = value; } }

    protected bool m_isSkipTurn = false;
    public bool IsSkipTurn { get { return m_isSkipTurn; } set { m_isSkipTurn = value; } }

    public Actor owner;
    public GameObject iconPrefab;
    public GameObject effectPrefab;

    public virtual void SetEffect(int power, int duration, Actor target)
    {
        m_amount = power;
        m_duration = duration;
        owner = target;
    }

    public virtual void ApplyEffect()
    {
        if(owner != null)
        {
            // Show Effects
        }
        Debug.Log("No Owner Found =========");
    }
}

public class Bleed : PhysicalEffect
{

    public override void SetEffect(int power, int duration, Actor target)
    {
        base.SetEffect(power, duration, target);
        m_name = "Bleed";
        m_isSkipTurn = false;
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();

        owner.Health -= this.m_amount;
        m_duration--;

        if(m_duration == 0)
        {
            owner.physicalEffects.Remove(this);
        }
    }
}

public class Infect : PhysicalEffect
{

}

public class Stun : PhysicalEffect
{

}
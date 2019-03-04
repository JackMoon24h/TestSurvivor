using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public enum PhysicalEffectType
//{
//    Damage,
//    Critical,
//    Heal,
//    Bleed,
//    BleedResist,
//    Infect,
//    InfectResist,
//    Stun,
//    StunResist,
//    Resist,
//    Buff,
//    Dodge,
//    Death,
//    MentalDMG,
//    MentalHeal,
//    Refusal
//}

public enum PhysicalEffectType
{
    Bleed,
    Infect,
    Stun,
    Buff,
    Move
}

public class PhysicalEffect : MonoBehaviour 
{
    protected string m_name;
    public string Name { get { return m_name; } }

    protected int m_amount = 3;
    public int Amount { get { return m_amount; } set { m_amount = value; } }

    protected int m_duration = 3;
    public int Duration { get { return m_duration; } set { m_duration = value; } }

    protected bool m_isSkipTurn = false;
    public bool IsSkipTurn { get { return m_isSkipTurn; } set { m_isSkipTurn = value; } }

    public PhysicalEffectType physicalEffectType;
    public Actor owner;

    public virtual void SetEffect(int power, int duration, Actor target)
    {
        m_name = this.physicalEffectType.ToString();
        m_amount = power;
        m_duration = duration;
        owner = target;

        SoundManager.Instance.PlaySE(8);
    }

    public virtual int UpdateDuration()
    {
        m_duration--;

        if (m_duration <= 0)
        {
            switch (this.physicalEffectType)
            {
                case PhysicalEffectType.Bleed:
                    owner.BleedEffects -= this.m_amount;
                    break;
                case PhysicalEffectType.Infect:
                    owner.InfectEffects -= this.m_amount;
                    break;
                case PhysicalEffectType.Stun:
                    if (owner.StunEffects > 0)
                    {
                        owner.StunEffects = 0;
                    }
                    break;
                case PhysicalEffectType.Buff:
                    owner.BuffEffects--;
                    break;
                default:
                    break;
            }
            Debug.Log(this.Name + " is expired");
            return owner.physicalEffects.IndexOf(this);
        }
        else
        {
            return -1;
        }
    }
}
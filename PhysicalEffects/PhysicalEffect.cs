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
    Buff
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
    public GameObject iconPrefab;

    public virtual void SetEffect(int power, int duration, Actor target)
    {
        m_name = this.physicalEffectType.ToString();
        m_amount = power;
        m_duration = duration;
        owner = target;
    }

    public virtual void UpdateDuration(List<int> indexList)
    {
        m_duration--;
        if (m_duration == 0)
        {
            indexList.Add(owner.physicalEffects.IndexOf(this));
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
                        owner.StunEffects--;
                    }
                    break;
                case PhysicalEffectType.Buff:
                    owner.BuffEffects--;
                    break;
                default:
                    break;
            }
        }
    }
}
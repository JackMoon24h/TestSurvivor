using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : PhysicalEffect 
{
    public override void SetEffect(int power, int duration, Actor target)
    {
        base.SetEffect(power, duration, target);
        physicalEffectType = PhysicalEffectType.Stun;
        m_isSkipTurn = true;
        m_amount = 1;
        m_duration = 1;
    }

}

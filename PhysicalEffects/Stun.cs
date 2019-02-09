using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : PhysicalEffect 
{
    public override void SetEffect(int power, int duration, Actor target)
    {
        physicalEffectType = PhysicalEffectType.Stun;
        m_isSkipTurn = true;
        base.SetEffect(power, duration, target);
        m_amount = 1;
        m_duration = 1;
    }

}

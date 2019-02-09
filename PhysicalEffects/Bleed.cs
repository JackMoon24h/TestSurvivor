using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : PhysicalEffect
{
    public override void SetEffect(int power, int duration, Actor target)
    {
        physicalEffectType = PhysicalEffectType.Bleed;
        m_isSkipTurn = false;
        base.SetEffect(power, duration, target);
    }

}

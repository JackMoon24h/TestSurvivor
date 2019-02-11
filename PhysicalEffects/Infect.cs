using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infect : PhysicalEffect 
{
    public override void SetEffect(int power, int duration, Actor target)
    {
        base.SetEffect(power, duration, target);
        physicalEffectType = PhysicalEffectType.Infect;
        m_isSkipTurn = false;

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hopeless : Affliction 
{

    public override void SetEffects(BaseCharacter target)
    {
        type = AfflictionType.Hopeless;
        m_name = type.ToString();
        m_description = "Feeling or causing despair and stress";
        base.SetEffects(target);
        owner.Damage = Utility.StatIntRound(owner.Damage * (1 - m_effect));
        owner.Protection = Utility.StatIntRound(owner.Protection * (1 - m_effect));


    }
}

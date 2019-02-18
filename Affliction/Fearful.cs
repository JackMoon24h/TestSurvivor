using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fearful : Affliction 
{

    public override void SetEffects(BaseCharacter target)
    {
        type = AfflictionType.Fearful;
        m_name = type.ToString();
        m_description = "Frightened or worried about something";
        base.SetEffects(target);
        owner.Critical = owner.Critical * (1 - m_effect);
        owner.Speed = Utility.StatIntRound(owner.Speed * (1 - m_effect));


    }
}

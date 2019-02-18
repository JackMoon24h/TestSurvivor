using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patience : Virtue 
{

    public override void SetEffects(BaseCharacter target)
    {
        type = VirtueType.Patience;
        m_name = type.ToString();
        m_description = "The capacity to accept or tolerate delay, problems, or suffering without becoming annoyed or anxious";
        base.SetEffects(target);

        owner.Damage = Utility.StatIntRound(owner.Damage * (1 + m_effect));
        owner.Protection = Utility.StatIntRound(owner.Protection * (1 + m_effect));
    }
}

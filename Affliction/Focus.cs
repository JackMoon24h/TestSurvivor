using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Focus : Virtue 
{

    public override void SetEffects(BaseCharacter target)
    {
        type = VirtueType.Focus;
        m_name = type.ToString();
        m_description = "Adapt to the prevailing level of light and become able to see clearly";
        base.SetEffects(target);

        owner.Dodge = (owner.Dodge * (1 + m_effect));
        owner.Accuracy = (owner.Accuracy * (1 + m_effect));
    }
}

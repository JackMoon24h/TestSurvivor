using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hopeless : Affliction 
{

    public override void SetEffects()
    {
        type = AfflictionType.Hopeless;
        m_name = type.ToString();
        m_description = "Feeling or causing despair and stress";
        base.SetEffects();
    }
}

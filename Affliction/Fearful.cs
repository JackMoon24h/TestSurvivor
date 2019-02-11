using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fearful : Affliction 
{

    public override void SetEffects()
    {
        type = AfflictionType.Fearful;
        m_name = type.ToString();
        m_description = "Frightened or worried about something";
        base.SetEffects();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paranoid : Affliction 
{

    public override void SetEffects()
    {
        type = AfflictionType.Paranoid;
        m_name = type.ToString();
        m_description = "Suffering from a mental illness in which you believe that other people are trying to harm you";
        base.SetEffects();
    }
}

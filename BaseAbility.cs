using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbility : MonoBehaviour 
{

    private string abilityName;
    private string abilityDescription;
    private int abilityID;
    private int abilityPower;

    public string AbilityName
    {
        get
        {
            return abilityName;
        }

        set
        {
            abilityName = value;
        }
    }

    public string AbilityDescription
    {
        get
        {
            return abilityDescription;
        }

        set
        {
            abilityDescription = value;
        }
    }

    public int AbilityID
    {
        get
        {
            return abilityID;
        }

        set
        {
            abilityID = value;
        }
    }

    public int AbilityPower
    {
        get
        {
            return abilityPower;
        }

        set
        {
            abilityPower = value;
        }
    }
}

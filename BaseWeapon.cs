using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : BaseStatItem 
{

	public enum WeaponType
    {
        HANDGUN,
        SHOTGUN,
        RIFLE
    }
    public WeaponType weaponType;

    private int m_effectID;

    public int EffectID
    {
        get
        {
            return m_effectID;
        }

        set
        {
            m_effectID = value;
        }
    }
}

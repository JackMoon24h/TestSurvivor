using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : BaseEnemy
{

    public override void Initiate()
    {
        base.Initiate();
        Setting();
    }

    void Setting()
    {

        m_level = 1;
        m_name = "Walker";
        m_maxHealth = 21;
        m_health = m_maxHealth;
        m_damage = 9;
        m_mentalDamage = 8;
        m_protection = 3;
        m_speed = 5;

        m_accuracy = 0.87f;
        m_dodge = 0.07f;
        m_critical = 0.2f;
        m_bleedRes = 0.5f;
        m_infectRes = 0.28f;
        m_stunRes = 0.25f;
        m_moveRes = 0.3f;
    }
}

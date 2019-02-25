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
        m_name = "Zombie";
        m_maxHealth = 1;
        m_health = m_maxHealth;
        m_damage = 8;
        m_mentalDamage = 100;
        m_protection = 2;
        m_speed = 7;

        m_accuracy = 0.99f;
        m_dodge = 0.02f;
        m_critical = 0.9f;
        m_bleedRes = 0.35f;
        m_infectRes = 0.18f;
        m_stunRes = 0.25f;
        m_moveRes = 0.45f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolter : BaseEnemy
{

    public override void Initiate()
    {
        base.Initiate();
        Setting();
    }

    void Setting()
    {
        m_level = 1;
        m_name = "Bolter";
        m_maxHealth = 23;
        m_health = m_maxHealth;
        m_damage = 7;
        m_mentalDamage = 4;
        m_protection = 4;
        m_speed = 3;

        m_accuracy = 0.85f;
        m_dodge = 0.05f;
        m_critical = 0.15f;
        m_bleedRes = 0.3f;
        m_infectRes = 0.3f;
        m_stunRes = 0.55f;
        m_moveRes = 0.35f;
    }
}

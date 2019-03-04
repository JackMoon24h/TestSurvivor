using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viral : BaseEnemy
{

    public override void Initiate()
    {
        base.Initiate();
        Setting();
    }

    void Setting()
    {
        m_level = 1;
        m_name = "Viral";
        m_maxHealth = 18;
        m_health = m_maxHealth;
        m_damage = 5;
        m_mentalDamage = 12;
        m_protection = 3;
        m_speed = 6;

        m_accuracy = 0.91f;
        m_dodge = 0.11f;
        m_critical = 0.15f;
        m_bleedRes = 0.25f;
        m_infectRes = 0.55f;
        m_stunRes = 0.15f;
        m_moveRes = 0.45f;
    }
}

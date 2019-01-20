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
        m_maxHealth = 17;
        m_health = m_maxHealth;
        m_damage = 6;
        m_protection = 4;
        m_speed = 7;
        m_accuracy = 10;
        m_dodge = 9;
        m_critical = 17;
        m_bleedRes = 20;
        m_infectRes = 45;
        m_stunRes = 12;
        m_moveRes = 13;
    }
}

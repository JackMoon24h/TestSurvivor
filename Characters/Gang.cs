﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gang : BaseCharacter 
{

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        Setting();
    }

    // Update is called once per frame
    protected override void Update () 
    {
        base.Update();
	}

    void Setting()
    {
        rarity = Rarity.Common;
        job = BaseCharacter.Job.Gang;
        jobName = job.ToString();
        jobDescription = this.Name + " had lived in slum vilage robbing and stealing money from people before the nightmare";
        m_level = 1;
        m_name = "John Washington";
        m_maxHealth = 22;
        m_health = m_maxHealth;
        m_maxMental = 100;
        m_mental = m_maxMental;
        m_damage = 10;
        m_protection = 4;
        m_endurance = 1;
        m_speed = 10;

        m_accuracy = 0.09f;
        m_dodge = 0.01f;
        m_critical = 0.9f;
        m_virtue = 0.5f;
        m_bleedRes = 0.35f;
        m_infectRes = 0.48f;
        m_stunRes = 0.25f;
        m_moveRes = 0.45f;
        m_deathBlow = 0.15f;
    }

}

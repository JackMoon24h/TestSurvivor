using System.Collections;
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
        m_damage = 30;
        m_protection = 5;
        m_endurance = 5;
        m_speed = 10;
        m_accuracy = 12;
        m_dodge = 15;
        m_critical = 27;
        m_virtue = 5;
        m_bleedRes = 25;
        m_infectRes = 18;
        m_stunRes = 18;
        m_moveRes = 17;
        m_deathBlow = 14;
    }

}

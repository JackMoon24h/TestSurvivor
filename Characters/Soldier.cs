using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : BaseCharacter 
{
    private string[] m_candidates =
    {
        "Brady Maison",
        "Charles Gipton",
        "George Risson"
    };
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        Setting();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    void Setting()
    {
        rarity = Rarity.SuperRare;
        job = BaseCharacter.Job.Soldier;
        jobName = job.ToString();
        jobDescription = this.Name + " was a veteran soldier who had experienced harsh conditions before break out.";
        m_level = 18;

        int rand = Random.Range(0, m_candidates.Length);

        m_name = m_candidates[rand];
        m_maxHealth = 26;
        m_health = m_maxHealth;
        m_maxMental = 100;
        m_mental = m_maxMental;
        m_damage = 14;
        m_protection = 5;
        m_endurance = 2;
        m_speed = 4;

        m_accuracy = 0.78f;
        m_dodge = 0.06f;
        m_critical = 0.25f;
        m_virtue = 0.25f;
        m_bleedRes = 0.3f;
        m_infectRes = 0.3f;
        m_stunRes = 0.40f;
        m_moveRes = 0.50f;
        m_deathBlow = 0.15f;

        m_preffredPosition = new Vector2[]
        {
            new Vector2(0.9f, 0.9f),
            new Vector2(0.8f, 0.8f),
            new Vector2(0.3f, 0.3f),
            new Vector2(0.1f, 0.1f)
        };

        m_preffredTarget = new Vector2[]
        {
            new Vector2(0.9f, 0.9f),
            new Vector2(0.8f, 0.8f),
            new Vector2(0.3f, 0.3f),
            new Vector2(0.1f, 0.1f)
        };
    }
}

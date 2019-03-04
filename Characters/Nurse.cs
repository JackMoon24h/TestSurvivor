using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nurse : BaseCharacter 
{
    private string[] m_candidates =
    {
        "Kate Kim",
        "Silly Grande",
        "Clara Hue"
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
        rarity = Rarity.Legend;
        job = BaseCharacter.Job.Nurse;
        jobName = job.ToString();
        jobDescription = this.Name + ", known as the Psychiatrist, was a private nurse with full of secrets";
        m_level = 12;
        int rand = Random.Range(0, m_candidates.Length);

        m_name = m_candidates[rand];
        m_maxHealth = 18;
        m_health = m_maxHealth;
        m_maxMental = 100;
        m_mental = m_maxMental;
        m_damage = 8;
        m_protection = 3;
        m_endurance = 2;
        m_speed = 5;

        m_accuracy = 0.84f;
        m_dodge = 0.12f;
        m_critical = 0.17f;
        m_virtue = 0.35f;
        m_bleedRes = 0.45f;
        m_infectRes = 0.5f;
        m_stunRes = 0.25f;
        m_moveRes = 0.16f;
        m_deathBlow = 0.15f;

        m_preffredPosition = new Vector2[]
        {
            new Vector2(0.2f, 0.2f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.9f, 0.9f),
            new Vector2(0.65f, 0.65f)
        };

        m_preffredTarget = new Vector2[]
        {
            new Vector2(0.5f, 0.5f),
            new Vector2(0.75f, 0.75f),
            new Vector2(0.75f, 0.75f),
            new Vector2(0.3f, 0.3f)
        };
    }
}

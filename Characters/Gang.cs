using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gang : BaseCharacter 
{
    private string[] m_candidates =
    {
        "John Washington",
        "Will Graham",
        "Tony Wilson",
        "Jay Robinson",
        "Harry Watson",
        "Ian Trigger",
        "Chris Pompei",
        "Brian Musk",
        "Joe Abraham",
        "Jason Tiler"
    };
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
        rarity = Rarity.Rare;
        job = BaseCharacter.Job.Gang;
        jobName = job.ToString();
        jobDescription = this.Name + " had lived in slum vilage robbing and stealing money from people before the nightmare";
        m_level = 14;

        int rand = Random.Range(0, m_candidates.Length);

        m_name = m_candidates[rand];
        m_maxHealth = 21;
        m_health = m_maxHealth;
        m_maxMental = 100;
        m_mental = m_maxMental;
        m_damage = 11;
        m_protection = 4;
        m_endurance = 1;
        m_speed = 6;

        m_accuracy = 0.9f;
        m_dodge = 0.09f;
        m_critical = 0.21f;
        m_virtue = 0.3f;
        m_bleedRes = 0.45f;
        m_infectRes = 0.35f;
        m_stunRes = 0.25f;
        m_moveRes = 0.33f;
        m_deathBlow = 0.15f;

        m_preffredPosition = new Vector2[]
        {
            new Vector2(0.3f, 0.3f),
            new Vector2(0.7f, 0.7f),
            new Vector2(0.8f, 0.8f),
            new Vector2(0.5f, 0.5f)
        };

        m_preffredTarget = new Vector2[]
        {
            new Vector2(0.4f, 0.4f),
            new Vector2(0.7f, 0.7f),
            new Vector2(0.7f, 0.7f),
            new Vector2(0.5f, 0.5f)
        };
    }
}

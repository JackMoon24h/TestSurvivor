using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour 
{
    public enum Job
    {
        Gang,
        Soldier,
        Thief
    }

    public enum PhysicalState
    {
        Normal,
        Buff,
        Bleed,
        Infected,
        Stunned
    }

    public enum PsychologicalState
    {
        Idle,
        Broken,
        Virtue
    }

    public Job job;
    public PhysicalState physicalState = PhysicalState.Normal;
    public PsychologicalState psychologicalState = PsychologicalState.Idle;
    public int currentPosition;

    // Basic Character's Paramters
    string m_name;
    public string Name { get { return m_name; } }

    int m_maxHealth;
    public int MaxHealth { get { return m_maxHealth; } set { m_maxHealth = value; } }

    int m_health;
    public int Health { get { return m_health; } set { m_health = value; } }

    int m_maxMental;
    public int MaxMental { get { return m_maxMental; } set { m_maxMental = value; } }

    int m_mental;
    public int Mental { get { return m_mental; } set { m_mental = value; } }

    int m_damage;
    public int Damage { get { return m_damage; } set { m_damage = value; } }

    int m_protection;
    public int Protection { get { return m_protection; } set { m_protection = value; } }

    int m_tolerance;
    public int Tolerance { get { return m_tolerance; } set { m_tolerance = value; } }

    int m_speed;
    public int Speed { get { return m_speed; } set { m_speed = value; } }

    float m_accuracy;
    public float Accuracy { get { return m_accuracy; } set { m_accuracy = value; } }

    float m_dodge;
    public float Dodge { get { return m_dodge; } set { m_dodge = value; } }

    float m_critical;
    public float Critical { get { return m_critical; } set { m_critical = value; } }

    float m_virtue;
    public float Virtue { get { return m_virtue; } set { m_virtue = value; } }

    float m_stressRes;
    public float StressRes { get { return m_stressRes; } set { m_stressRes = value; } }

    float m_bleedRes;
    public float BleedRes { get { return m_bleedRes; } set { m_bleedRes = value; } }

    float m_infectRes;
    public float InfectRes { get { return m_infectRes; } set { m_infectRes = value; } }

    float m_stunRes;
    public float StunRes { get { return m_stunRes; } set { m_stunRes = value; } }

    float m_moveRes;
    public float MoveRes { get { return m_moveRes; } set { m_moveRes = value; } }

    float m_deathBlow;
    public float DeathBlow { get { return m_deathBlow; } set { m_deathBlow = value; } }



    // Use this for initialization
    void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
}

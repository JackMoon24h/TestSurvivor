using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour 
{
    public enum Job
    {
        Thug,
        Soldier,
        Thief,
        Nurse
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

    // Enum
    public Job job;
    public PhysicalState physicalState = PhysicalState.Normal;
    public PsychologicalState psychologicalState = PsychologicalState.Idle;

    // Reference
    public BoxCollider2D col;

    // Assign them in Inspecter
    public GameObject cursor;
    public GameObject targetCursor;

    // Status
    public int currentPosition;
    public bool isActive = false;
    public bool isTarget = false;
    public bool isDead = false;

    // Parameters

    protected int m_level;
    public int Level { get { return m_level; } set { m_level = value; } }

    protected string m_name;
    public string Name { get { return m_name; } set { m_name = value; } }

    protected float m_maxHealth;
    public float MaxHealth { get { return m_maxHealth; } set { m_maxHealth = value; } }

    protected float m_health;
    public float Health { get { return m_health; } set { m_health = value; } }

    protected float m_maxMental;
    public float MaxMental { get { return m_maxMental; } set { m_maxMental = value; } }

    protected float m_mental;
    public float Mental { get { return m_mental; } set { m_mental = value; } }

    protected float m_damage;
    public float Damage { get { return m_damage; } set { m_damage = value; } }

    protected float m_protection;
    public float Protection { get { return m_protection; } set { m_protection = value; } }

    protected float m_endurance;
    public float Endurance { get { return m_endurance; } set { m_endurance = value; } }

    protected float m_speed;
    public float Speed { get { return m_speed; } set { m_speed = value; } }

    protected float m_accuracy;
    public float Accuracy { get { return m_accuracy; } set { m_accuracy = value; } }

    protected float m_dodge;
    public float Dodge { get { return m_dodge; } set { m_dodge = value; } }

    protected float m_critical;
    public float Critical { get { return m_critical; } set { m_critical = value; } }

    protected float m_virtue;
    public float Virtue { get { return m_virtue; } set { m_virtue = value; } }

    protected float m_stressRes;
    public float StressRes { get { return m_stressRes; } set { m_stressRes = value; } }

    protected float m_bleedRes;
    public float BleedRes { get { return m_bleedRes; } set { m_bleedRes = value; } }

    protected float m_infectRes;
    public float InfectRes { get { return m_infectRes; } set { m_infectRes = value; } }

    protected float m_stunRes;
    public float StunRes { get { return m_stunRes; } set { m_stunRes = value; } }

    protected float m_moveRes;
    public float MoveRes { get { return m_moveRes; } set { m_moveRes = value; } }

    protected float m_deathBlow;
    public float DeathBlow { get { return m_deathBlow; } set { m_deathBlow = value; } }

    // Ability
    public List<Ability> abilitySlots = new List<Ability>();
    public Ability ability1;
    public Ability ability2;
    public Ability ability3;
    public Ability ability4;
    public Ability defaultAbility;

    // Images
    public Sprite thumbSprite;

    // Use this for initialization

    private void Awake()
    {
        col = this.GetComponent<BoxCollider2D>();
        cursor = this.transform.GetChild(1).gameObject;
        targetCursor = this.transform.GetChild(2).gameObject;
    }

    void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    public void SetActive(bool state)
    {
        this.cursor.SetActive(state);
        this.isActive = state;
    }

    public void SetAsTarget(bool state)
    {
        this.targetCursor.SetActive(state);
        this.isTarget = state;
    }

    public Ability GetAbilityByNumber(int number)
    {
        if (this.abilitySlots[number - 1] != null)
        {
            return this.abilitySlots[number - 1];
        }
        else
        {
            return null;
        }
    }

    public Ability GetAbilityByClick()
    {
        
        return null;
    }
}

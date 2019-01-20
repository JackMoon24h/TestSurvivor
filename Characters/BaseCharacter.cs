using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(CharacterAction))]
[RequireComponent(typeof(SkillManager))]
public class BaseCharacter : Actor 
{
    // Ref
    [HideInInspector]
    public CharacterAction characterAction;
    public SkillManager skillManager;
    BoxCollider2D col;

    // Images
    public Sprite profileImage;

    // Exclusive properties
    protected string jobName;
    public string JobName { get { return jobName; } set { jobName = value; } }

    protected string jobDescription;
    public string JobDescription { get { return jobDescription; } set { jobDescription = value; } }

    protected int m_maxMental;
    public int MaxMental { get { return m_maxMental; } set { m_maxMental = value; } }

    protected int m_mental;
    public int Mental { get { return m_mental; } set { m_mental = value; } }

    protected int m_endurance;
    public int Endurance { get { return m_endurance; } set { m_endurance = value; } }

    protected int m_stressRes;
    public int StressRes { get { return m_stressRes; } set { m_stressRes = value; } }

   
    // float params

    protected float m_virtue;
    public float Virtue { get { return m_virtue; } set { m_virtue = value; } }

    protected float m_deathBlow;
    public float DeathBlow { get { return m_deathBlow; } set { m_deathBlow = value; } }

    public enum Rarity
    {
        Common,
        Rare,
        Unique,
        Legendary
    }

    public enum Job
    {
        Gang,
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
        Sane,
        Broken,
        Virtue
    }

    // Enum
    public Rarity rarity;
    public Job job;
    public PhysicalState physicalState = PhysicalState.Normal;
    public PsychologicalState psychologicalState = PsychologicalState.Sane;

    protected virtual void Awake()
    {
        skillManager = GetComponent<SkillManager>();
        characterAction = GetComponent<CharacterAction>();
        col = GetComponent<BoxCollider2D>();
    }

    // Use this for initialization
    protected virtual void Start () 
    {

	}
	
	// Update is called once per frame
	protected virtual void Update () 
    {
        Move();
	}

    protected void Move()
    {
        if (PlayerManager.instance.isMovingForward)
        {
            characterAction.MoveForwardAction();
        }
        else if (PlayerManager.instance.isMovingBackWard)
        {
            characterAction.MoveBackWardAction();
        }
        else
        {
            characterAction.StopAction();
        }
    }

    public virtual void SetPreferredPosition()
    {

    }

    public override void ReceiveDamage(int dmg)
    {
        base.ReceiveDamage(dmg);
    }
}

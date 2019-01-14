using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActor
{
    void ApplyDamage();
}

[System.Serializable]
[RequireComponent(typeof(CharacterAction))]
[RequireComponent(typeof(SkillManager))]
public class BaseCharacter : MonoBehaviour 
{
    // Ref
    [HideInInspector]
    public CharacterAction characterAction;
    public SkillManager skillManager;
    BoxCollider2D col;

    // Assign from the inspector
    public GameObject cursor;
    public GameObject targetCursor;

    [SerializeField]
    protected int m_position;
    public int Position { get { return m_position; } set { m_position = value; } }

    private int[] m_preffredPosition = new int[4];
    public int[] PreffredPosition { get{return m_preffredPosition;} set{m_preffredPosition = value;}}

    // Main States
    public bool isActive = false;
    public bool isDead = false;
    public bool isTargeted = false;

    // Images
    public Sprite profileImage;

    // base params
    protected string jobName;
    public string JobName { get { return jobName; } set { jobName = value; } }

    protected string jobDescription;
    public string JobDescription { get { return jobDescription; } set { jobDescription = value; } }

    protected int m_level;
    public int Level { get { return m_level; } set { m_level = value; } }

    protected string m_name;
    public string Name { get { return m_name; } set { m_name = value; } }

    // int params
    protected int m_maxHealth;
    public int MaxHealth { get { return m_maxHealth; } set { m_maxHealth = value; } }

    protected int m_health;
    public int Health { get { return m_health; } set { m_health = value; } }

    protected int m_maxMental;
    public int MaxMental { get { return m_maxMental; } set { m_maxMental = value; } }

    protected int m_mental;
    public int Mental { get { return m_mental; } set { m_mental = value; } }

    protected int m_damage;
    public int Damage { get { return m_damage; } set { m_damage = value; } }

    protected int m_protection;
    public int Protection { get { return m_protection; } set { m_protection = value; } }

    protected int m_endurance;
    public int Endurance { get { return m_endurance; } set { m_endurance = value; } }

    protected int m_speed;
    public int Speed { get { return m_speed; } set { m_speed = value; } }

    protected int m_stressRes;
    public int StressRes { get { return m_stressRes; } set { m_stressRes = value; } }

    protected int m_bleedRes;
    public int BleedRes { get { return m_bleedRes; } set { m_bleedRes = value; } }

    protected int m_infectRes;
    public int InfectRes { get { return m_infectRes; } set { m_infectRes = value; } }

    protected int m_stunRes;
    public int StunRes { get { return m_stunRes; } set { m_stunRes = value; } }

    protected int m_moveRes;
    public int MoveRes { get { return m_moveRes; } set { m_moveRes = value; } }

    // float params
    protected float m_accuracy;
    public float Accuracy { get { return m_accuracy; } set { m_accuracy = value; } }

    protected float m_dodge;
    public float Dodge { get { return m_dodge; } set { m_dodge = value; } }

    protected float m_critical;
    public float Critical { get { return m_critical; } set { m_critical = value; } }

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

    public BaseSkill activeCommand;

    private void Awake()
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

    protected virtual void OnSelect()
    {
        if(Commander.instance.IsBattle)
        {
            return;
        }
        PlayerManager.instance.SetActiveCharacterAtPos(this.Position);
        this.isActive = true;
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

    public virtual void CastToEnemy(BaseSkill activeSkill, BaseEnemy target)
    {
        // Player Action
        PlayerManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        // Enemy Action
        target.enemyAction.Act(activeSkill.skillTargetActionType);

        activeSkill.Excute(target.gameObject);
    }

    public virtual void CastToEnemies(BaseSkill activeSkill, List<BaseEnemy> targets)
    {
        // Player Action
        PlayerManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        // Enemy Action
        foreach(var t in targets)
        {
            t.enemyAction.Act(activeSkill.skillTargetActionType);
            activeSkill.Excute(t.gameObject);
        }
    }

    public virtual void CastToSelf(BaseSkill activeSkill)
    {
        PlayerManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        activeSkill.Excute(gameObject);
    }

    public virtual void CastToAlly(BaseSkill activeSkill, BaseCharacter target)
    {
        // Player Action
        PlayerManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        // Ally Action
        target.characterAction.Act(activeSkill.skillTargetActionType);


        activeSkill.Excute(target.gameObject);
    }

    public virtual void CastToAllies(BaseSkill activeSkill, List<BaseCharacter> targets)
    {
        // Player Action
        PlayerManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        // Enemy Action
        foreach (var t in targets)
        {
            if(t != PlayerManager.instance.activeCharacter)
            {
                t.characterAction.Act(activeSkill.skillTargetActionType);
                activeSkill.Excute(t.gameObject);
            }
        }

    }

    public virtual void CastToRandomTarget(BaseSkill activeSkill)
    {
        Debug.Log("Implement later on");
    }

    public void ReceiveDamage(int power)
    {
        m_health -= power;
        if(m_health <= 0)
        {
            m_health = 0;
            this.isDead = true;
            PlayerManager.instance.characterList.RemoveAt(this.Position - 1);
        }
    }
}

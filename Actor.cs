using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(CharacterAction))]
[RequireComponent(typeof(SkillManager))]
public class Actor : MonoBehaviour 
{
    public enum Job
    {
        Gang,
        Soldier,
        Thief,
        Nurse,
        Walker,
        Viral,
        Goon,
        Volatile
    }
    public Job job;

    // Assign from the inspector
    public GameObject cursor;
    public GameObject targetCursor;
    public GameObject hpGauge;

    [HideInInspector] public CharacterAction characterAction;
    [HideInInspector] public SkillManager skillManager;
    [HideInInspector] public BoxCollider2D col;

    [SerializeField]
    protected int m_position;
    public int Position { get { return m_position; } set { m_position = value; } }

    protected int[] m_preffredPosition = new int[4];
    public int[] PreffredPosition { get { return m_preffredPosition; } set { m_preffredPosition = value; } }

    // Main States
    public bool isActive = false;
    public bool isDead = false;
    public bool isTargeted = false;
    public bool isSwapTarget = false;

    // Common Parameters
    protected int m_level;
    public int Level { get { return m_level; } set { m_level = value; } }

    protected string m_name;
    public string Name { get { return m_name; } set { m_name = value; } }

    protected int m_maxHealth;
    public int MaxHealth { get { return m_maxHealth; } set { m_maxHealth = value; } }

    protected int m_health;
    public int Health { get { return m_health; } set { m_health = value; } }

    protected int m_damage;
    public int Damage { get { return m_damage; } set { m_damage = value; } }

    protected int m_protection;
    public int Protection { get { return m_protection; } set { m_protection = value; } }

    protected int m_speed;
    public int Speed { get { return m_speed; } set { m_speed = value; } }

    // Common Float Parameters
    protected float m_accuracy;
    public float Accuracy { get { return m_accuracy; } set { m_accuracy = value; } }

    protected float m_dodge;
    public float Dodge { get { return m_dodge; } set { m_dodge = value; } }

    protected float m_critical;
    public float Critical { get { return m_critical; } set { m_critical = value; } }

    // Common Resistances
    protected int m_bleedRes;
    public int BleedRes { get { return m_bleedRes; } set { m_bleedRes = value; } }

    protected int m_infectRes;
    public int InfectRes { get { return m_infectRes; } set { m_infectRes = value; } }

    protected int m_stunRes;
    public int StunRes { get { return m_stunRes; } set { m_stunRes = value; } }

    protected int m_moveRes;
    public int MoveRes { get { return m_moveRes; } set { m_moveRes = value; } }

    protected int m_debuffRes;
    public int DebuffRes { get { return m_debuffRes; } set { m_debuffRes = value; } }

    public BaseSkill activeCommand;

    protected virtual void Awake()
    {
        characterAction = GetComponent<CharacterAction>();
        skillManager = GetComponent<SkillManager>();
        col = GetComponent<BoxCollider2D>();
    }

    public virtual void CastToEnemy(BaseSkill activeSkill, BaseEnemy target)
    {
        // Player Action
        PlayerManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        // Enemy Action
        target.characterAction.Act(activeSkill.skillTargetActionType);

        activeSkill.Excute(target.gameObject);
    }

    public virtual void CastToEnemies(BaseSkill activeSkill, List<BaseEnemy> targets)
    {
        // Player Action
        PlayerManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        // Enemy Action
        foreach (var t in targets)
        {
            t.characterAction.Act(activeSkill.skillTargetActionType);
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
            if (t != PlayerManager.instance.activeCharacter)
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

    // Handle Effects
    public virtual void ReceiveDamage(int dmg)
    {
        var actualDMG = (int)Mathf.Round(dmg / this.m_protection);
        this.m_health -= actualDMG;

        if (this.m_health <= 0)
        {
            this.m_health = 0;
            this.isDead = true;
            Dead();
        }

        UpdateHPBar();
    }

    public virtual void UpdateHPBar()
    {
        var ratio = Mathf.Max((float)this.m_health / (float)this.m_maxHealth, 0);

        iTween.ScaleTo(this.hpGauge, iTween.Hash(
            "x", ratio,
            "isLocal", true,
            "easetype", iTween.EaseType.easeInQuart,
            "time", 1f,
            "delay", 0.5f
        ));
    }

    public virtual void Dead()
    {
        characterAction.DeadAction();
    }
}

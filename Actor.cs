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

    public static Vector2 enemyCorrection = new Vector2(0.5f, 0f);
    public static float iconSpacing = 0.8f;
    public static readonly Vector2[] effectsPositions =
    {
        new Vector2(-2.7f + iconSpacing * 1, 3f),
        new Vector2(-2.7f + iconSpacing * 2, 3f),
        new Vector2(-2.7f + iconSpacing * 3, 3f),
        new Vector2(-2.7f + iconSpacing * 4, 3f),
    };

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

    public float setEffectDelay = 0.5f;

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
    protected float m_bleedRes;
    public float BleedRes { get { return m_bleedRes; } set { m_bleedRes = value; } }

    protected float m_infectRes;
    public float InfectRes { get { return m_infectRes; } set { m_infectRes = value; } }

    protected float m_stunRes;
    public float StunRes { get { return m_stunRes; } set { m_stunRes = value; } }

    protected float m_moveRes;
    public float MoveRes { get { return m_moveRes; } set { m_moveRes = value; } }



    public BaseSkill activeCommand;

    // Physical Effects
    public List<PhysicalEffect> physicalEffects = new List<PhysicalEffect>();

    int m_bleedEffects;
    public int BleedEffects { get { return m_bleedEffects; } set { m_bleedEffects = value; } }

    int m_infectEffects;
    public int InfectEffects { get { return m_infectEffects; } set { m_infectEffects = value; } }

    int m_buffEffects;
    public int BuffEffects { get { return m_buffEffects; } set { m_buffEffects = value; } }

    int m_stunEffects;
    public int StunEffects { get { return m_stunEffects; } set { m_stunEffects = value; } }

    // For Handle Effects
    bool m_isSubActionOver = true;
    public bool IsSubActionOver { get { return m_isSubActionOver; } set { m_isSubActionOver = value; } }

    protected virtual void Awake()
    {
        characterAction = GetComponent<CharacterAction>();
        skillManager = GetComponent<SkillManager>();
        col = GetComponent<BoxCollider2D>();
        m_isSubActionOver = true;
    }

    public virtual void CastToEnemy(BaseSkill activeSkill, BaseEnemy target)
    {
        // Player Action
        PlayerManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        // Enemy Action
        target.characterAction.Act(activeSkill.skillTargetActionType);

        activeSkill.Excute(this, target.gameObject);
    }

    public virtual void CastToEnemies(BaseSkill activeSkill, List<BaseEnemy> targets)
    {
        // Player Action
        PlayerManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        // Enemy Action
        foreach (var t in targets)
        {
            t.characterAction.Act(activeSkill.skillTargetActionType);
            activeSkill.Excute(this, t.gameObject);
        }
    }

    public virtual void CastToSelf(BaseSkill activeSkill)
    {
        PlayerManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        activeSkill.Excute(this, gameObject);
    }

    public virtual void CastToAlly(BaseSkill activeSkill, BaseCharacter target)
    {
        // Player Action
        PlayerManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        // Ally Action
        if(target != this)
        {
            target.characterAction.Act(activeSkill.skillTargetActionType);
        }

        activeSkill.Excute(this, target.gameObject);
    }

    public virtual void CastToAllies(BaseSkill activeSkill, List<BaseCharacter> targets)
    {
        // Player Action
        PlayerManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        // Enemy Action
        foreach (var t in targets)
        {
            if (t != this)
            {
                t.characterAction.Act(activeSkill.skillTargetActionType);
            }
            activeSkill.Excute(this, t.gameObject);
        }
    }

    public virtual void CastToRandomTarget(BaseSkill activeSkill)
    {
        Debug.Log("Implement later on");
    }

    // Handle Effects
    public virtual void TakeDamage(int dmg)
    {
        var actualDMG = (int)Mathf.Round(dmg / this.m_protection);
        this.m_health -= actualDMG;

        UIManager.instance.CreateEffect("Damage", this, actualDMG);

        DeathCheck();
    }

    void DeathCheck()
    {
        if (this.m_health <= 0)
        {
            this.m_health = 0;
            UpdateHPBar();
            this.isDead = true;
            characterAction.Dead();
        }
        else
        {
            UpdateHPBar();
        }
    }

    public virtual void UpdateHPBar()
    {
        var ratio = Mathf.Max((float)this.m_health / (float)this.m_maxHealth, 0);

        iTween.ScaleTo(this.hpGauge, iTween.Hash(
            "x", ratio,
            "isLocal", true,
            "easetype", iTween.EaseType.easeInQuart,
            "time", 1.2f,
            "delay", 0.6f
        ));
    }



    public void TakeEffect(PhysicalEffectType type, int pow, int dur)
    {
        StartCoroutine(PhysicalEffectRoutine(type, pow, dur));
    }

    IEnumerator PhysicalEffectRoutine(PhysicalEffectType type, int pow, int dur)
    {
        m_isSubActionOver = false;
        while (Commander.instance.IsActing)
        {
            yield return null;
        }

        var typeNumber = (int)type;

        var effectObject = Instantiate(Commander.instance.physicalEffectPrefabs[typeNumber]);
        effectObject.transform.SetParent(this.gameObject.transform);

        if(this is BaseCharacter)
        {
            effectObject.transform.localPosition = effectsPositions[typeNumber];
        }
        else
        {
            effectObject.transform.localPosition = effectsPositions[typeNumber] + enemyCorrection;
        }


        var effect = effectObject.GetComponent<PhysicalEffect>();
        effect.SetEffect(pow, dur, this);

        yield return new WaitForSeconds(setEffectDelay);

        this.SetPhysicalEffect(effect);

        m_isSubActionOver = true;
    }
   

    void SetPhysicalEffect(PhysicalEffect effect)
    {
        physicalEffects.Add(effect);
        effect.owner = this;
        effect.gameObject.transform.SetParent(this.gameObject.transform);

        switch (effect.physicalEffectType)
        {
            case PhysicalEffectType.Bleed:
                m_bleedEffects += effect.Amount;
                break;
            case PhysicalEffectType.Infect:
                m_infectEffects += effect.Amount;
                break;
            case PhysicalEffectType.Stun:

                // Stun cannot be dupilicated
                if (m_stunEffects == 0)
                {
                    m_stunEffects++;
                }
                break;
            case PhysicalEffectType.Buff:
                m_buffEffects++;
                break;
            default:
                break;
        }
        UIManager.instance.CreateEffect(effect.Name, this, effect.Amount);
    }

    // Removing elements in iteration must be performed outside of foreach loop. Store its index and then delete it later.
    public void RemovePhysicalEffect(PhysicalEffect effect)
    {
        switch (effect.physicalEffectType)
        {
            case PhysicalEffectType.Bleed:
                m_bleedEffects -= effect.Amount;
                break;
            case PhysicalEffectType.Infect:
                m_infectEffects -= effect.Amount;
                break;
            case PhysicalEffectType.Stun:
                if(m_stunEffects > 0)
                {
                    m_stunEffects--;
                }
                break;
            case PhysicalEffectType.Buff:
                m_buffEffects--;
                break;
            default:
                break;
        }
    }

    public void OnTurnStart()
    {
        StartCoroutine(OnTurnStartRoutine());
    }

    IEnumerator OnTurnStartRoutine()
    {
        m_isSubActionOver = false;

        var dmgSum = m_bleedEffects + m_infectEffects;
        this.m_health -= dmgSum;

        if (dmgSum > 0)
        {
            UIManager.instance.CreateEffect("Damage", this, dmgSum);
            DeathCheck();

            while(Commander.instance.IsActing)
            {
                yield return null;
            }

            if (this.gameObject == null || this.isDead)
            {
                yield break;
            }
        }

        yield return new WaitForSeconds(0.2f);

        if (m_stunEffects > 0)
        {
            Commander.instance.turnStateMachine.IsSkipTurn = true;
        }

        var indexList = new List<int>();
        foreach (var t in physicalEffects)
        {
            t.UpdateDuration(indexList);
        }

        foreach(var t in indexList)
        {
            Destroy(physicalEffects[t].gameObject);
            physicalEffects.RemoveAt(t);
        }

        // Random Behavior

        m_isSubActionOver = true;
    }

    public virtual void TakeMentalDamage(int mentalDmg)
    {

    }

    public virtual void TakeMentalEffect()
    {

    }
}

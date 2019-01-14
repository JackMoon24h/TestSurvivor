using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAction))]
public class BaseEnemy : MonoBehaviour 
{
   
    public EnemyAction enemyAction;

    // Assign from the inspector
    public GameObject cursor;
    public GameObject targetCursor;

    [SerializeField]
    protected int m_position;
    public int Position { get { return m_position; } set { m_position = value; } }

    // Main States
    public bool isActive = false;
    public bool isDead = false;
    public bool isTargeted = false;

    protected int m_level;
    public int Level { get { return m_level; } set { m_level = value; } }

    protected string m_name;
    public string Name { get { return m_name; } set { m_name = value; } }

    // int params
    protected int m_maxHealth;
    public int MaxHealth { get { return m_maxHealth; } set { m_maxHealth = value; } }

    protected int m_health;
    public int Health { get { return m_health; } set { m_health = value; } }

    protected int m_damage;
    public int Damage { get { return m_damage; } set { m_damage = value; } }

    protected int m_mentalDamage;
    public int MentalDamage { get { return m_mentalDamage; } set { m_mentalDamage = value; } }

    protected int m_protection;
    public int Protection { get { return m_protection; } set { m_protection = value; } }

    protected int m_speed;
    public int Speed { get { return m_speed; } set { m_speed = value; } }

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

    public enum EnemyType
    {
        Walker,
        Viral,
        Goon,
        Volatile
    }
    public EnemyType enemyType;

    public virtual void Initiate()
    {
        enemyAction = GetComponent<EnemyAction>();
    }

    public virtual void CastToEnemy(BaseSkill activeSkill, BaseEnemy target)
    {

    }

    public virtual void CastToEnemies(BaseSkill activeSkill, List<BaseEnemy> targets)
    {

    }

    public virtual void CastToSelf(BaseSkill activeSkill)
    {

    }

    public virtual void CastToAlly(BaseSkill activeSkill, BaseCharacter target)
    {

    }

    public virtual void CastToAllies(BaseSkill activeSkill, List<BaseCharacter> targets)
    {

    }

    public virtual void CastToRandomTarget(BaseSkill activeSkill)
    {

    }

    public void ReceiveDamage(int power)
    {
        var damage = Mathf.Max(0, power - m_protection);

        m_health -= damage;
        if (m_health <= 0)
        {
            m_health = 0;
            this.isDead = true;
            PlayerManager.instance.characterList.RemoveAt(this.Position - 1);
        }
    }
}

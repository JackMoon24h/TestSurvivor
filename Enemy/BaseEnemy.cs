using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAction))]
public class BaseEnemy : Actor 
{
   
    public EnemyAction enemyAction;

    // Exclusive properties

    protected int m_mentalDamage;
    public int MentalDamage { get { return m_mentalDamage; } set { m_mentalDamage = value; } }


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

    public override void CastToEnemy(BaseSkill activeSkill, BaseEnemy target)
    {

    }

    public override void CastToEnemies(BaseSkill activeSkill, List<BaseEnemy> targets)
    {

    }

    public override void CastToSelf(BaseSkill activeSkill)
    {

    }

    public override void CastToAlly(BaseSkill activeSkill, BaseCharacter target)
    {

    }

    public override void CastToAllies(BaseSkill activeSkill, List<BaseCharacter> targets)
    {

    }

    public override void CastToRandomTarget(BaseSkill activeSkill)
    {

    }

    public override void ReceiveDamage(int dmg)
    {
        base.ReceiveDamage(dmg);
    }
}

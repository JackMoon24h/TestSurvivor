using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterAction))]
public class BaseEnemy : Actor 
{
   
    public CharacterAction characterAction;

    // Exclusive properties

    protected int m_mentalDamage;
    public int MentalDamage { get { return m_mentalDamage; } set { m_mentalDamage = value; } }

    public virtual void Initiate()
    {
        characterAction = GetComponent<CharacterAction>();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterAction))]
public class BaseEnemy : Actor
{
    // Exclusive properties
    public List<BaseSkill> candidates = new List<BaseSkill>();
    public Actor target;

    protected int m_mentalDamage;
    public int MentalDamage { get { return m_mentalDamage; } set { m_mentalDamage = value; } }

    protected override void Awake()
    {
        base.Awake();
    }

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

    public void ChooseCommand()
    {
        candidates.Clear();

        for (int i = 0; i < skillManager.skillList.Count; i++)
        {
            if (IsAvailableCommand(skillManager.skillList[i]))
            {
                candidates.Add(skillManager.skillList[i]);
            }
        }

        if (candidates.Count == 0)
        {
            // Move Position or Skip Turn if this enemy is left alone
            return;
        }

        int rand = Random.Range(1, candidates.Count);
        EnemyManager.instance.activeCommand = candidates[rand];
        Commander.instance.turnStateMachine.currentTurnState = TurnStateMachine.TurnState.ConfirmTarget;
    }

    bool IsAvailableCommand(BaseSkill thisSkill)
    {
        bool result;
        int temp = 0;
        // First, caster's position
        if (thisSkill.castPositions[this.Position - 1])
        {
            // Secondly, check targets' positions

            switch (thisSkill.skillRange)
            {
                case SkillRange.Unfriendly:

                    for (int i = 0; i < PlayerManager.instance.characterList.Count; i++)
                    {
                        if (thisSkill.targetPositions[i])
                        {
                            temp += 1;
                        }
                    }

                    if (temp > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }

                    break;

                case SkillRange.Friendly:

                    for (int i = 0; i < EnemyManager.instance.characterList.Count; i++)
                    {
                        if (thisSkill.targetPositions[i])
                        {
                            temp += 1;
                        }
                    }

                    if (temp > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    break;

                case SkillRange.Self:
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }

            return result;
        }
        else
        {
            return false;
        }
    }
}
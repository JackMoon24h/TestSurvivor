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

    public void CastToEnemy(BaseSkill activeSkill, BaseCharacter target)
    {
        // Player Action
        EnemyManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        // Enemy Action
        //target.characterAction.Act(activeSkill.skillTargetActionType);

        activeSkill.Excute(this, target.gameObject);
    }

    public void CastToEnemies(BaseSkill activeSkill, List<BaseCharacter> targets)
    {
        // Player Action
        EnemyManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        // Enemy Action
        foreach (var t in targets)
        {
            //t.characterAction.Act(activeSkill.skillTargetActionType);
            activeSkill.Excute(this, t.gameObject);
        }
    }

    public override void CastToSelf(BaseSkill activeSkill)
    {
        EnemyManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        activeSkill.Excute(this, gameObject);
    }

    public void CastToAlly(BaseSkill activeSkill, BaseEnemy target)
    {
        // Player Action
        EnemyManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        // Ally Action
        target.characterAction.Act(activeSkill.skillTargetActionType);


        activeSkill.Excute(this, target.gameObject);
    }

    public void CastToAllies(BaseSkill activeSkill, List<BaseEnemy> targets)
    {
        // Player Action
        EnemyManager.instance.activeCharacter.characterAction.Act(activeSkill.skillActionType);

        // Enemy Action
        foreach (var t in targets)
        {
            if (t != EnemyManager.instance.activeCharacter)
            {
                t.characterAction.Act(activeSkill.skillTargetActionType);
                activeSkill.Excute(this, t.gameObject);
            }
        }
    }

    public override void CastToRandomTarget(BaseSkill activeSkill)
    {
        Debug.Log("Implement later on");
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
    }

    protected override IEnumerator MentalEffectRoutine(Actor attacker, bool hasMentalEffect, int mentalDMG)
    {
        yield return base.MentalEffectRoutine(attacker, hasMentalEffect, mentalDMG);

        // Critical Check
        if(this.onCrit)
        {
            var character = (BaseCharacter)attacker;

            if (character.IsVirtuous)
            {
                character.speaker.FixedSpeak("Attack~~~!!!");
                while (Commander.instance.IsSpeaking)
                {
                    yield return null;
                }
                foreach (var other in PlayerManager.instance.characterList)
                {
                    if(!other.IsAfflicted)
                    {
                        other.TakeMentalHeal(4);
                    }
                }
                while (character.DoingMentalAction)
                {
                    yield return null;
                }
            }
            else
            {
                foreach (var another in PlayerManager.instance.characterList)
                {
                    if (another != character && another.IsAfflicted)
                    {
                        //another.speaker.FixedSpeak("You were just lucky...We are all going to die anyway....");
                        //while(Commander.instance.IsSpeaking)
                        //{
                        //    yield return null;
                        //}
                        character.TakeMentalDamage(another.onDodMentalDamage);
                        break;
                    }
                }
                while (character.DoingMentalAction)
                {
                    yield return null;
                }
            }

            yield break;
        }

        //Dodge Check

        if(this.onDodge)
        {
            var character = (BaseCharacter)attacker;

            foreach(var another in PlayerManager.instance.characterList)
            {
                if(another != character && !another.IsVirtuous)
                {
                    //another.speaker.FixedSpeak("What?! Missed?!...Stupid...!!!");
                    //while (Commander.instance.IsSpeaking)
                    //{
                    //    yield return null;
                    //}
                    character.TakeMentalDamage(another.onDodMentalDamage);
                    break;
                }
            }
            while(character.DoingMentalAction)
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(1f);
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
            Commander.instance.turnStateMachine.IsSkipTurn = true;
            return;
        }

        int rand = Random.Range(0, candidates.Count);

        this.activeCommand = candidates[rand];
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
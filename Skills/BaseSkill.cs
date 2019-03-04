using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillType // Type of trigger to excute skill
{
    SingleTarget,
    MultipleTarget
}

// Battle related actions only
public enum ActionType
{
    MainAttack,
    SubAttack,
    Buff,
    Hit,
    CriticalHit,
    Pick,
    Dodge,
    Buffed // When receive buff
}

public enum SkillRange // Whether it is selectable or not
{
    Unfriendly,
    Friendly,
    Self // Randomly target one of the detonated targets
}

[System.Serializable]
public class BaseSkill : MonoBehaviour
{
    public string skillName;
    public string skillDescription;
    [Range(1,5)]
    public int level;
    public Sprite skillIcon;
    public Actor.Job belongTo;
    public Actor owner;
    public GameObject[] effectsPrefab; // needs assigned when we create the skill

    // Unique Mechanism
    public SkillType skillType;
    public SkillRange skillRange;
    public ActionType skillActionType;
    public ActionType skillTargetActionType = ActionType.Hit;

    public bool[] castPositions = new bool[4];
    public bool[] targetPositions = new bool[4];

    // Basic Paramater modifiers
    public float accMode;
    public float critMode;
    public int dmgMode;

    public Actor targetActor;
    public float delay = 2f;
    public bool canCrit = true;
    public bool canDodge = true;

    float increaseHitChance = 0.1f;


    // Public method to set the values in the skill ui
    public virtual void SetValues(GameObject skillDisplayObject)
    {
        if(skillDisplayObject)
        {
            SkillDisplay SD = skillDisplayObject.GetComponent<SkillDisplay>();
            SD.thisSkillName.text = skillName;
            SD.thisSkillDescription.text = skillDescription;
            SD.thisSkillLevel.text = level.ToString();
            SD.thisSkillIcon.sprite = skillIcon;
        }

    }

    public virtual void Excute(Actor attacker, GameObject target)
    {

        if(target.tag == "Enemy")
        {
            targetActor = target.GetComponent<BaseEnemy>();
        }
        else
        {
            targetActor = target.GetComponent<BaseCharacter>();
        }

        // Dodge Check
        if (this.canDodge)
        {
            var hitChance = Mathf.Clamp(attacker.Accuracy + accMode - targetActor.Dodge, 0f, 1f);

            if(targetActor.onDodge)
            {
                hitChance = Mathf.Clamp(hitChance + increaseHitChance, 0f, 1f);
            }

            float rand = Random.Range(0, 1f);


            targetActor.onCrit = false;
            targetActor.onDodge = false;

            if (rand <= hitChance)
            {
                // Critical Check
                float critRoll = Random.Range(0f, 1f);
                var critChance = attacker.Critical + this.critMode;

                if (this.canCrit && critRoll <= critChance)
                {
                    // Critical!
                    targetActor.onCrit = true;
                    targetActor.characterAction.Act(ActionType.CriticalHit);
                }
                else
                {
                    targetActor.onCrit = false;
                    targetActor.characterAction.Act(this.skillTargetActionType);
                }
            }
            else
            {
                // Dodge!
                targetActor.onDodge = true;

                targetActor.characterAction.Act(ActionType.Dodge);
                UIManager.instance.CreateEffect("Dodge", targetActor, 0);
            }
        }
        else
        {
            // Critical Check
            float critRoll = Random.Range(0f, 1f);
            var critChance = attacker.Critical + this.critMode;

            if (this.canCrit && critRoll <= critChance)
            {
                // Critical!
                targetActor.onCrit = true;
                targetActor.characterAction.Act(ActionType.CriticalHit);
            }
            else
            {
                targetActor.onCrit = false;
                targetActor.characterAction.Act(this.skillTargetActionType);
            }
        }

        GenerateSound(attacker);
    }

    void GenerateSound(Actor attacker)
    {
        if(attacker is BaseCharacter || attacker.gameObject.tag == "Survivor")
        {
            switch(skillActionType)
            {
                case ActionType.MainAttack:
                    int rand = Random.Range(5, 7);
                    SoundManager.Instance.PlaySE(rand);
                    break;
                case ActionType.SubAttack:
                    SoundManager.Instance.PlaySE(17);
                    break;
                case ActionType.Buff:
                    SoundManager.Instance.PlaySE(11);
                    break;
                default:
                    SoundManager.Instance.PlaySE(17);
                    break;
            }
        }
        else if (attacker is BaseEnemy || attacker.gameObject.tag == "Enemy")
        {
            int randE = Random.Range(15, 17);
            SoundManager.Instance.PlaySE(randE);
        }
    }
}
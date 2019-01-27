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
    Idle // None
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
    public GameObject[] bloodEffects; // needs assigned when we create the skill

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

    public bool canCrit = false; // Whether it can crit or not
    public List<SkillEffect> effects = new List<SkillEffect>();

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

    public virtual void Excute(GameObject target)
    {
        Debug.Log("Skill Generated");
        // Instantiate common damage effects

        if(target.tag == "Enemy")
        {
            targetActor = target.GetComponent<BaseEnemy>();
        }
        else
        {
            targetActor = target.GetComponent<BaseCharacter>();
        }
    }
}
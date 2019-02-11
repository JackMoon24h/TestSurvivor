using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour 
{
    // Owned Skills
    public List<BaseSkill> skillList = new List<BaseSkill>(4);

    public BaseSkill GetSkill(int skillNum)
    {
        return skillList[skillNum - 1];
    }
}

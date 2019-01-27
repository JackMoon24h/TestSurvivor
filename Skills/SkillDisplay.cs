using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDisplay : MonoBehaviour 
{
    public int thisSkillNumber;
    public BaseCharacter thisCharacter;
    public BaseSkill thisSkill;
    public Text thisSkillName;
    public Text thisSkillDescription;
    public Image thisSkillIcon;
    public Text thisSkillLevel;

    // Set from inspector
    public Image[] thisCastPos = new Image[4];
    public Color canCastColor;
    public Color cannotCastColor;

    public Image[] thisTargetPos = new Image[4];
    public Color canTargetColor;
    public Color cannotTargetColor;

    public Button btn;

    private bool m_isAvailable;
    public bool IsAvailable { get{return m_isAvailable; } set{ m_isAvailable = value;}}


    private void Awake()
    {
        thisSkillName = this.gameObject.GetComponentInChildren<Text>();
        thisSkillIcon = this.gameObject.GetComponent<Image>();
        btn = GetComponent<Button>();
    }

    private void Start()
    {
        btn.onClick.AddListener(OnClickEvent);
    }

    public void SetSkillInfo(int num)
    {
        this.thisSkillNumber = num;
    }

    public void UpdateSkillInfo(BaseCharacter updateTarget)
    {
        thisSkill = updateTarget.skillManager.GetSkill(thisSkillNumber);

        thisSkillName.text = thisSkill.skillName;
        thisSkillIcon.sprite = thisSkill.skillIcon;
        //thisSkillDescription.text = thisSkill.skillDescription;
        //thisSkillLevel.text = thisSkill.level.ToString();
        SetCastPosUI();
        SetTargetPosUI();

        if (Commander.instance.IsBattle)
        {
            SetAvailableSkills(updateTarget);
        }
    }

    void SetCastPosUI()
    {
        for (int i = 0; i < thisSkill.castPositions.Length; i++)
        {
            if (thisSkill.castPositions[i])
            {
                thisCastPos[i].color = canCastColor;
            }
            else
            {
                thisCastPos[i].color = cannotCastColor;
            }
        }
    }

    void SetTargetPosUI()
    {
        for (int i = 0; i < thisSkill.targetPositions.Length; i++)
        {
            if (thisSkill.targetPositions[i])
            {
                thisTargetPos[i].color = canTargetColor;
            }
            else
            {
                thisTargetPos[i].color = cannotTargetColor;
            }
        }
    }

    // Called when the command btn is clickeda
    public void OnClickEvent()
    {
        if(m_isAvailable)
        {
            // DrawTargets
            PlayerManager.instance.DrawTargets(thisSkill);
            PlayerManager.instance.activeCharacter.activeCommand = thisSkill;
        }
    }

    public void SetAvailableSkills(BaseCharacter target)
    {

        // First, caster's position
        if (thisSkill.castPositions[target.Position - 1])
        {
            // Secondly, check targets' positions

            switch (thisSkill.skillRange)
            {
                case SkillRange.Unfriendly:
                    int temp = 0;
                    // EnemyManager.instance.characterist can have 1~4 objects
                    // Which menas, if i = 4, EnemyManager.instance.characterist[i = 4] is an error * Argumant out of range
                    for (int i = 0; i < EnemyManager.instance.characterList.Count; i++)
                    {
                        if(thisSkill.targetPositions[i])
                        {
                            temp += 1;
                        }
                    }

                    if(temp > 0)
                    {
                        m_isAvailable = true;
                    }
                    else
                    {
                        m_isAvailable = false;
                    }

                    break;

                case SkillRange.Friendly:
                    m_isAvailable = true;
                    break;

                case SkillRange.Self:
                    m_isAvailable = true;
                    break;

                default:
                    m_isAvailable = false;
                    break;
            }
        }
        else
        {
            m_isAvailable = false;
        }

        btn.interactable = m_isAvailable;
    }

}

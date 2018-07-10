using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTarget : MonoBehaviour 
{
    GameManager gameManager;
    SquadManager squadManager;
    EnemySquadManager enemySquadManager;
    Character character;

    public enum Target
    {
        ENEMY,
        TEAM,
        SELF
    }

    public enum TargetType
    {
        SELECTABLE,
        FIXED
    }

    public bool[] availablePos = new bool[4];
    public bool[] targetRange = new bool[4];

    public Target target;

    private void Start()
    {
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        squadManager = Object.FindObjectOfType<SquadManager>().GetComponent<SquadManager>();
    }

    public void DrawTarget()
    {
        if(CanCast())
        {
            switch(target)
            {
                case Target.ENEMY:
                    // Enemies's target cursors should be shown if they are alive
                    for (int i = 0; i < targetRange.Length; i++)
                    {
                        if(targetRange[i])
                        {
                            enemySquadManager.enemySquadPositions.GetCharacterAtPos(i + 1);
                            // SetActive (false) to target cursor!==========================


                        }

                    }

                    // Give them to gameManager

                    // Swift to next routine : wait for confirm order
                    break;

                case Target.TEAM:
                    // Team's target cursors should be shown if they are alive

                    // Give them to gameManager

                    // Swift to next routine : wait for confirm order
                    break;

                case Target.SELF:
                    // Self's target cursors should be shown

                    // Give them to gameManager

                    // Swift to next routine : wait for confirm order
                    break;
            }

        }

        // Go to next step : wait for comfirm action
    }

    // Check the character's currentPosition if character can cast the selected skill
    public bool CanCast()
    {
        if (!gameManager.IsBattle)
        { 
            return false;
        }

        if (!character.isActive)
        {
            return false;
        }


        for (int i = 0; i < availablePos.Length; i++)
        {
            if (availablePos[character.currentPosition - 1])
            {
                return true;
            }
        }

        return false;
    }
}

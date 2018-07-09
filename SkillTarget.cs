using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTarget : MonoBehaviour 
{
    SquadManager squadManager;
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


    private void Start()
    {
        squadManager = Object.FindObjectOfType<SquadManager>().GetComponent<SquadManager>();
    }

    public void DrawTarget()
    {
        if(CanCast())
        {
            // Show Cursor to target base on the Target enum

        }

        // Go to next step : wait for comfirm action
    }

    // Check the character's currentPosition if character can cast the selected skill
    public bool CanCast()
    {
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

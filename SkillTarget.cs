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

    public bool[] selectableRange = new bool[4];
    public bool[] fixedRange = new bool[4];
    public bool[] availablePos = new bool[4];

    private void Start()
    {
        squadManager = Object.FindObjectOfType<SquadManager>().GetComponent<SquadManager>();
    }

    public void DrawTarget()
    {
        // Check the character's currentPosition. If character is positioned at availablePos

        // Check it's Target
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTarget : MonoBehaviour 
{
    GameManager gameManager;
    SquadManager squadManager;
    Skill skillManager;
    EnemySquadManager enemySquadManager;

    public enum Target
    {
        ENEMY,
        TEAM,
        SELF
    }
    public Target target;

    public enum TargetType
    {
        SELECTABLE,
        FIXED
    }
    public TargetType targetType;

    public bool[] availablePos = new bool[4];
    public bool[] targetRange = new bool[4];
    public List<GameObject> actualTargets = new List<GameObject>();


    private void Start()
    {
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        squadManager = Object.FindObjectOfType<SquadManager>().GetComponent<SquadManager>();
        skillManager = GetComponent<Skill>();
    }

    // When skill button is clicked, this function will be called
    public void DrawTarget()
    {
        if (gameManager.turnStep == GameManager.TurnStep.ConfirmCommand)
        {
            ResetDraw();
        }
        actualTargets.Clear();

        if(!CanCast())
        {
            Debug.Log("Cannot Cast");
            return;
        }

        gameManager.commandBtn.SetActive(true);

        gameManager.turnStep = GameManager.TurnStep.ConfirmCommand;

        switch (target)
        {
            case Target.ENEMY:
                // Enemies's target cursors should be shown if they are alive

                var list = gameManager.enemySquadManager.GetCurrentEnemyList();

                foreach (var t in list)
                {
                    t.targetCursor.SetActive(false);
                }

                for (int i = 0; i < targetRange.Length; i++)
                {
                    if (targetRange[i])
                    {
                        var tar = gameManager.enemySquadManager.enemySquadPositions.GetEnemyAtPos(i + 1);
                        tar.targetCursor.SetActive(true);
                        actualTargets.Add(tar.gameObject);
                    }
                }
                break;

            case Target.TEAM:
                // Team's target cursors should be shown if they are alive

                break;

            case Target.SELF:
                
                // Self's target cursors should be shown
                break;
        }

        // Give actual targets to gameManager
        gameManager.SetAction(skillManager.character, skillManager, actualTargets);


        // Go to next step : wait for comfirm action

    }

    // Check the character's currentPosition if character can cast the selected skill

    bool CanCast()
    {
        if (!gameManager.IsBattle)
        { 
            // Show Skill Information
            return false;
        }

        if (!skillManager.character.isActive)
        {
            // Show Skill Information
            return false;
        }

        if(gameManager.turnState == GameManager.TurnState.Player)
        {
            for (int i = 0; i < availablePos.Length; i++)
            {
                if (availablePos[skillManager.character.currentPosition - 1])
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void ResetDraw()
    {
        foreach(var t in actualTargets)
        {
            t.GetComponent<Enemies>().targetCursor.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour 
{
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
	public List<Unit> castTargets = new List<Unit>();

    // Reference
    Overseer overseer;
    Deck playerDeck;
    Unit unit;

    // Sprites
    public Sprite abilitySprite;

	// Use this for initialization
	void Awake () 
	{
        overseer = Object.FindObjectOfType<Overseer>().GetComponent<Overseer>();
        playerDeck = Object.FindObjectOfType<Deck>().GetComponent<Deck>();
        unit = this.transform.parent.gameObject.GetComponent<Unit>();
	}
	
    bool CanCast()
    {
        if (!overseer.IsBattle)
        {
            // Show Skill Information
            return false;
        }

        if (!unit.isActive)
        {
            // Show Skill Information
            return false;
        }

        for (int i = 0; i < availablePos.Length; i++)
        {
            if (availablePos[unit.currentPosition - 1])
            {
                return true;
            }
        }

        return false;
    }

    public void DrawTarget()
    {
        if (overseer.turnStep == Overseer.TurnStep.ConfirmCommand)
        {
            this.ResetDraw();
        }

        castTargets.Clear();

        if (!CanCast())
        {
            Debug.Log("Cannot Cast");
            return;
        }

        switch (target)
        {
            case Target.ENEMY:
                // target's cursors should be shown if they are alive
                for (int i = 0; i < targetRange.Length; i++)
                {
                    if (targetRange[i])
                    {
                        var tar =playerDeck.GetUnitByPosition(i + 1);
                        tar.SetAsTarget(true);
                        castTargets.Add(tar);
                    }
                }

                break;

            case Target.TEAM:
                // target's cursors should be shown if they are alive

                break;

            case Target.SELF:

                // target's cursors should be shown
                break;
        }

        overseer.turnStep = Overseer.TurnStep.ConfirmCommand;

        //overseer.commandBtn.SetActive(true);

        // Let overseer know the cast targets

        // Go to next step : wait for comfirm action

    }

    public void ResetDraw()
    {
        foreach (var t in castTargets)
        {
            t.SetAsTarget(false);
            overseer.turnStep = Overseer.TurnStep.ChooseCommand;
        }
    }
}

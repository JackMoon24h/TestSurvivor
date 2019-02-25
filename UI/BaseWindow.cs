using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseWindow : MonoBehaviour 
{
    public Button closeBtn;
    public Vector3 spawnPosition = new Vector3(180f, 40f, 0f);

    protected virtual bool CanOpen()
    {
        if (UIManager.instance.rewardWindow.activeInHierarchy)
        {
            return false;
        }

        if (Commander.instance.IsBattle)
        {
            if (Commander.instance.turnStateMachine.currentTurn == TurnStateMachine.Turn.PLAYER && Commander.instance.turnStateMachine.currentTurnState == TurnStateMachine.TurnState.WaitForCommand)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public virtual void CloseWindow()
    {
        StartCoroutine(CloseWindowRoutine());
    }

    IEnumerator CloseWindowRoutine()
    {
        yield return new WaitForSeconds(0.25f);

        UIManager.instance.EndUIShield();
        Commander.instance.touchInput.InputEnabled = true;
        PlayerManager.instance.playerInput.InputEnabled = true;
        this.gameObject.SetActive(false);
    }

    public virtual void OpenWindow()
    {
        UIManager.instance.BeginUIShield();
        Commander.instance.touchInput.InputEnabled = false;
        PlayerManager.instance.playerInput.InputEnabled = false;
        this.gameObject.SetActive(true);
    }
}

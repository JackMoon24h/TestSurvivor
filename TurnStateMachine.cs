using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnStateMachine : MonoBehaviour 
{
    public enum Turn
    {
        PLAYER,
        ENEMY
    }
    public Turn currentTurn;

    public enum TurnState
    {
        SetActiveUnit, // Choose player or enemy character
        WaitForCommand, // Player must select a skill command to use
        ConfirmTarget, // Confirm targets
        DoAction, // Actors act and calculate damage, Check win, lose, and judgement condition
        HandleEffects, // Calculates various effects
        JudgeMental, // Test mental level
        FinishTurn // Finish turn
    }
    public TurnState currentTurnState;

    [Range(1, 9999)]
    [SerializeField]
    private int m_round = 1;
    public int Round { get{return m_round;} set{m_round = value;}}

    [Range(1, 8)]
    [SerializeField]
    private int m_turnCount = 1;
    public int TurnCount { get{return m_turnCount;} set{m_turnCount = value;}}

    int pSpeedSum;
    int eSpeedSum;

    bool m_hasConfirmedCommand;
    public bool HasConfirmedCommand { get { return m_hasConfirmedCommand; } set { m_hasConfirmedCommand = value; } }

    bool m_hasHandledEffects;
    public bool HasHandledEffects { get { return m_hasHandledEffects; } set { m_hasHandledEffects = value; } }



    // Use this for initialization
    void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    // Do UI related things
    void CheckState()
    {
        switch(currentTurnState)
        {
            case TurnState.SetActiveUnit:
                break;
            case TurnState.WaitForCommand:
                break;
            case TurnState.ConfirmTarget:
                break;
            case TurnState.DoAction:
                break;
            case TurnState.HandleEffects:
                break;
            case TurnState.JudgeMental:
                break;
            case TurnState.FinishTurn:
                break;
        }
    }


    public void Initialize()
    {
        currentTurnState = TurnState.SetActiveUnit;
        m_round = 1;
        m_turnCount = 1;

        pSpeedSum = 0;
        eSpeedSum = 0;

        for(int i = 0; i < PlayerManager.instance.characterList.Count; i++)
        {
            pSpeedSum += PlayerManager.instance.characterList[i].Speed;
        }

        for (int i = 0; i < EnemyManager.instance.characterList.Count; i++)
        {
            eSpeedSum += EnemyManager.instance.characterList[i].Speed;
        }

        if(pSpeedSum >= eSpeedSum)
        {
            // Player's turn
            currentTurn = Turn.PLAYER;
            var randPos = Random.Range(1, PlayerManager.instance.characterList.Count + 1);
            PlayerManager.instance.SetActiveCharacterAtPos(randPos);
        }
        else
        {
            // Enemy's turn
            currentTurn = Turn.ENEMY;
            var randPos = Random.Range(1, EnemyManager.instance.characterList.Count + 1);
            EnemyManager.instance.SetActiveCharacterAtPos(randPos);
        }
    }

    public IEnumerator TurnRoutine()
    {
        yield return new WaitForSeconds(1f);
    }



    public IEnumerator StartTurnRoutine()
    {
        Debug.Log("StartTurnRoutine");

        // For Debugging purpose
        currentTurn = Turn.PLAYER;
        var randPos = Random.Range(1, PlayerManager.instance.characterList.Count + 1);
        PlayerManager.instance.SetActiveCharacterAtPos(randPos);

        if (currentTurn == Turn.PLAYER && PlayerManager.instance.activeCharacter)
        {
            currentTurnState = TurnState.WaitForCommand;
            PlayerManager.instance.activeCharacter.characterAction.ReadyAction();

        }
        else if (currentTurn == Turn.ENEMY && EnemyManager.instance.activeCharacter)
        {
            Debug.Log("Enemy's turn");
            // Excute AI Play
        }
        else
        {
            Debug.LogWarning("No active character found");
        }


        yield return new WaitForSeconds(0.5f);

        // touchInput should be enabled to true after UI panel has been updated (MUST). Because UI panel should set available skills while players cannot touch each skill
        UIManager.instance.EndUIShield();
        Commander.instance.touchInput.InputEnabled = true;

        // Player must choose a command
        // Player can change its command before confirming targets
        while(!m_hasConfirmedCommand)
        {
            yield return null;
        }
        Commander.instance.turnStateMachine.currentTurnState = TurnState.DoAction;
    }

    public IEnumerator UpdateTurnRoutine()
    {
        Debug.Log("UpdateTurnRoutine");
        if (Commander.instance.actionEvent != null)
        {
            Commander.instance.actionEvent.Invoke();
            // Sounds
        }

        while(Commander.instance.IsActing)
        {
            // Wait until the action is over
            yield return null;
        }

        currentTurnState = TurnState.HandleEffects;

        // Show Damage or Effect animations

        while(!m_hasHandledEffects)
        {
            // For Test
            yield return new WaitForSeconds(0.3f);
            m_hasHandledEffects = true;
        }

    }

    public IEnumerator EndTurnRoutine()
    {
        Debug.Log("EndTurnRoutine");
        currentTurnState = TurnState.FinishTurn;
        yield return new WaitForSeconds(1f);

        // Initialize variables for the next turn
        m_hasHandledEffects = false;
        m_hasConfirmedCommand = false;

        m_turnCount++;
        UIManager.instance.EndUIShield();
    }
}

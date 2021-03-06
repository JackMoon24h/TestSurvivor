﻿using System.Collections;
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

    [Range(1, 9999)]
    [SerializeField]
    private int m_turnCount = 1;
    public int TurnCount { get{return m_turnCount;} set{m_turnCount = value;}}

    private int m_turnsInRound;
    public int TurnsInRound { get { return m_turnsInRound; } set { m_turnsInRound = value; } }

    private List<int> m_initialSPDmode = new List<int>(){ 1, 2, 3, 4, 5, 6, 7, 8 };
    public List<int> InitialSPDmode { get { return m_initialSPDmode; } }

    public List<Actor> queue = new List<Actor>();

    bool m_isSkipTurn;
    public bool IsSkipTurn { get { return m_isSkipTurn; } set { m_isSkipTurn = value; } }

    bool m_hasConfirmedCommand;
    public bool HasConfirmedCommand { get { return m_hasConfirmedCommand; } set { m_hasConfirmedCommand = value; } }

    bool m_hasHandledEffects;
    public bool HasHandledEffects { get { return m_hasHandledEffects; } set { m_hasHandledEffects = value; } }


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
        Commander.instance.GetCurrentActors();
        currentTurnState = TurnState.SetActiveUnit;
        m_round = 1;
        m_turnCount = 1;

        // Set Queue for round 1
        queue = SetQueue(Commander.instance.actorList);
        m_turnsInRound = queue.Count;
    }

    // All current actors can act 1 time per 1 round
    public IEnumerator RoundRoutine()
    {
        yield return StartCoroutine(StartTurnRoutine()); // User Input or Enemy AI selection
        yield return StartCoroutine(UpdateTurnRoutine()); // Action
        yield return StartCoroutine(EndTurnRoutine()); // Calculation
    }



    IEnumerator StartTurnRoutine()
    {
        if(queue[m_turnCount - 1] == null)
        {
            m_isSkipTurn = true;
            yield break;
        }

        // If it is dead then skip turn
        //while(queue[m_turnCount - 1] == null)
        //{
        //    yield return new WaitForSeconds(0.5f);
        //    GetNextTurn();

        //}
        Debug.Log("StartTurnRoutine : Turn / Round " + m_turnCount + " / " + m_round);

        // Get Active Character from queue
        if (queue[m_turnCount - 1].gameObject.tag == "Enemy")
        {
            // Enemy turn
            currentTurn = Turn.ENEMY;
            currentTurnState = TurnState.WaitForCommand;
            var enemy = (BaseEnemy)queue[m_turnCount - 1];

            enemy.OnTurnStart();

            if (enemy == null || enemy.isDead)
            {
                m_isSkipTurn = true;
                yield break;
            }

            enemy.characterAction.ReadyAction();
            EnemyManager.instance.SetActiveCharacter(enemy);

            while (!enemy.IsSubActionOver)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            EnemyManager.instance.PlayTurn();
        }
        else
        {
            // Player turn
            currentTurn = Turn.PLAYER;
            currentTurnState = TurnState.WaitForCommand;
            var player = (BaseCharacter)queue[m_turnCount - 1];


            player.OnTurnStart();

            if(m_isSkipTurn)
            {
                yield break;
            }

            player.characterAction.ReadyAction();
            PlayerManager.instance.SetActiveCharacter(player);

            if (player == null || player.isDead)
            {
                m_isSkipTurn = true;
            }

            while (!player.IsSubActionOver)
            {
                yield return null;
            }

            // If it is the last one standing in the deck then check if it has available skill
            if(PlayerManager.instance.characterList.Count == 1 && UIManager.instance.availableSkNum == 0)
            {
                m_isSkipTurn = true;
            }
        }

        if (m_isSkipTurn)
        {
            Commander.instance.narrator.Narrate("Skip Turn!!");
            while(Commander.instance.narrator.IsNarrating)
            {
                yield return null;
            }
            yield break;
        }

        yield return new WaitForSeconds(0.6f);

        // touchInput should be enabled to true after UI panel has been updated (MUST). Because UI panel should set available skills while players cannot touch each skill
        UIManager.instance.EndUIShield();
        Commander.instance.touchInput.InputEnabled = true;

        // Player must choose a command and confirm the target in order to breka the while loop below
        // Player can change its command before confirming targets
        while(!m_hasConfirmedCommand)
        {
            yield return null;
            if(m_isSkipTurn)
            {
                yield break;
            }
        }
        UIManager.instance.CloseSkillInfo();
        UIManager.instance.BeginUIShield();
        Commander.instance.turnStateMachine.currentTurnState = TurnState.DoAction;
    }

    IEnumerator UpdateTurnRoutine()
    {
        if(m_isSkipTurn)
        {
            yield break;
        }

        Debug.Log("UpdateTurnRoutine");
        if (Commander.instance.actionEvent != null)
        {
            Commander.instance.actionEvent.Invoke();
            // Sounds
        }

        while(Commander.instance.IsActing)
        {
            // Wait until main action is over
            yield return null;
        }
    }

    IEnumerator EndTurnRoutine()
    {
        Debug.Log("EndTurnRoutine");
        currentTurnState = TurnState.HandleEffects;

        // Show Damage or Effect animations

        while (!m_hasHandledEffects)
        {
            yield return null;
            m_hasHandledEffects = HasAllActorsDone();
        }

        if(!m_isSkipTurn)
        {
            yield return new WaitForSeconds(0.3f);
        }

        // Mental Suffering

        currentTurnState = TurnState.FinishTurn;
        if(!m_isSkipTurn)
        {
            yield return new WaitForSeconds(0.6f);
        }

        // Initialize variables for the next turn
        m_hasHandledEffects = false;
        m_hasConfirmedCommand = false;

        Debug.Log("Enemy has " + EnemyManager.instance.characterList.Count + " units left");
        Debug.Log("Player has " + PlayerManager.instance.characterList.Count + " units left");

        // Check if it meets battle over conditions after every turn

        if (Commander.instance.AreEnemiesAllDead())
        {
            // Won. Stop coroutine and finish battle.
            Commander.instance.IsBattle = false;
            yield break;

        }
        else if (Commander.instance.AreCharactersAllDead())
        {
            // Lose.
            Commander.instance.IsBattle = false;
            PlayerManager.instance.allDead = true;
            Commander.instance.LoseLevel();
            yield break;
        }
        Debug.Log("Win or Lose conditions are not met");

        m_turnCount++;
        m_turnsInRound = queue.Count;
        m_isSkipTurn = false;

        if(m_turnCount > m_turnsInRound)
        {
            m_turnCount = 1;
            m_round++;
            UpdateRound();
        }

        //UIManager.instance.EndUIShield();
        currentTurnState = TurnState.SetActiveUnit;
    }

    void UpdateRound()
    {
        Commander.instance.GetCurrentActors();

        // Set Queue for next round
        queue = SetQueue(Commander.instance.actorList);
    }

    // It sorts given 2 lists
    public static void DarkSort(List<int> spdList, List<Actor> aList)
    {
        int n = spdList.Count;

        for(int i = 0; i < n - 1; i++)
        {
            for(int j = 0; j < n - i - 1; j ++)
            {
                if(spdList[j] < spdList[j + 1])
                {
                    // Swap
                    int temp = spdList[j];
                    spdList[j] = spdList[j + 1];
                    spdList[j + 1] = temp;

                    // If swap spdArray, then swap actor arrays too
                    Actor tempActor = aList[j];
                    aList[j] = aList[j + 1];
                    aList[j + 1] = tempActor;
                }
            }
        }
    }

    public List<Actor> SetQueue(List<Actor> aList)
    {
        List<int> spdList = new List<int>();

        // First Round has special queue mode
        if(m_round == 1)
        {
            InitialSPDmode.Shuffle();
            for (int i = 0; i < aList.Count; i++)
            {
                spdList.Add(aList[i].Speed + InitialSPDmode[i]);
            }
            DarkSort(spdList, aList);

            return aList;
        }

        for (int i = 0; i < aList.Count; i++)
        {
            spdList.Add(aList[i].Speed);
        }

        DarkSort(spdList, aList);

        return aList;
    }

    public void GetNextTurn()
    {
        Debug.Log("Get Next Turn");
        this.m_turnCount++;

        m_turnsInRound = queue.Count;

        if (m_turnCount > m_turnsInRound)
        {
            m_turnCount = 1;
            m_round++;
            UpdateRound();
        }
    }

    bool HasAllActorsDone()
    {
        bool result = true;
        foreach(var t in queue)
        {
            if(t != null && !t.IsSubActionOver)
            {
                result = false;
                break;
            }
        }
        return result;
    }

    public bool IsStillMentalActing()
    {
        bool result = false;
        foreach(var t in PlayerManager.instance.characterList)
        {
            if(t != null && t.DoingMentalAction)
            {
                result = true;
                break;
            }
        }
        return result;
    }
}

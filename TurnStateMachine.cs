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

    [Range(1, 9999)]
    [SerializeField]
    private int m_turnCount = 1;
    public int TurnCount { get{return m_turnCount;} set{m_turnCount = value;}}

    private int m_turnsInRound;
    public int TurnsInRound { get { return m_turnsInRound; } set { m_turnsInRound = value; } }

    private List<int> m_initialSPDmode = new List<int>(){ 1, 2, 3, 4, 5, 6, 7, 8 };
    public List<int> InitialSPDmode { get { return m_initialSPDmode; } }

    public List<Actor> queue = new List<Actor>();

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

        // For debugging purpose. Can be deleted later on.
        UIManager.instance.SetEnemyListtUI();
        UIManager.instance.SetPlayerListUI();
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
        Debug.Log("StartTurnRoutine : Turn / Round " + m_turnCount + " / " + m_round);

        // Get Active Character from queue
        if(queue[m_turnCount - 1].gameObject.tag == "Enemy")
        {
            // Enemy turn
            currentTurn = Turn.ENEMY;
            var enemy = (BaseEnemy)queue[m_turnCount - 1];
            enemy.characterAction.ReadyAction();
            EnemyManager.instance.SetActiveCharacter(enemy);

            currentTurnState = TurnState.WaitForCommand;
            yield return new WaitForSeconds(0.5f);

            EnemyManager.instance.PlayTurn();
        }
        else
        {
            // Player turn
            currentTurn = Turn.PLAYER;
            var player = (BaseCharacter)queue[m_turnCount - 1];
            player.characterAction.ReadyAction();
            PlayerManager.instance.SetActiveCharacter(player);
            currentTurnState = TurnState.WaitForCommand;
        }

        yield return new WaitForSeconds(0.5f);

        // touchInput should be enabled to true after UI panel has been updated (MUST). Because UI panel should set available skills while players cannot touch each skill
        UIManager.instance.EndUIShield();
        Commander.instance.touchInput.InputEnabled = true;

        // Player must choose a command and confirm the target in order to breka the while loop below
        // Player can change its command before confirming targets
        while(!m_hasConfirmedCommand)
        {
            yield return null;
        }
        UIManager.instance.BeginUIShield();
        Commander.instance.turnStateMachine.currentTurnState = TurnState.DoAction;
    }

    IEnumerator UpdateTurnRoutine()
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

        // For debugging purpose. Can be deleted later on.
        UIManager.instance.SetEnemyListtUI();
        UIManager.instance.SetPlayerListUI();
    }

    IEnumerator EndTurnRoutine()
    {
        Debug.Log("EndTurnRoutine");
        currentTurnState = TurnState.FinishTurn;
        yield return new WaitForSeconds(1f);

        // Initialize variables for the next turn
        m_hasHandledEffects = false;
        m_hasConfirmedCommand = false;

        Debug.Log("Enemy has " + EnemyManager.instance.characterList.Count + " units left");
        Debug.Log("Player has " + PlayerManager.instance.characterList.Count + " units left");

        // Check if it meets battle over conditions after every turn

        if (currentTurn == Turn.PLAYER && Commander.instance.AreEnemiesAllDead())
        {
            // Won. Stop coroutine and finish battle.
            Commander.instance.IsBattle = false;
            yield break;
        }
        else if (currentTurn == Turn.ENEMY && Commander.instance.AreCharactersAllDead())
        {
            // Lose.
            Commander.instance.LoseLevel();
        }

        m_turnCount++;
        m_turnsInRound = queue.Count;

        if(m_turnCount > m_turnsInRound)
        {
            m_turnCount = 1;
            m_round++;
            UpdateRound();
        }

        UIManager.instance.EndUIShield();
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
}

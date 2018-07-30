using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{

    public SquadManager squadManager;
    public EnemySquadManager enemySquadManager;
    public ObjectTrigger goal;
    public Camera mainCamera;

    bool m_hasLevelStarted = false;
    public bool HasLevelStarted { get { return m_hasLevelStarted; } set { m_hasLevelStarted = value; } }

    bool m_isGamePlaying = false;
    public bool IsGamePlaying { get { return m_isGamePlaying; } set { m_isGamePlaying = value; } }

    bool m_isGameOver = false;
    public bool IsGameOver { get { return m_isGameOver; } set { m_isGameOver = value; } }

    bool m_isBattle = false;
    public bool IsBattle { get { return m_isBattle; } set { m_isBattle = value; } }

    bool m_hasLevelFinished = false;
    public bool HasLevelFinished { get { return m_hasLevelFinished; } set { m_hasLevelFinished = value; } }

    public float delay = 2.5f;

    bool m_isTurnComplete = false;
    public bool IsTurnComplete { get { return m_isTurnComplete; } set { m_isTurnComplete = value; }}

    bool m_isActing = false;
    public bool IsActing { get { return m_isActing; } set { m_isActing = value; } }

    public List<MainPanel> mainPanels = new List<MainPanel>();
    public GameObject commandBtn;
    public Narrator narrator;

    Vector3 enemyPositionOffset = new Vector3(0f, 0.25f, 0f);

    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent battleEvent;
    public UnityEvent battleOverEvent;
    public UnityEvent loseLevelEvent;
    public UnityEvent endLevelEvent;
    public UnityEvent narrationEvent;

    public enum TurnState
    {
        Player,
        Enemy
    }
    public TurnState turnState = TurnState.Player;

    public int turnCount;
    public TurnState previousTurnState;

    public enum TurnStep
    {
        WaitForCommand,
        ChooseCommand,
        ConfirmCommand,
        Act,
        FinishTurn
    }
    public TurnStep turnStep = TurnStep.WaitForCommand;

    public float skillAnimTime = 3.5f;

    // Skill Related
    public Character actor;
    public Skill activeSkill;
    public List<GameObject> activeTargets = new List<GameObject>();

    private void Awake()
    {
        squadManager = Object.FindObjectOfType<SquadManager>().GetComponent<SquadManager>();
        goal = GameObject.FindWithTag("Goal").GetComponent<ObjectTrigger>();
        mainPanels = (Object.FindObjectsOfType<MainPanel>() as MainPanel[]).ToList();
        mainCamera = Camera.main;
        narrator = GameObject.Find("Narration").GetComponent<Narrator>();

        for (int i = 0; i < 3; i++)
        {
            mainPanels[i].gameObject.SetActive(false);
        }

        commandBtn = GameObject.Find("CommandButton");
        commandBtn.SetActive(false);
    }

    // Use this for initialization
    void Start () 
    {
        if(squadManager != null)
        {
            StartCoroutine("RunGameLoop");
        }
        else
        {
            Debug.LogWarning("No Squad found!");
        }
	}

    public void UpdateNarration(string narration)
    {
        narrator.gameObject.GetComponent<Text>().text = narration;

        if (narrationEvent != null)
        {
            narrationEvent.Invoke();
        }
    }

    IEnumerator RunGameLoop()
    {
        yield return StartCoroutine("StartLevelRoutine");
        yield return StartCoroutine("PlayLevelRoutine");
        yield return StartCoroutine("EndLevelRoutine");
    }

    // By Clicking the start button, StartLevelRoutine will be over
    IEnumerator StartLevelRoutine()
    {
        Debug.Log("START LEVEL");
        squadManager.squadInput.InputEnabled = false;

        while(!m_hasLevelStarted)
        {
            yield return null;
        }

        if(startLevelEvent != null)
        {
            startLevelEvent.Invoke();
        }
    }

    IEnumerator PlayLevelRoutine()
    {
        Debug.Log("PLAY LEVEL");
        m_isGamePlaying = true;
        yield return new WaitForSeconds(delay);
        squadManager.squadInput.InputEnabled = true;

        if(playLevelEvent != null)
        {
            // Fade off start screens
            playLevelEvent.Invoke(); 
        }

        while(!m_isGameOver)
        {
            yield return null;

            m_isGameOver = IsWinner();

        }

        Debug.Log("WIN! ===================");
    }

    IEnumerator BattleLevelRoutine()
    {
        Debug.Log("Battle Routine Started!");

        squadManager.squadInput.InputEnabled = false;

        if(battleEvent != null)
        {
            // Battle Animations and start turn
            battleEvent.Invoke();
        }

        // Show Battle Animation for seconds
        yield return new WaitForSeconds(2f);

        squadManager.squadInput.InputEnabled = true;

        StartTurn();

        while(m_isBattle)
        {
            yield return null;
        }

        yield return new WaitForSeconds(delay);

        if(battleOverEvent != null)
        {
            battleOverEvent.Invoke();
        }

        squadManager.squadInput.InputEnabled = true;
        Debug.Log("BATTLE IS OVER");
    }


    IEnumerator EndLevelRoutine()
    {
        Debug.Log("END LEVEL");

        squadManager.squadInput.InputEnabled = false;

        if(endLevelEvent != null)
        {
            endLevelEvent.Invoke();
        }

        while(!m_hasLevelFinished)
        {
            yield return null;
        }

        RestartLevel();
    }

    void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }



    // attached to start button in order to start the level
    public void PlayLevel()
    {
        m_hasLevelStarted = true;
    }

    // Check if player has reached the goal
    bool IsWinner()
    {
        if(goal != null)
        {
            return goal.isActive;
        }
        Debug.LogWarning("Warning : Goal has not been set! ====================");
        return false;
    }

    // This will be called by triggers on the field
    public void InitBattle(GameObject enemySquadPrefab)
    {
        Debug.Log("BATTLE LEVEL");
        m_isBattle = true;
        squadManager.squadMover.Stop();

        var enemySquad = Instantiate(enemySquadPrefab, squadManager.gameObject.transform.position + enemyPositionOffset, Quaternion.identity);
        enemySquadManager = enemySquad.GetComponent<EnemySquadManager>();

        enemySquadManager.GetCurrentEnemyList();
        squadManager.GetCurrentCharacterList();

        turnCount = 0;

        StartCoroutine("BattleLevelRoutine");
    }

    // This will be called by SquadInput
    public void GetClickedObject(Collider2D col)
    {
        Debug.Log("Clicked " + col.gameObject.name);

        switch (col.gameObject.tag)
        {
            case "Survivor":

                var target = col.gameObject.GetComponent<Character>();

                SetActivePanel(col.gameObject);

                // If not Battle, Change active unit and give control to that target
                if(!IsBattle)
                {
                    squadManager.SetActiveUnit(target);
                }
                // if it is in the middle of battle, check if the clicked unit is active unit
                break;

            case "Enemy":

                var enemy = col.gameObject.GetComponent<Enemies>();
                // Show enemy information by poping up window

                break;

            case "Object":
                
                // Get active unit

                // Start coroutine to pick up the object

                break;
        }
    }

    public void SetActivePanel(GameObject target)
    {
        foreach(var t in mainPanels)
        {
            if(t.character.gameObject == target)
            {
                t.gameObject.SetActive(true);
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }
    }


    public void StartTurn()
    {
        // For debugging purpose, set random.range(0, 1). It should be set to random.range(-1, 1)
        var min = 0;
        var max = 1;
        var temp = Random.Range(min, max);

        m_isTurnComplete = false;

        if(temp >= 0)
        {
            this.turnState = GameManager.TurnState.Player;
            turnCount++;
            StartCoroutine("PlayerTurnRoutine");
        }
        else
        {
            this.turnState = GameManager.TurnState.Enemy;
            turnCount++;
            StartCoroutine("EnemyTurnRoutine");
        }
    }


    IEnumerator PlayerTurnRoutine()
    {
        // Choose Active Unit
        var currentList = squadManager.GetCurrentCharacterList();
        var activeUnit = currentList[Random.Range(0, currentList.Count)];

        squadManager.SetActiveUnit(activeUnit);
        this.SetActivePanel(activeUnit.gameObject);

        Debug.Log("Turn " + turnCount + " : Player's Turn - position " + squadManager.activeUnit.currentPosition);

        yield return new WaitForSeconds(0.5f);

        while (turnStep == TurnStep.WaitForCommand)
        {
            // activeUnit.Say();
            yield return new WaitForSeconds(1.5f);

            turnStep = TurnStep.ChooseCommand;
        }

        while(turnStep == TurnStep.ChooseCommand)
        {
            // If Skill Button is clicked, it will call SkillTarget.DrawTargets() and move to the next TurnStep

            yield return null;
        }

        while (turnStep == TurnStep.ConfirmCommand)
        {

            yield return null;
        }

        while (turnStep == TurnStep.Act)
        {
            // If comandBtn is clicked, it will call ComfirmCommand() in order to change the TurnStep to Act.

            UpdateNarration(activeSkill.skillName + " Strikes...!");

            // Animations come here
            actor.Act(activeSkill, activeTargets);

            while(m_isActing)
            {
                yield return null;
            }

            // Set Anim Time and then change turn step
            yield return new WaitForSeconds(0.5f);

            turnStep = TurnStep.FinishTurn;
        }

        while (turnStep == TurnStep.FinishTurn)
        {
            // Calculation, unit saying, status change, etc

            // If second Anim is over, change the turnstep
            yield return new WaitForSeconds(1f);

            turnStep = TurnStep.WaitForCommand;
        }

        yield return new WaitForSeconds(1f);

        m_isTurnComplete = true;

        // Check if Enemies are all dead after turn is complete
        if(AreEnemiesAllDead())
        {
            Debug.Log("All Enemies Are Dead");
            m_isBattle = false;
        }
        else
        {
            UpdateTurn();
        }
    }

    public void UpdateTurn()
    {
        Debug.Log("Turn Updated");
        StartTurn();
    }

    IEnumerator EnemyTurnRoutine()
    {
        // Choose Active Unit
        var currentList = enemySquadManager.GetCurrentEnemyList();
        var activeUnit = currentList[Random.Range(0, currentList.Count)];

        enemySquadManager.SetActiveUnit(activeUnit);

        Debug.Log("Turn " + turnCount + " : Enemy's Turn - position " + enemySquadManager.activeUnit.currentPosition);

        yield return new WaitForSeconds(0.5f);

        while (turnStep == TurnStep.WaitForCommand)
        {
            // activeUnit.Say();
            yield return new WaitForSeconds(1.5f);

            turnStep = TurnStep.ChooseCommand;
        }

        while (turnStep == TurnStep.ChooseCommand)
        {
            // We need a function to choose 1 skill Randomly

            yield return null;
        }

        while (turnStep == TurnStep.ConfirmCommand)
        {
            // This Step is not necessary for enemies

            yield return null;
        }

        while (turnStep == TurnStep.Act)
        {
            // Animations come here

            // Set Anim Time and then change turn step
            yield return new WaitForSeconds(skillAnimTime);

            turnStep = TurnStep.FinishTurn;
        }

        while (turnStep == TurnStep.FinishTurn)
        {
            // Calculation, unit saying, status change, etc

            // If second Anim is over, change the turnstep
            yield return null;
        }

        m_isTurnComplete = true;

        yield return new WaitForSeconds(0.5f);

        // Check if Enemies are all dead after turn is complete
        if (AreSurvivorsAllDead())
        {
            m_isBattle = false;
            LoseLevel();
        }
        else
        {
            UpdateTurn();
        }
    }

    public void LoseLevel()
    {
        StartCoroutine(LoseLevelRoutine());
    }

    IEnumerator LoseLevelRoutine()
    {
        m_isGameOver = true;

        yield return new WaitForSeconds(1.5f);

        if(loseLevelEvent != null)
        {
            loseLevelEvent.Invoke();
        }

        yield return new WaitForSeconds(1.5f);

        Debug.Log("LOSE! =============");

        RestartLevel();
    }



    bool AreEnemiesAllDead()
    {
        enemySquadManager.GetCurrentEnemyList();
        foreach(var enemy in enemySquadManager.enemyList)
        {
            if(!enemy.isDead)
            {
                return false;
            }
        }
        return true;
    }

    bool AreSurvivorsAllDead()
    {
        squadManager.GetCurrentCharacterList();
        foreach(var survivor in squadManager.characterList)
        {
            if(!survivor.isDead)
            {
                return false;
            }
        }
        return true;
    }

    // This will be called by clicking commandBtn
    public void ConfirmCommand()
    {
        Debug.Log("Command Confirmed!");
        this.turnStep = TurnStep.Act;

        activeSkill.skillTarget.ResetDraw();
    }

    public void SetAction(Character activeUnit, Skill skill, List<GameObject> targets)
    {
        this.actor = activeUnit;
        this.activeSkill = skill;
        this.activeTargets = targets;
    }

    // For Test Purpose
    public void TestButton()
    {
        m_isTurnComplete = true;
        foreach (var enemy in enemySquadManager.enemyList)
        {
            enemy.isDead = true;
        }
    }

}

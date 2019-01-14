using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMover))]
public class PlayerManager : MonoBehaviour 
{
    public static PlayerManager instance;
    public PlayerInput playerInput;
    public PlayerMover playerMover;
    public BoxCollider2D finder;

    public static float spacing = -3.5f;
    public static readonly Vector2[] positions =
    {
        new Vector2(1 * spacing, 0f),
        new Vector2(2 * spacing, 0f),
        new Vector2(3 * spacing, 0f),
        new Vector2(4 * spacing, 0f),
    };


    public bool isMovingForward = false;
    public bool isMovingBackWard = false;
    public bool isReached = false;


    public GameObject[] characterPrefabs = new GameObject[4];
    public List<BaseCharacter> characterList = new List<BaseCharacter>();
    public BaseCharacter activeCharacter;
    public GameObject clickedObject;

    // Skill targets storage
    public List<BaseEnemy> unfriendlyTargets = new List<BaseEnemy>();
    public List<BaseCharacter> friendlyTargets = new List<BaseCharacter>();

    private void Awake()
    {
        MakeSingleton();
        playerMover = GetComponent<PlayerMover>();
        playerInput = GetComponent<PlayerInput>();
        finder = GetComponent<BoxCollider2D>();


    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        // If character List is empty, then Instantiate default character setting
        if(characterList.Count == 0)
        {
            for(int i = 0; i < characterPrefabs.Length; i++)
            {
                if(characterPrefabs[i] != null)
                {
                    var survivor = Instantiate(characterPrefabs[i], Vector3.zero, Quaternion.identity);
                    survivor.transform.SetParent(this.transform);
                    characterList.Add(survivor.GetComponent<BaseCharacter>());
                }
            }
            Debug.Log("Character List has been made by default setting");
        }

        SetPositions(characterList);
        // Setting for UI
        UIManager.instance.SkillPanelInitilize();

        // Set Active Character
        SetActiveCharacterAtPos(1);
    }

    private void Update()
    {
        playerInput.GetKeyInput();

        if(playerInput.H > 0)
        {
            if (this.transform.position.x > Commander.instance.rightBorder)
            {
                return;
            }
            playerMover.MoveForward();
        }
        else if (playerInput.H < 0)
        {
            if (this.transform.position.x < Commander.instance.leftBorder)
            {
                return;
            }
            playerMover.MoveBackWard();
        }
        else
        {
            playerMover.Stop();
        }
    }

    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    // Initialize ()
    public void SetPositions(List<BaseCharacter> cList)
    {
        for (int i = 0; i < cList.Count; i++)
        {
            if(cList[i] != null)
            {
                cList[i].Position = i + 1;
                cList[i].transform.localPosition = PlayerManager.positions[i];
            }
        }
    }


    // Can be called 3 ways
    // 1. Initialize
    // 2. When player select a character when it is not in the battle
    // 3. AI chooses the active character by turn order
    public void SetActiveCharacterAtPos(int pos)
    {
        var target = GetCharacterAtPos(pos);

        // Do nothing if selected character is current active character
        if (!Commander.instance.IsBattle && activeCharacter == target)
        {
            return;
        }

        // Set existing activeCharacter to inActive. In the beginning there is no existing active character.
        if(activeCharacter)
        {
            activeCharacter.isActive = false;
            activeCharacter.cursor.SetActive(false);
        }

        // Set New ActiveCharacter
        activeCharacter = target;
        activeCharacter.isActive = true;
        activeCharacter.cursor.SetActive(true);

        // Update UI Panel
        UIManager.instance.UpdateUIPanel(activeCharacter);
    }

    public void SetActiveCharacter(BaseCharacter target)
    {
        if (!Commander.instance.IsBattle && activeCharacter == target)
        {
            return;
        }

        if(activeCharacter)
        {
            activeCharacter.isActive = false;
            activeCharacter.cursor.SetActive(false);
        }
        activeCharacter = target;
        activeCharacter.isActive = true;
        activeCharacter.cursor.SetActive(true);

        // Update UI Panel
        UIManager.instance.UpdateUIPanel(activeCharacter);
    }

    public BaseCharacter GetCharacterAtPos(int number)
    {
        // It can return null if the character at pos is dead
        return characterList[number - 1];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Trigger":
                var battle = collision.gameObject.GetComponent<Trigger>();
                Commander.instance.InitBattle(battle);
                break;

            case "Goal":
                isReached = true;
                Commander.instance.IsGameOver = true;
                Debug.Log("Cleared!========================");
                break;
        }
    }

    public void DrawTargets(BaseSkill activeSkill)
    {
        switch(activeSkill.skillRange)
        {
            case SkillRange.Unfriendly:

                ClearUnfriendlyTargets();

                for (int i = 0; i < EnemyManager.instance.characterList.Count; i++)
                {
                    if(activeSkill.targetPositions[i])
                    {
                        EnemyManager.instance.characterList[i].targetCursor.SetActive(true);
                        EnemyManager.instance.characterList[i].isTargeted = true;
                        unfriendlyTargets.Add(EnemyManager.instance.characterList[i]);
                    }
                }
                Commander.instance.turnStateMachine.currentTurnState = TurnStateMachine.TurnState.ConfirmTarget;
                break;


            case SkillRange.Friendly:
                ClearFriendlyTargets();

                for (int i = 0; i < PlayerManager.instance.characterList.Count; i++)
                {
                    if (activeSkill.targetPositions[i])
                    {
                        PlayerManager.instance.characterList[i].targetCursor.SetActive(true);
                        PlayerManager.instance.characterList[i].isTargeted = true;
                        friendlyTargets.Add(PlayerManager.instance.characterList[i]);
                    }
                }
                Commander.instance.turnStateMachine.currentTurnState = TurnStateMachine.TurnState.ConfirmTarget;

                break;
            case SkillRange.Random:
                ClearUnfriendlyTargets();

                for (int i = 0; i < EnemyManager.instance.characterList.Count; i++)
                {
                    if (activeSkill.targetPositions[i])
                    {
                        EnemyManager.instance.characterList[i].targetCursor.SetActive(true);
                        EnemyManager.instance.characterList[i].isTargeted = true;
                        unfriendlyTargets.Add(EnemyManager.instance.characterList[i]);
                    }
                }
                Commander.instance.turnStateMachine.currentTurnState = TurnStateMachine.TurnState.ConfirmTarget;
                break;
            default:
                break;
        }
    }

    void ClearUnfriendlyTargets()
    {
        foreach (var t in unfriendlyTargets)
        {
            t.targetCursor.SetActive(false);
            t.isTargeted = false;
        }

        unfriendlyTargets.Clear();
    }

    void ClearFriendlyTargets()
    {
        foreach (var t in friendlyTargets)
        {
            t.targetCursor.SetActive(false);
            t.isTargeted = false;
        }

        friendlyTargets.Clear();
    }

    public void ConfirmAllyTarget(BaseCharacter selectedTarget)
    {
        Commander.instance.turnStateMachine.HasConfirmedTargets = true;
        Commander.instance.turnStateMachine.currentTurnState = TurnStateMachine.TurnState.DoAction;

        switch(activeCharacter.activeCommand.skillType)
        {
            case SkillType.SingleTarget:
                activeCharacter.CastToAlly(activeCharacter.activeCommand, selectedTarget);

                ClearFriendlyTargets();
                break;
            case SkillType.MultipleTarget:
                activeCharacter.CastToAllies(activeCharacter.activeCommand, friendlyTargets);

                ClearFriendlyTargets();
                break;
            case SkillType.SelfTarget:
                activeCharacter.CastToSelf(activeCharacter.activeCommand);

                break;
        }
    }

    public void ConfirmEnemyTarget(BaseEnemy selectedTarget)
    {
        Commander.instance.turnStateMachine.HasConfirmedTargets = true;
        Commander.instance.turnStateMachine.currentTurnState = TurnStateMachine.TurnState.DoAction;
        UIManager.instance.BeginUIShield();

        switch (activeCharacter.activeCommand.skillType)
        {
            case SkillType.SingleTarget:
                activeCharacter.CastToEnemy(activeCharacter.activeCommand, selectedTarget);

                ClearUnfriendlyTargets();
                break;

            case SkillType.MultipleTarget:
                activeCharacter.CastToEnemies(activeCharacter.activeCommand, unfriendlyTargets);

                ClearUnfriendlyTargets();
                break;
            default:
                break;
        }
    }
}

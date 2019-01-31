using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : DeckManager
{
    public static EnemyManager instance;
    public static float spacing = 3.5f;
    public static readonly Vector2[] positions =
    {
        new Vector2(1 * spacing, 0f),
        new Vector2(2 * spacing, 0f),
        new Vector2(3 * spacing, 0f),
        new Vector2(4 * spacing, 0f),
    };

    public List<BaseEnemy> characterList = new List<BaseEnemy>();
    public BaseEnemy activeCharacter;
    public GameObject clickedObject;
    public BaseSkill activeCommand;

    // Exclusive Parameters
    

    private void Awake()
    {
        MakeSingleton();
    }

    // Use this for initialization
    void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
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

    public void Deploy(Trigger t)
    {
        characterList.Clear();
        this.transform.position = PlayerManager.instance.transform.position + new Vector3(0, 0.25f, 0f);

        for (int i = 0; i < t.enemyPrefabs.Length; i++)
        {
            if (t.enemyPrefabs[i] != null)
            {
                var enemy = Instantiate(t.enemyPrefabs[i]);
                enemy.transform.SetParent(this.transform);

                var enemyCompo = enemy.GetComponent<BaseEnemy>();
                characterList.Add(enemyCompo);

                // Initialize Enemies
                enemyCompo.Initiate();
                enemyCompo.characterAction.Initiate();

                enemyCompo.cursor.SetActive(false);
            }
        }
        SetPositions(characterList);
        Debug.Log("Enemy has been deployed");
    }

    public void SetPositions(List<BaseEnemy> cList)
    {
        for (int i = 0; i < cList.Count; i++)
        {
            if (cList[i] != null)
            {
                cList[i].Position = i + 1;
                cList[i].transform.localPosition = EnemyManager.positions[i];
            }
        }
    }

    public void SetActiveCharacterAtPos(int pos)
    {
        var target = GetCharacterAtPos(pos);

        SetActiveCharacter(target);
    }

    public void SetActiveCharacter(BaseEnemy target)
    {
        if (activeCharacter)
        {
            ClearActiveCharacter();
        }

        if (PlayerManager.instance.activeCharacter)
        {
            PlayerManager.instance.ClearActiveCharacter();
        }
        activeCharacter = target;

        activeCharacter.isActive = true;
        activeCharacter.cursor.SetActive(true);

        Commander.instance.narrator.Narrate(activeCharacter.Name + " is attacking...!");
    }

    public BaseEnemy GetCharacterAtPos(int number)
    {
        // It can return null if the character at pos is dead
        return characterList[number - 1];
    }

    public void ClearActiveCharacter()
    {
        activeCharacter.isActive = false;
        activeCharacter.cursor.SetActive(false);
        activeCharacter.targetCursor.SetActive(false);

        // After completing necessary things, then clear activeCharacter to null
        activeCharacter = null;
    }

    public void PlayTurn()
    {
        StartCoroutine(PlayTurnRoutine());
    }

    IEnumerator PlayTurnRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        EnemyManager.instance.activeCharacter.ChooseCommand();
        Commander.instance.narrator.Narrate(EnemyManager.instance.activeCommand.skillName);

        yield return new WaitForSeconds(1f);
        // Set Target
        if(EnemyManager.instance.activeCommand.skillRange == SkillRange.Unfriendly)
        {

        }

        Commander.instance.turnStateMachine.HasConfirmedCommand = true;

        // Choose Available Command


        // Select Target
        // Do Action
        yield return null;
    }


}

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

    public static float spacing = -3.5f;
    public static readonly Vector2[] positions =
    {
        new Vector2(1 * spacing, 0f),
        new Vector2(2 * spacing, 0f),
        new Vector2(3 * spacing, 0f),
        new Vector2(4 * spacing, 0f),
    };


    public bool isMoving = false;
    public bool isRetreating = false;
    public bool isReached = false;

    public BoxCollider2D finder;

    public List<GameObject> characterPrefabs = new List<GameObject>();
    public List<BaseCharacter> characterList = new List<BaseCharacter>();

    private void Awake()
    {
        MakeSingleton();
        playerMover = GetComponent<PlayerMover>();
        playerInput = GetComponent<PlayerInput>();
        finder = GetComponent<BoxCollider2D>();

        playerInput.InputEnabled = true;
        SetPositions(characterList);
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

    private void Initialize()
    {
        for (int i = 0; i < characterPrefabs.Count; i++)
        {
            if(characterPrefabs[i] != null)
            {
                var temp = Instantiate(characterPrefabs[i]);
                characterList.Add(temp.GetComponent<BaseCharacter>());
            }
        }
    }

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

    public BaseCharacter GetCharacterAtPos(int number)
    {
        return characterList[number - 1];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Goal")
        {
            isReached = true;
        }
    }
}

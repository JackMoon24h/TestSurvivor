using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadMover : MonoBehaviour 
{
    public bool isMoving = false;
    [HideInInspector]public float moveForwardSpeed = 2.5f;
    [HideInInspector]public float moveBackwardSpeed = 1.5f;
    [HideInInspector]public iTween.EaseType easeType = iTween.EaseType.linear;

    GameManager gameManager;
    SquadManager squadManager;

    GameObject backGrounds;

	void Awake () 
    {
        squadManager = this.GetComponent<SquadManager>();
        backGrounds = GameObject.FindWithTag("Backgrounds");
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
	}

    public void MoveForward()
    {
        if(isMoving)
        {
            return;
        }
        StartCoroutine(MoveRoutine(squadManager.squadInput.MovePos, moveForwardSpeed));
    }

    public void MoveBackWard()
    {
        if (isMoving)
        {
            return;
        }
        StartCoroutine(MoveRoutine(squadManager.squadInput.MovePos, moveBackwardSpeed));
    }

    IEnumerator MoveRoutine(Vector3 destinationPos, float moveSpeed)
    {
        isMoving = true;

        iTween.MoveTo(gameObject, iTween.Hash(
            "x", destinationPos.x,
            "y", destinationPos.y,
            "z", destinationPos.z,
            "easetype", easeType,
            "speed", moveSpeed
        ));

        while(Vector3.Distance(destinationPos, this.transform.position) > 0.1f)
        {
            squadManager.squadInput.InputEnabled = false;
            yield return null;
        }

        Stop();
        this.transform.position = destinationPos;

        if(gameManager.IsGameOver)
        {
            squadManager.squadInput.InputEnabled = false;
        }
        else
        {
            squadManager.squadInput.InputEnabled = true;
        }

    }

    public void Stop()
    {
        iTween.Stop(gameObject);
        isMoving = false;
    }


}

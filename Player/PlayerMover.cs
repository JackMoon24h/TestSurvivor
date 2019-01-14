using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMover : MonoBehaviour 
{

    public float moveSpeed = 3f;
    public float moveBackSpeed = -1.5f;


    // Not necessary if not using iTween
    public Vector3 destination;
    public iTween.EaseType easeType = iTween.EaseType.linear;

    void Move(float moveSPD)
    {
        this.transform.Translate(moveSPD * Time.deltaTime, 0, 0);
    }

    public void MoveForward()
    {
        PlayerManager.instance.isMovingForward = true;
        Move(moveSpeed);
    }

    public void MoveBackWard()
    {
        PlayerManager.instance.isMovingBackWard = true;
        Move(moveBackSpeed);
    }


    public void Stop()
    {
        PlayerManager.instance.isMovingForward = false;
        PlayerManager.instance.isMovingBackWard = false;
    }
}

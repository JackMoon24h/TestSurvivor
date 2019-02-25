using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjects : MonoBehaviour 
{
    public float moveSpeed;
    public float moveBackSpeed;

	
	// Update is called once per frame
	void Update () 
    {
		if(PlayerManager.instance.isMovingForward)
        {
            MoveForward();
        }
        else if (PlayerManager.instance.isMovingBackWard)
        {
            MoveBackWard();
        }
    }

    void Move(float moveSPD)
    {
        this.transform.Translate(moveSPD * Time.deltaTime, 0, 0);
    }

    void MoveForward()
    {
        Move(moveSpeed);
    }

    void MoveBackWard()
    {
        if(PlayerManager.instance.transform.position.x > 0)
        {
            Move(moveBackSpeed);
        }
    }
}

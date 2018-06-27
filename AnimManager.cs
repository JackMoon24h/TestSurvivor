using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour 
{

    SquadManager squadManager;
    [HideInInspector]public Animator animator;

	// Use this for initialization
	void Start () 
    {
        squadManager = Object.FindObjectOfType<SquadManager>().GetComponent<SquadManager>();
        animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(squadManager.squadMover.isMoving)
        {
            animator.SetBool("walk", true);
            if(squadManager.squadInput.Direction > 0f)
            {
                animator.SetFloat("direction", 1);
            }
            else 
            {
                animator.SetFloat("direction", -1);
            }
        }
        else
        {
            animator.SetBool("walk", false);
        }
	}
}

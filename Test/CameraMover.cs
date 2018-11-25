using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour 
{

    public GameObject playerSquad;
    Vector3 offset;

	// Use this for initialization
	void Awake () 
    {
        playerSquad = GameObject.FindWithTag("Player");
	}

    private void Start()
    {
        offset = this.transform.position - playerSquad.transform.position;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        this.transform.position = playerSquad.transform.position + offset;
    }
}

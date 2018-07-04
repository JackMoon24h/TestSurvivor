using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ObjectTrigger : MonoBehaviour 
{
    BoxCollider2D col;

    public bool isActive = false;


	// Use this for initialization
	void Start () 
    {
        col = GetComponent<BoxCollider2D>();
	}
	
    void OnTriggerEnter2D (Collider2D other) 
    {
        isActive = true;
        Debug.Log("HIT!!!!!");
	}


}

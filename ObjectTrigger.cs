using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ObjectTrigger : MonoBehaviour 
{
    BoxCollider2D col;
    string objectName;

    public bool isTouchEnabled = false;
    public bool isClicked = false;


	// Use this for initialization
	void Start () 
    {
        col = GetComponent<BoxCollider2D>();
        objectName = this.gameObject.name;
	}
	
    void OnTriggerEnter2D (Collider2D other) 
    {
        isTouchEnabled = true;
        isClicked = true;
        Debug.Log("HIT!!!!!");
	}


}

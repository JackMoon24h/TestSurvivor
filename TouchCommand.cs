using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchCommand : MonoBehaviour {

	Manager gameManager;

	void Start(){
		gameManager = Manager.instance;
	}

	public void OnClick()
	{
		Debug.Log(this.name + " is clicked");
	}
}

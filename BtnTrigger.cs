using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnTrigger : MonoBehaviour {

	Manager gameManager;

	void Start()
	{
		gameManager = Manager.instance;
	}

	void OnMouseDown()
	{
		gameObject.SetActive (false);

		gameManager.activeSurvivor.turnState = BaseSurvivor.TurnState.ACTING;
	}
}

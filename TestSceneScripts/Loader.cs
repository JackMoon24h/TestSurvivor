using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

	public GameObject gameManager;
	public GameObject statusWindow;

	// Use this for initialization
	void Awake () {
		if (Manager.instance == null)
			Instantiate (gameManager);
		Debug.Log ("Manager Created");
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;

public class SKillButton : MonoBehaviour {

	Manager gameManager;
	Skills skill;

	void Start(){
		gameManager = Manager.instance;

	}

	public void MyClick(){
		Debug.Log ("This Skill is clicked");
	}

	void DrawTarget(Skills givenSkill){
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusWindow : MonoBehaviour {

	public static StatusWindow instance = null;
	Manager gameManager;
	[SerializeField]BaseSurvivor selectedSurvivor;

	Text labelHP;
	Text labelMP;
	Text labelDMG;
	Text labelPROT;
	Text labelACC;
	Text labelDOD;
	Text labelINF;
	Text labelBLD;
	Text labelFEAR;
	Text labelSTUN;
	Text labelMOVE;
	Text labelDB;

	// Use this for initialization
	void Awake(){
		if(instance == null){
			instance = this;
		} else if (instance != this){
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	void Start () {

		gameManager = Manager.instance;

		labelHP = GameObject.Find ("Window").transform.GetChild (0).GetComponent<Text>();
		labelMP = GameObject.Find ("Window").transform.GetChild (1).GetComponent<Text>();
		labelDMG = GameObject.Find ("Window").transform.GetChild (2).GetComponent<Text>();
		labelPROT = GameObject.Find ("Window").transform.GetChild (3).GetComponent<Text>();
		labelACC = GameObject.Find ("Window").transform.GetChild (4).GetComponent<Text>();
		labelDOD = GameObject.Find ("Window").transform.GetChild (5).GetComponent<Text>();
		labelINF = GameObject.Find ("Window").transform.GetChild (6).GetComponent<Text>();
		labelBLD = GameObject.Find ("Window").transform.GetChild (7).GetComponent<Text>();
		labelFEAR = GameObject.Find ("Window").transform.GetChild (8).GetComponent<Text>();
		labelSTUN = GameObject.Find ("Window").transform.GetChild (9).GetComponent<Text>();
		labelMOVE = GameObject.Find ("Window").transform.GetChild (10).GetComponent<Text>();
		labelDB = GameObject.Find ("Window").transform.GetChild (11).GetComponent<Text>();
	}
	
	public void UpdateWindow(BaseSurvivor selected){

		this.selectedSurvivor = selected;

		labelHP.text = labelHP.name + " : " + selected.health;
		labelMP.text = labelMP.name + " : " + selected.mental;
		labelDMG.text = labelDMG.name + " : " + selected.damage;
		labelPROT.text = labelPROT.name + " : " + selected.protection;
		labelACC.text = labelACC.name + " : " + selected.accuracy;
		labelDOD.text = labelDOD.name + " : " + selected.dodge;
		labelINF.text = labelINF.name + " : " + selected.infectRes;
		labelBLD.text = labelBLD.name + " : " + selected.bleedRes;
		labelFEAR.text = labelFEAR.name + " : " + selected.fearRes;
		labelSTUN.text = labelSTUN.name + " : " + selected.stunRes;
		labelMOVE.text = labelMOVE.name + " : " + selected.moveRes;
		labelDB.text = labelDB.name + " : " + selected.deathBlow;
	}
}

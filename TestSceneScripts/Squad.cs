using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Squad : MonoBehaviour {

	public GameObject position1;
	public GameObject position2;
	public GameObject position3;
	public GameObject position4;

	public Vector3 startPos1 = new Vector3(-1.5f, -0.5f, 0f);
	public Vector3 startPos2 = new Vector3(-3.25f, -0.5f, 0f);
	public Vector3 startPos3 = new Vector3(-5.0f, -0.5f, 0f);
	public Vector3 startPos4 = new Vector3(-6.75f, -0.5f, 0f);

	private List<GameObject> Characters = new List<GameObject>();
	private int activeCharacter;

	private Manager gameManager;

	public Squad(GameObject cha1, GameObject cha2, GameObject cha3, GameObject cha4){
		position1 = Instantiate (cha1, startPos1, Quaternion.identity);
		position2 = Instantiate (cha2, startPos2, Quaternion.identity);
		position3 = Instantiate (cha3, startPos3, Quaternion.identity);
		position4 = Instantiate (cha4, startPos4, Quaternion.identity);
	}

	// Use this for initialization
	void Start () {
		gameManager = Manager.instance;

		this.transform.position = new Vector3 (0f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}

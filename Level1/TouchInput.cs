using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour 
{
    Camera mainCamera;
    GameManager gameManager;

    public GameObject activeUnit;
    public GameObject clickedObject;

	// Use this for initialization
	protected virtual void Start () 
	{
        mainCamera = Camera.main;
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
	}
	
	// Update is called once per frame
    protected virtual void Update () 
	{
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, new Vector3(0, 0, 10f), 100f);

            if(hit.collider != null)
            {
                clickedObject = this.GetObjectInfo(hit.collider);
                Debug.Log("Clicked Object : " + clickedObject.name);
            }
            else
            {
                Debug.Log("No Object Found");
            }
        }
	}

    public GameObject GetObjectInfo(Collider2D col)
    {
        return col.gameObject;
    }
}

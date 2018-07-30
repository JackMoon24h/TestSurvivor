using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour 
{
    bool m_inputEnabled = false;
    public bool InputEnabled { get { return m_inputEnabled; } set { m_inputEnabled = value; } }

    Camera mainCamera;
    Deck playerDeck;

	// Use this for initialization
	void Start () 
    {
        mainCamera = Camera.main;
        playerDeck = Object.FindObjectOfType<Deck>().GetComponent<Deck>();
	}
	
    public void GetTouchInput()
    {
        if(m_inputEnabled)
        {
            if(Input.GetMouseButtonDown(0))
            {
                // Cast ray
                var worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPos, new Vector3(0, 0, 10f), 100f);

                if(hit.collider != null)
                {
                    // If it clicked something, than check the tag
                    switch(hit.collider.tag)
                    {
                        case "Survivor":
                            playerDeck.GetUnitByClick(hit.collider);
                            break;

                        case "Enemy":
                            playerDeck.GetUnitByClick(hit.collider);
                            break;

                        case "Object":
                            playerDeck.GetObjectByClick(hit.collider);
                            break;
                    }
                }
                else
                {
                    // It didn't click anything. So, move
                }
            }
        }
    }
}

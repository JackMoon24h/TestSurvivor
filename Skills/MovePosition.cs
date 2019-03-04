using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePosition : MonoBehaviour 
{
    public Button btn;
    public bool isBtnPressed;

	// Use this for initialization
	void Start () 
    {
        btn = GetComponent<Button>();

        btn.onClick.AddListener(OnClickEvent);
	}
	
	void OnClickEvent()
    {
        if(isBtnPressed)
        {
            // Cancel drawing targets
            PlayerManager.instance.CancelSwap();
            return;
        }

        // DrawTargets
        if(PlayerManager.instance.characterList.Count <= 1)
        {
            return;
        }
        PlayerManager.instance.DrawSwapPositions();
        isBtnPressed = true;
    }
}

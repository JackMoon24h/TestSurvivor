using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Narrator : MonoBehaviour 
{
    GameObject narration;
    GraphicMover graphicMover;
    Text narrationText;

    public GameObject afflictionLabel;
    Text affLabelText;
    GraphicMover afflictionGM;

    private bool m_isNarrating = false;
    public bool IsNarrating { get{return m_isNarrating;} set{m_isNarrating = value;}}

    // Use this for initialization
    void Start () 
    {
        narration = GameObject.Find("Narration");
        graphicMover = narration.GetComponent<GraphicMover>();
        narrationText = narration.GetComponentInChildren<Text>();
        affLabelText = afflictionLabel.GetComponentInChildren<Text>();
        afflictionGM = afflictionLabel.GetComponent<GraphicMover>();
        SetUp();
	}

    void SetUp()
    {
        graphicMover.mode = GraphicMoverMode.MoveInOut;
    }

    public void Narrate(string sentence)
    {
        m_isNarrating = true;
        narrationText.text = sentence;
        graphicMover.Move();
    }

    public void ShowAfflictionResult(string afflictionName)
    {
        m_isNarrating = true;
        affLabelText.text = afflictionName;
        afflictionGM.Move();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadInput : MonoBehaviour 
{
    bool m_inputEnabled = false;
    public bool InputEnabled { get { return m_inputEnabled; } set { m_inputEnabled = value; } }

    Vector3 m_movePos;
    public Vector3 MovePos { get { return m_movePos; } }

    float m_direction;
    public float Direction { get { return m_direction; } set { m_direction = value; } }

    public bool inputDetected = false;

    Vector3 forward = new Vector3(6f, 0f, 0f);
    Vector3 backward = new Vector3(-3f, 0f, 0f);


    // Sense input and direction
    public void GetTouhInput()
    {
        if(m_inputEnabled)
        {
            if(Input.GetMouseButtonDown(0))
            {
                inputDetected = true;
                var screenPos = Input.mousePosition;
                var worldPos = Camera.main.ScreenToWorldPoint(screenPos);

                if(worldPos.x >= this.transform.position.x)
                {
                    var movePos = this.transform.position + forward;
                    m_direction = 1f;
                    m_movePos = movePos;
                } 
                else 
                {
                    var movePos = this.transform.position + backward;
                    m_direction = -1f;
                    m_movePos = movePos;
                }
            }
        }
    }
}

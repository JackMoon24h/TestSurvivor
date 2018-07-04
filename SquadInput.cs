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

    public bool moveInputDetected = false;

    Vector3 forward = new Vector3(6f, 0f, 0f);
    Vector3 backward = new Vector3(-3f, 0f, 0f);

    GameManager gameManager;
    SquadManager squadManager;

    private void Start()
    {
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        squadManager = GetComponent<SquadManager>();
    }

    // Sense input and direction
    public void GetTouchInput()
    {
        if(m_inputEnabled)
        {
            if(Input.GetMouseButtonDown(0))
            {
                // Cast ray
                var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPos, new Vector3(0, 0, 10f), 100f);

                // If player clicks an object
                if(hit.collider != null)
                {
                    gameManager.GetClickedObject(hit.collider);
                }
                else // If player tries to move
                {
                    moveInputDetected = true;
                    if (worldPos.x >= this.transform.position.x)
                    {
                        var movePos = this.transform.position + forward;
                        m_direction = 1f;
                        m_movePos = movePos;
                    }
                    else if(worldPos.x < this.transform.position.x - 14f)
                    {
                        var movePos = this.transform.position + backward;
                        m_direction = -1f;
                        m_movePos = movePos;
                    }
                }
            }
        }
    }
}

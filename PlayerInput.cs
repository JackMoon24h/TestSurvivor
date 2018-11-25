using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    private float m_h;
    public float H { get { return m_h; } }

    bool m_inputEnabled = false;
    public bool InputEnabled { get { return m_inputEnabled; } set { m_inputEnabled = value; } }

    public void GetKeyInput()
    {
        if(m_inputEnabled)
        {
            m_h = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            m_h = 0f;
        }
    }
}

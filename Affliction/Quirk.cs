using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quirk : MonoBehaviour 
{
    [SerializeField]
    protected string m_name;
    public string Name { get { return m_name; } }

    [SerializeField]
    protected string m_description;
    public string Description { get { return m_description; } }

    [SerializeField]
    protected bool m_isPositive;
    public bool IsPositive { get { return m_isPositive; } }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VirtueType
{
    Patience,
    Focus
}

public class Virtue : MonoBehaviour 
{
    protected string m_name;
    public string Name { get { return m_name; } }

    protected string m_description;
    public string Description { get { return m_description; } }

    protected float m_effect = 0.3f;
    public float Effect { get { return m_effect; } set { m_effect = value; } }

    public VirtueType type;
    public BaseCharacter owner;
    public List<ActOut> possibleActOuts = new List<ActOut>();

    // Use this for initialization
    protected virtual void Start()
    {

    }

    public virtual void SetEffects(BaseCharacter target)
    {
        owner = target;
        owner.IsVirtuous = true;
        owner.virtuousEffect = this;

        // Common Debuff for all afflictions
        owner.MaxHealth = Utility.StatIntRound(owner.MaxHealth * (1 + m_effect));
        owner.BleedRes = Utility.StatFloatRound(owner.BleedRes * (1 + m_effect));
        owner.InfectRes = Utility.StatFloatRound(owner.InfectRes * (1 + m_effect));
        owner.StunRes = Utility.StatFloatRound(owner.StunRes * (1 + m_effect));
        owner.MoveRes = Utility.StatFloatRound(owner.MoveRes * (1 + m_effect));
    }

    public virtual ActOut GetActOut()
    {
        var temp = Random.Range(0, possibleActOuts.Count);
        return possibleActOuts[temp];
    }
}

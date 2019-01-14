using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour 
{
    private string m_itemName;
    private string m_itemDescription;
    private int m_itemID;
    public enum ItemType
    {
        EQUIPMENT,
        WEAPON,
        MEDICINE,
        FOOD
    }
    public ItemType itemType;

    public string ItemName
    {
        get
        {
            return m_itemName;
        }

        set
        {
            m_itemName = value;
        }
    }

    public string ItemDescription
    {
        get
        {
            return m_itemDescription;
        }

        set
        {
            m_itemDescription = value;
        }
    }

    public int ItemID
    {
        get
        {
            return m_itemID;
        }

        set
        {
            m_itemID = value;
        }
    }
}

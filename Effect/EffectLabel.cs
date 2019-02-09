using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectLabel : MonoBehaviour 
{
    public float fadeOutSpeed = 2f;
    // Use this for initialization
    void Start () 
    {
        Destroy(gameObject, fadeOutSpeed);
	}

}
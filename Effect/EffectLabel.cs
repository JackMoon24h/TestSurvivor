using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectLabel : MonoBehaviour 
{
    public Text DMGlabel;
    public float fadeOutSpeed = 0.5f;
    public float alpha = 1f;
    public float moveValue = 2f;
    // Use this for initialization
    void Start () 
    {
        DMGlabel = GetComponentInChildren<Text>();
	}

    void LateUpdate()
    {
        //alpha -= fadeOutSpeed * Time.deltaTime;
        //DMGlabel.color = new Color(1f, 0f, 0f, alpha);

        //alpha -= fadeOutSpeed * Time.deltaTime;

        //transform.rotation = Camera.main.transform.rotation;
        //transform.position += Vector3.up * moveValue * Time.deltaTime;

        //if (alpha < 0f)
        //{
        //    Destroy(gameObject);
        //}
    }
}

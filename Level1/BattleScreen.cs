using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class BattleScreen : MonoBehaviour 
{
    public Color solidColor = new Color(128f, 0f, 0f, 255f);

    public Color clearColor = new Color(1f, 1f, 1f, 0f);

    public float delay = 0.5f;
    public float timeToFade = 2f;
    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo;

    public float fadeOffDelay = 2f;
    public float fadeOnDelay = 0.1f;

    MaskableGraphic graphic;

	
	void Awake () 
    {
        graphic = GetComponent<MaskableGraphic>();
        gameObject.SetActive(false);
	}

    void UpdateColor(Color newColor)
    {
        graphic.color = newColor;
    }

    public void initBattle()
    {
        StartCoroutine("initBattleRoutine");
    }

    IEnumerator initBattleRoutine()
    {
        yield return StartCoroutine("FadeOnRoutine");
        yield return StartCoroutine("FadeOffRoutine");
    }

    IEnumerator FadeOnRoutine()
    {
        yield return new WaitForSeconds(fadeOnDelay);

        iTween.ValueTo(gameObject, iTween.Hash(
            "from", clearColor,
            "to", solidColor,
            "time", timeToFade,
            "delay", delay,
            "easetype", easeType,
            "onupdatetarget", gameObject,
            "onupdate", "UpdateColor"
        ));
    }

    IEnumerator FadeOffRoutine()
    {
        yield return new WaitForSeconds(fadeOffDelay);

        iTween.ValueTo(gameObject, iTween.Hash(
            "from", solidColor,
            "to", clearColor,
            "time", timeToFade,
            "delay", delay,
            "easetype", easeType,
            "onupdatetarget", gameObject,
            "onupdate", "UpdateColor"
        ));

        this.gameObject.SetActive(false);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class Narrator : MonoBehaviour 
{
    public Color solidColor = new Color(128f, 0f, 0f, 255f);

    public Color clearColor = new Color(1f, 1f, 1f, 0f);

    public float delay = 0.5f;
    public float timeToFade = 2f;

    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo;

    public float fadeOffDelay = 2f;
    public float fadeOnDelay = 0.1f;

    public float fadeInSpeed = 0.5f;
    public float fadeOutSpeed = 1f;

    MaskableGraphic graphic;


    void Awake()
    {
        graphic = GetComponent<MaskableGraphic>();
    }

    void UpdateColor(Color newColor)
    {
        graphic.color = newColor;
    }


    public void Narrate()
    {
        StartCoroutine("NarrationRoutine");
    }

    IEnumerator NarrationRoutine()
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

        //iTween.MoveFrom(gameObject, iTween.Hash(
        //    "x", this.transform.localPosition.x - 10f,
        //    "y", this.transform.localPosition.y,
        //    "z", this.transform.localPosition.z,
        //    "time", timeToFade,
        //    "delay", delay, 
        //    "easetype", easeType
        //));
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

        //iTween.MoveTo(gameObject, iTween.Hash(
        //    "x", this.transform.localPosition.x + 10f,
        //    "y", this.transform.localPosition.y,
        //    "z", this.transform.localPosition.z,
        //    "time", timeToFade,
        //    "delay", delay,
        //    "easetype", easeType
        //));

        this.gameObject.SetActive(false);
    }

}

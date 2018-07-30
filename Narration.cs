using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class Narration : MonoBehaviour
{
    public Color solidColor = new Color(128f, 0f, 0f, 255f);
    public Color clearColor = new Color(1f, 1f, 1f, 0f);

    public float delay = 0.5f;
    public float timeToFade = 1.2f;
    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo;

    public float fadeOffDelay = 1.5f;
    public float fadeOnDelay = 0.1f;

    public float fadeInSpeed = 0.5f;
    public float fadeOutSpeed = 1f;

    bool isNarrating = false;

    MaskableGraphic graphic;
    Text narrationText;

    void Awake()
    {
        graphic = GetComponent<MaskableGraphic>();
        narrationText = GetComponent<Text>();
    }

    void UpdateColor(Color newColor)
    {
        graphic.color = newColor;
    }


    public void Narrate(string sentence)
    {
        StartCoroutine(NarrationRoutine(sentence));
    }

    IEnumerator NarrationRoutine(string sentence)
    {
        if(isNarrating)
        {
            yield return null;
        }
        narrationText.text = sentence;

        yield return new WaitForSeconds(0.1f);

        isNarrating = true;

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

        yield return new WaitForSeconds(0.5f);

        isNarrating = false;
    }

}

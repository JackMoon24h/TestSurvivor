using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speaker : MonoBehaviour 
{
    BaseCharacter thisCharacter;
    public GameObject chatBox;
    public Text lineText;
    public float typeDelay = 0.03f;
    public float delay = 1f;

    private void Start()
    {
        thisCharacter = GetComponent<BaseCharacter>();
        chatBox = GameObject.FindWithTag("ChatBox");
        lineText = chatBox.GetComponentInChildren<Text>();
    }

    private string[] m_normalState =
    {
        "We need to get there as soon as possible....!",
        "Hey, don't forget the reason why we are here..",
        "Be careful...I saw something behind us....",
        "I miss the old days...before the breakout..."
    };

    private string[] m_virtueState =
    {
        "Suffer me now!",
        "Be Strong! We can survive! We have been through worse than this!!",
        "I will kill these zombies one by one... Painfully..!",
        "....Is that it? Let them all come. I don't fear you."
    };

    private string[] m_afflictedState =
    {
        "....They are gonna kill us all....it is over....",
        "...Run....We cannot survive this....We need to run..!!",
        "I should have not joined this team....This is all your falut...!!",
        "HUAAAAA!!! I don't want to die here....HELP.....!!!"
    };

    void ShowChatBox(bool value)
    {
        lineText.text = null;
        chatBox.SetActive(value);

        chatBox.GetComponent<RectTransform>().localPosition = new Vector3((thisCharacter.Position - 1) * -220, 350f, 0f);
    }

    public void Speak()
    {
        if(Commander.instance.IsSpeaking)
        {
            return;
        }

        Commander.instance.IsSpeaking = true;
        ShowChatBox(true);

        if(thisCharacter.IsAfflicted)
        {
            int rand = Random.Range(0, m_afflictedState.Length);
            lineText.color = Color.red;
            StartCoroutine(SpeakRoutine(m_afflictedState[rand]));
        }
        else if (thisCharacter.IsVirtuous)
        {
            int rand = Random.Range(0, m_virtueState.Length);
            lineText.color = Color.yellow;
            StartCoroutine(SpeakRoutine(m_virtueState[rand]));
        }
        else
        {
            int rand = Random.Range(0, m_normalState.Length);
            lineText.color = Color.white;
            StartCoroutine(SpeakRoutine(m_normalState[rand]));
        }
    }

    IEnumerator SpeakRoutine(string allLine)
    {
        for(int i = 0; i < allLine.Length; i++)
        {
            lineText.text = allLine.Substring(0, i);
            yield return new WaitForSeconds(typeDelay);
        }

        yield return new WaitForSeconds(delay);
        ShowChatBox(false);
        yield return new WaitForSeconds(0.1f);
        Commander.instance.IsSpeaking = false;
    }

    public void FixedSpeak(string sentence)
    {
        if (Commander.instance.IsSpeaking)
        {
            return;
        }

        Commander.instance.IsSpeaking = true;
        ShowChatBox(true);

        if (thisCharacter.IsAfflicted)
        {
            lineText.color = Color.red;
            StartCoroutine(SpeakRoutine(sentence));
        }
        else if (thisCharacter.IsVirtuous)
        {
            lineText.color = Color.yellow;
            StartCoroutine(SpeakRoutine(sentence));
        }
        else
        {
            lineText.color = Color.white;
            StartCoroutine(SpeakRoutine(sentence));
        }
    }
}

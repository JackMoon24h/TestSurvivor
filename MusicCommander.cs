using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCommander : MonoBehaviour {

    public static MusicCommander instance;
    private AudioSource audioSource;

    private void Awake()
    {
        MakeSingleton();
    }

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMusic(bool state)
    {
        if(state)
        {
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        } 
        else
        {
            if(audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public AudioClip mouseOver;
    public AudioClip click;
    public AudioSource soundPlayer;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void mouseEnter()
    {
        soundPlayer.PlayOneShot(mouseOver, 0.5f);
    }

    public void mouseDown()
    {
        soundPlayer.PlayOneShot(click, 1f);
    }
    
}

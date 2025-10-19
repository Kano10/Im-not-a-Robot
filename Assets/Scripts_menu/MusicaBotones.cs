using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicaBotones : MonoBehaviour
{
    private AudioSource music;
    public AudioClip ClickAudio;

    void Start(){
        music = GetComponent<AudioSource>();
    }

    public void ClickAudioOn(){
        music.PlayOneShot(ClickAudio);
    }
}

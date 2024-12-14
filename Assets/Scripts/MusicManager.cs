using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource audioSource;
    public List<AudioClip> music = new List<AudioClip>();
    public AudioClip selectionMusic;
    int musicIndex = 0;
    bool combatMusic = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
        audioSource.clip = selectionMusic;
        audioSource.Play();
    }
    public void StartCombatMusic()
    {
        System.Random random = new System.Random();
        musicIndex = random.Next(music.Count);
        audioSource.clip = music[musicIndex];
        audioSource.Play();
        combatMusic = true;
    }
    void Update()
    {
        if (combatMusic && !audioSource.isPlaying)
        {
            musicIndex = (musicIndex + 1) % music.Count;
            audioSource.clip = music[musicIndex];
            audioSource.Play();
        }
    }
}

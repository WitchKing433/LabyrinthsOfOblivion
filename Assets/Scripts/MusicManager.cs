using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource audioSource;
    public List<AudioClip> music = new List<AudioClip>();
    public AudioClip selectionMusic;
    public AudioClip endMusic;
    int musicIndex = 0;
    bool combatMusic = false;
    bool selectMusic = true;
    bool dragonborn = false;
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
    public void StartEndMusic()
    {
        combatMusic = false;
        audioSource.clip = endMusic;
        audioSource.Play();
    }
    void Update()
    {
        if (combatMusic && !audioSource.isPlaying)
        {
            musicIndex = (musicIndex + 1) % music.Count;
            audioSource.clip = music[musicIndex];
            audioSource.Play();
        }
        if (dragonborn && !audioSource.isPlaying)
        {
            audioSource.clip = endMusic;
            audioSource.Play();
        }
        if (selectMusic && !audioSource.isPlaying)
        {
            audioSource.clip = selectionMusic;
            audioSource.Play();
        }
    }
}

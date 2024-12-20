using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    public float duration = 0f;
    Image image;
    int currentFrame = 0;
    float frameRate = 0f;
    float timer = 0f;
    public bool endless = false;
    void Start()
    {
        image = GetComponent<Image>();
        frameRate = duration / sprites.Length;
        image.sprite = sprites[currentFrame];
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame++;
            if (currentFrame == sprites.Length) 
            {
                if (!endless)
                {
                    Destroy(this.gameObject);
                }
                else if (endless)
                {
                    currentFrame %= sprites.Length;
                }                
            }
            if (currentFrame < sprites.Length)
            {
                image.sprite = sprites[currentFrame];
            }
        }
    }
}

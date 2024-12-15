using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Animation : MonoBehaviour
{
    public GameObject[] sprites;
    public float duration;
    int currentFrame = 0;
    float frameRate;
    float timer = 0f;
    public bool endless;
    void Start()
    {
        frameRate = duration / sprites.Length;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            sprites[currentFrame].SetActive(false);
            currentFrame ++;
            if (!endless && currentFrame == sprites.Length)
            {
                Destroy(this.gameObject);
            }
            else if (endless)
            {
                currentFrame %= sprites.Length;
            }
            if(currentFrame < sprites.Length)
                sprites[currentFrame].SetActive(true);
        }        
    }
}

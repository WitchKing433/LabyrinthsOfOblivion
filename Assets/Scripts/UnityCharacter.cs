using ClassLibraryMazeGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCharacter : MonoBehaviour
{
    public GameObject scroll;
    public ClassCharacter daedra;
    public UnityEngine.Color color;
    void Start()
    {
        

    }
    void Update()
    {
        if (daedra.Name == "Hermaeus Mora" && color == null)
            color = new UnityEngine.Color(6, 113, 0);
    }
    public void OnClick()
    {
        if(daedra.owner == Factory.game.turn)
        {
            Factory.game.SelectCharacter(daedra);
            scroll.GetComponent<CharacterScroll>().activeCharacter = this.gameObject;
        }
    }
}

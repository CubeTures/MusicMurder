using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightCone : MonoBehaviour
{
    PlayerTempo playerTempo;
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite small;
    [SerializeField] Sprite medium;
    [SerializeField] Sprite large;

    // Start is called before the first frame update
    void Awake()
    {
        playerTempo = GameObject.Find("Canvas/Accuracy").GetComponent<PlayerTempo>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerTempo.getStealth() <= 2){
            spriteRenderer.sprite = large;
        }else if(playerTempo.getStealth() == 8){
            spriteRenderer.sprite = small;
        }else{
            spriteRenderer.sprite = medium;
        }
    }
}

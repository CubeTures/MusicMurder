using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dizzy : MonoBehaviour
{
    [SerializeField] Sprite[] sprites = new Sprite[4];
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(animate());
    }

    private IEnumerator animate(){
        while(true){
            for(int i = 0; i < sprites.Length; i++){
                spriteRenderer.sprite = sprites[i];

                yield return new WaitForSeconds(0.075f);
            }
        }
    }
}

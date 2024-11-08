using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] Sprite[] sprites = new Sprite[6];
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(animate());
    }

    private IEnumerator animate(){
        for(int i = 0; i < sprites.Length; i++){
            spriteRenderer.sprite = sprites[i];

            yield return new WaitForSeconds(0.05f);
        }

        Destroy(gameObject);
    }
}

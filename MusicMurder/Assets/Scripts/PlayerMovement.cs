using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static System.Math;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private bool isMoving, canInput;
    private Vector2 currentTile, nextTile;
    int speed;
    static float t = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        currentTile = body.position;
        isMoving = false;
        canInput = true;
        speed = 100;
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        if((Math.Abs(vertical) > .2f || Math.Abs(horizontal) > .2f) && !isMoving && canInput){
            canInput = false;
            isMoving = true;
            currentTile.x = body.position.x;
            currentTile.y = body.position.y;
            if(Math.Abs(vertical) >= Math.Abs(horizontal)){
                nextTile.x = body.position.x;
                nextTile.y = body.position.y + (vertical > 0 ? 1 : -1);
            }else{
                nextTile.x = body.position.x + (horizontal > 0 ? 1 : -1);
                nextTile.y = body.position.y;
            }
        }

        if(isMoving){
            body.position = new Vector2(Mathf.Lerp(currentTile.x, nextTile.x, t), Mathf.Lerp(currentTile.y, nextTile.y, t));
            t += 0.1f * Time.deltaTime * speed;
        }

        if(body.position == nextTile){
            isMoving = false;
            t = 0.0f;
            if(Math.Abs(vertical) <= .2f && Math.Abs(horizontal) <= .2f){
                canInput = true;
            }
            Debug.Log(horizontal);
        }
    }
}

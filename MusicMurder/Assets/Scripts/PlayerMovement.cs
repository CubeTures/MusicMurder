using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static System.Math;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private bool isMoving;
    private Vector2 currentTile, nextTile, input;
    int speed = 100;
    static float t = 0.0f; // Evan, why is t static?

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        currentTile = body.position;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            input.y = 1;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            input.y = -1;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            input.x = -1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            input.x = 1;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        SetNextTile();
        MoveToNextTile();
        CheckEndMove();
    }

    void SetNextTile()
    {
        if ((Abs(input.y) > .2f || Abs(input.x) > .2f) && !isMoving)
        {
            isMoving = true;
            currentTile = body.position;

            if (Abs(input.y) >= Abs(input.x))
            {
                nextTile.x = body.position.x;
                nextTile.y = body.position.y + (input.y > 0 ? 1 : -1);
            }
            else
            {
                nextTile.x = body.position.x + (input.x > 0 ? 1 : -1);
                nextTile.y = body.position.y;
            }

            input = Vector2.zero;
        }
    }

    void MoveToNextTile()
    {
        if (isMoving)
        {
            body.position = Vector2.Lerp(currentTile, nextTile, t);
            t += 0.1f * Time.fixedDeltaTime * speed;
        }
    }

    void CheckEndMove()
    {
        if (Equals(body.position, nextTile))
        {
            isMoving = false;
            t = 0.0f;
        }
    }
}

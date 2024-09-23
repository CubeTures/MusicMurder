using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static System.Math;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    private Rigidbody2D body;
    const string wallTag = "Walls";

    [SerializeField] private bool isMoving;
    private Vector2 currentTile, nextTile, input;
    int speed = 100;
    float t = 0.0f;
    // Evan, why is t static? I'm going to assume that was not intentional and make it not static...

    public delegate void PlayerAction(PlayerActionType actionType, float timestamp);
    PlayerAction onPlayerAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Player Instance not Null");
        }
    }

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
            NotifyOnPlayerAction(PlayerActionType.MOVE);

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckWallCollision(collision);
    }

    void CheckWallCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
        {
            nextTile = currentTile;
        }
    }

    public void ListenOnPlayerAction(PlayerAction p)
    {
        onPlayerAction += p;
    }

    public void UnlistenOnPlayerAction(PlayerAction p)
    {
        onPlayerAction -= p;
    }

    void NotifyOnPlayerAction(PlayerActionType actionType)
    {
        float timestamp = Time.time;
        foreach(PlayerAction p in onPlayerAction.GetInvocationList())
        {
            p.Invoke(actionType, timestamp);
        }
    }
}

public enum PlayerActionType
{
    MOVE,
    ATTACK,
}

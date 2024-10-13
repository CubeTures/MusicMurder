using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static System.Math;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    const string wallTag = "Walls";

    [SerializeField] protected bool isMoving;
    public Vector2 currentTile {get; private set;}
    private Vector2 nextTile;
    float t = 0.0f;
    [SerializeField]protected int health;

    protected Vector2 direction;
    protected int speed = 100;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTile = rb.position;
        isMoving = false;
    }

    private void FixedUpdate()
    {
        CheckMove();
        MoveToNextTile();
        CheckEndMove();
    }

    void CheckMove()
    {
        if ((Abs(direction.y) > .2f || Abs(direction.x) > .2f) && !isMoving)
        {
            OnMove();
            SetNextTile();            
        }
    }

    virtual protected void OnMove() { }

    void SetNextTile()
    {
        isMoving = true;
        currentTile = rb.position;

        if (Abs(direction.y) >= Abs(direction.x))
        {
            nextTile.x = rb.position.x;
            nextTile.y = (float)Round((rb.position.y + (direction.y > 0 ? 1 : -1))*2)/2;
        }
        else
        {
            nextTile.x = (float)Round((rb.position.x + (direction.x > 0 ? 1 : -1))*2)/2;
            nextTile.y = rb.position.y;
        }

        nextTile = SnapTile(nextTile);
        direction = Vector2.zero;
    }

    Vector2 SnapTile(Vector2 tile)
    {
        return new Vector2(
                Mathf.Floor(tile.x) + 0.5f,
                Mathf.Floor(tile.y) + 0.5f
            );
    }

    void MoveToNextTile()
    {
        if (isMoving)
        {
            rb.position = Vector2.Lerp(currentTile, nextTile, t);
            t += 0.1f * Time.fixedDeltaTime * speed;
        }
    }

    void CheckEndMove()
    {
        if (Equals(rb.position, nextTile))
        {
            isMoving = false;
            t = 0.0f;
            currentTile = nextTile;
        }
    }

    public void TakeDamage(int damage){
        health-=damage;
    }

    public int GetHealth(){
        return health;
    }

    public void CancelMove()
    {
        nextTile = currentTile;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        CheckWallCollision(collision);
    }

    void CheckWallCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
        {
            CancelMove();
        }
    }
}

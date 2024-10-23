using UnityEngine;

using static System.Math;

public abstract class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    const string wallTag = "Walls";

    [SerializeField] protected bool isMoving;
    [SerializeField] public Vector2 currentTile { get; private set; }
    [SerializeField] private Vector2 nextTile;
    float t = 0.0f;
    public bool colliding;

    protected Vector2 direction;
    protected int speed = 100;

    Metronome metronome;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTile = rb.position;
        isMoving = false;
        colliding = false;
        metronome = Metronome.Instance;
        SetListenStatus(true);
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

    protected abstract void OnMove();

    void SetNextTile()
    {
        isMoving = true;
        currentTile = rb.position;

        if (Abs(direction.y) >= Abs(direction.x))
        {
            nextTile.x = rb.position.x;
            nextTile.y = (float)Round((rb.position.y + (direction.y > 0 ? 1 : -1)) * 2) / 2;
        }
        else
        {
            nextTile.x = (float)Round((rb.position.x + (direction.x > 0 ? 1 : -1)) * 2) / 2;
            nextTile.y = rb.position.y;
        }

        nextTile = SnapTile(nextTile);
        direction = Vector2.zero;
    }

    protected virtual void RemoveFromMap()
    {

    }

    protected virtual void RemoveFromMapPrime()
    {

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
        if (colliding)
        {
            CancelMoveCollide();
            colliding = false;
        }
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
            //if(isMoving){
            RemoveFromMap();
            //}
            isMoving = false;
            t = 0.0f;
            currentTile = nextTile;
            direction = Vector2.zero;
        }
    }

    public void CancelMove()
    {
        if(direction != Vector2.zero)
            RemoveFromMapPrime();
        else
            RemoveFromMap();
        isMoving = true;
        nextTile = currentTile;
        direction = Vector2.zero;
    }

    public void CancelMoveCollide()
    {
        isMoving = true;
        nextTile = currentTile;
        direction = Vector2.zero;
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

    public Vector2 getNext()
    {
        return new Vector2(currentTile.x + direction.x, currentTile.y + direction.y);
    }

    public Vector2 getNextPrime()
    {
        return new Vector2(nextTile.x, nextTile.y);
    }

    protected abstract void OnMetronomeBeat(float timestamp, float nextBeatTimestamp, bool startup);

    private void OnEnable()
    {
        SetListenStatus(true);
    }

    private void OnDisable()
    {
        SetListenStatus(false);
    }

    private void OnDestroy()
    {
        SetListenStatus(false);
    }

    void SetListenStatus(bool status)
    {
        if (metronome != null)
        {
            if (status)
            {
                metronome.ListenOnMetronomeBeat(OnMetronomeBeat);
            }
            else
            {
                metronome.UnlistenOnMetronomeBeat(OnMetronomeBeat);
            }
        }
    }
}

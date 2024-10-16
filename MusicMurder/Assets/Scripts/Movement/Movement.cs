using UnityEngine;

using static System.Math;

public abstract class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    const string wallTag = "Walls";

    [SerializeField] protected bool isMoving;
    public Vector2 currentTile {get; private set;}
    private Vector2 nextTile;
    float t = 0.0f;

    protected Vector2 direction;
    protected int speed = 100;

    Metronome metronome;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTile = rb.position;
        isMoving = false;
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

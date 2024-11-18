using UnityEngine;

public class TrySpawnEnemy : MonoBehaviour
{
    [SerializeField] Vector2[] attemptPositions;
    [SerializeField] GameObject enemy;
    readonly Vector2 size = new Vector2(.2f, .2f);

    public void TrySpawn()
    {
        foreach (Vector2 pos in attemptPositions)
        {
            if (PositionAvailable(pos))
            {
                Spawn(pos);
                break;
            }
        }
    }

    bool PositionAvailable(Vector2 pos)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(pos, size, 0);

        return hits.Length == 0;
    }

    void Spawn(Vector2 pos)
    {
        Instantiate(enemy, pos, Quaternion.identity, transform);
    }
}

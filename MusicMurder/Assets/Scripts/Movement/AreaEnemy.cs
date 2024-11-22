using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreaEnemy : Enemy
{
    protected Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    protected int areaAttackLife = 2;
    protected int attackCooldown = 1;
    protected int areaWidth = 5, areaHeight = 2;
    protected AreaAttackPattern area;
    protected string attackPattern = "x x x" +
                                     " x x ";
    [SerializeField] GameObject projctile;

    AudioSource audio;
    protected int audioCountdown = -1;

    protected new void Start()
    {
        area = new AreaAttackPattern(areaHeight, areaWidth, attackPattern);
        base.Start();
        audio = GetComponent<AudioSource>();
    }

    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup){
        base.OnMetronomeBeat(timestamp, failTimestamp, nextBeatTimestamp, startup);
        if(audioCountdown == 1){
            audio.Play();
        }
        audioCountdown--;
    }

    protected override void Move()
    {
        if (PlayerIsInLine(areaHeight, areaWidth) is Vector2 direction)
        {
            Attack(direction);
        }
        else
        {
            SetDirectionFromPathfinding();
        }
    }

    protected virtual void Attack(Vector2 direction)
    {
        List<Vector2> positions = area.GetPositions(transform.position, direction);
        CreateDamageTiles(positions);
        audioCountdown = areaAttackLife;
    }

    protected void CreateDamageTiles(HashSet<Vector2> positions)
    {
        CreateDamageTiles(positions.ToList());
    }

    protected void CreateDamageTiles(List<Vector2> positions)
    {
        foreach (Vector2 position in positions)
        {
            GameObject tileObj = Instantiate(projctile, position, Quaternion.identity, transform);
            DamageTile tile = tileObj.GetComponent<DamageTile>();
            tile.Setup(areaAttackLife);
        }

        cooldown = attackCooldown;
    }
}

public interface DamageTileCreator
{
    public void TileDestroyed(DamageTile tile);
}
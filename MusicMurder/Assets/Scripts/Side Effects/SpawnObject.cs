using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] GameObject spawnObject;
    [SerializeField] Vector2 spawnPosition;
    [SerializeField] bool spawnAtTransform;

    public void Spawn()
    {
        if(spawnAtTransform)
        {
            Instantiate(spawnObject, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(spawnObject, spawnPosition, Quaternion.identity);
        }
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCameraBounds : MonoBehaviour
{
    const string boundsTag = "Bounds";

    CinemachineConfiner confiner;
    Collider2D bounds;

    private void Awake()
    {
        SetBounds();
    }

    void SetBounds()
    {
        confiner = GetComponent<CinemachineConfiner>();
        GameObject _bounds = GameObject.FindGameObjectWithTag(boundsTag);
        bounds = _bounds.GetComponent<Collider2D>();
        confiner.m_ConfineMode = CinemachineConfiner.Mode.Confine2D;
        confiner.m_BoundingShape2D = bounds;
    }
}

using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    Transform transformB;

    Vector3 pointA;
    Vector3 pointB;

    void Start()
    {
        pointA = transform.position;
        pointB = transformB.position;
    }

    public Vector3 PointA
    {
        get { return pointA; }
    }

    public Vector3 PointB
    {
        get { return pointB; }
    }
}

using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float mSpeed;

    Vector3 point1;
    Vector3 point2;
    Vector3 vector;

    Transform playerTransform;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    void Start ()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        point1 = transform.position;
        point2 = playerTransform.position;

        vector = point2 - point1; //to extend the journey length beyon the player position

        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(transform.position, playerTransform.position);
    }

    private void Update()
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * mSpeed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        //transform.right is a vector 3
        transform.position = Vector3.Lerp(point1, point2 + (vector*2), fractionOfJourney);
    }
}

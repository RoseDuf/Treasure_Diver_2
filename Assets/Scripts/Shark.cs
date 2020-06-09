using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    public Vector3 point1;
    public Vector3 point2;
    Vector3 pointB;
    Vector3 pointA;
    [SerializeField] float range;

    //these will be according to level
    float mSpeed;
    float mScale;
    float mMinSpeed;
    float mMaxSpeed;

    //desired shark sizes
    [SerializeField] float mMinScale;
    [SerializeField] float mMaxScale;

    [SerializeField] private float turnSpeed;

    //Things we need to animate
    Animator mAnimator;
    [SerializeField] Rigidbody mRigidBody;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    Transform playerPos;
    bool sharkIsFull;

    // Start is called before the first frame update
    void Start()
    {
        //point1 = pointA;
        pointB = point2;

        //speeds according to level+1 or +2
        mMinSpeed = Levels.levels + (1*5);
        mMaxSpeed = Levels.levels + (2*5);
        //random speeds and scale
        mSpeed = Random.Range(mMinSpeed, mMaxSpeed);
        mScale = Random.Range(mMinScale, mMaxScale);
        transform.localScale *= mScale;
        mAnimator = GetComponent<Animator>();

        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(point1, point2);
        
        sharkIsFull = false;

        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        //when item is dropped, follow item
        if (Item.itemDropped == true)
        {
            Vector3 toItem = transform.position - Item.item.position;
            if (toItem.magnitude <= range && !sharkIsFull)
            {
                point2 = Item.item.position;
                transform.LookAt(Item.item);
                transform.position += transform.forward * 5 * Time.deltaTime; //slow down speed
            }
        }

        //if player in range, follow player
        Vector3 toPlayer = transform.position - playerPos.position;
        if (toPlayer.magnitude <= range && !sharkIsFull && Item.itemDropped == false && Score.amountOfGold >= 15)
        {
            point2 = playerPos.position;
            transform.LookAt(playerPos);
            transform.position += transform.forward * 5 * Time.deltaTime; //slow down speed
        }

        //if shark went off original path, update points for linear interpolation
        if (point2 != pointB)
        {
            point2 = pointB;
            pointA = transform.position;
            point1 = pointA;

            // Keep a note of the time the movement started.
            startTime = Time.time;
            // reCalculate the journey length.
            journeyLength = Vector3.Distance(point1, point2);
        }
        
        //linear interpolation default movement
        if (point2 == pointB)
        {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startTime) * mSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            //transform.right is a vector 3
            transform.position = Vector3.Lerp(point1, point2, fractionOfJourney);

            Vector3 direction = (point1 - point2).normalized;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-direction), turnSpeed * Time.deltaTime);
        }
        
        //Destroy when shark reaches target point
        if (transform.position == pointB)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Drop")
        {
            Item.itemDropped = false;
            sharkIsFull = true;
        }
        if (other.tag == "Player")
        {
            sharkIsFull = true;
        }
    }

    public Vector3 Point1
    {
        get { return point1; }
        set { point1 = value; }
    }
    public Vector3 Point2
    {
        get { return point2; }
        set { point2 = value; }
    }
}

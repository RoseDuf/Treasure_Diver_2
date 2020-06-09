using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    //SPIDER PREFAB

    //Variables to move object
    [SerializeField] float mSpeed;
    [SerializeField] Rigidbody mRigidBody;
    [SerializeField] ParticleSystem dustparticles;
    [SerializeField] ParticleSystem dustSpawn;

    //Face direction variables
    Vector2 mFacingDirection;
    Vector2 forward;
    Vector3 point1;
    Vector3 point2;
    [SerializeField] private float turnSpeed;
    private float nextPointTimer;
    [SerializeField] float pointDelay;
    bool mMoving;

    //spawnpoint coordinate info
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minZ;
    [SerializeField] float maxZ;

    //Timer Variables
    public static int objectsDestroyed;

    //Animator variables
    public static bool mSpawning;
    Animator mAnimator;
    
    float nextTimeToSpawn;

    [SerializeField] float dieTimer;
    float timeToDie;

    Transform playerTransform;
    Transform spiderSize;
    Vector3 tempSpiderSize;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    private void Start()
    {
        mAnimator = GetComponent<Animator>();
        mSpawning = true;

        //spawning scale
        spiderSize = transform;
        tempSpiderSize = spiderSize.localScale;
        spiderSize.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        //timer
        nextTimeToSpawn = SpiderSpawner.nextTimeToSpawn + 4f;
        nextPointTimer = 0f;
        timeToDie = Time.time + dieTimer;

        //transforms
        point1 = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //particles
        dustparticles.Stop();
        dustSpawn.Play();
    }

    // Update is called once per frame
    void Update()
    {
        DustParticles();
        FaceDirection();
        CheckSpawning();
        ChangeDirection();
        CheckIfTimeToDie();
    }

    private void CheckSpawning()
    {
        if (nextTimeToSpawn <= Time.time && mSpawning)
        {
            dustSpawn.Stop();
            spiderSize.localScale = tempSpiderSize;
            mSpawning = false;
        }
        else if (nextTimeToSpawn > Time.time)
        {
            dustSpawn.Play();
            spiderSize.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            mSpawning = true;
        }

        mAnimator.SetBool("mSpawning", mSpawning);

        if (transform.position.x != point2.x && transform.position.z != point2.z)
        {
            mMoving = true;
        }
        else
        {
            mMoving = false;
        }

        mAnimator.SetBool("mMoving", mMoving);

    }

    private void DustParticles()
    {
        if (mMoving)
        {
            dustparticles.Play();
        }
        if (!mMoving || mSpawning)
        {
            dustparticles.Stop();
        }
    }

    private void ChangeDirection()
    {
        if (!mSpawning)
        {
            if (nextPointTimer <= Time.time) 
            {
                SetDestination();

                point1 = transform.position;

                // Keep a note of the time the movement started.
                startTime = Time.time;

                // Calculate the journey length.
                journeyLength = Vector3.Distance(point1, point2);

                nextPointTimer = Time.time + pointDelay;
            }

            if (transform.position.x != point2.x && transform.position.z != point2.z && timeToDie > Time.time)
            {
                // Distance moved equals elapsed time times speed..
                float distCovered = (Time.time - startTime) * mSpeed;

                // Fraction of journey completed equals current distance divided by total distance.
                float fractionOfJourney = distCovered / journeyLength;

                //transform.right is a vector 3
                transform.position = Vector3.Lerp(point1, point2, fractionOfJourney);
            }
        }
    }

    private void FaceDirection()
    {
        if (transform.position.x != point2.x && transform.position.z != point2.z && timeToDie > Time.time)
        {
            Vector3 direction = (transform.position - point2).normalized;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);

            //so that spider wont rotate on x and z axis
            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.x = 0;
            currentRotation.z = 0;
            transform.rotation = Quaternion.Euler(currentRotation);
        }
    }

    private void SetDestination()
    {
        float coordX = Random.Range(minX, maxX);
        float coordZ = Random.Range(minZ, maxZ);

        point2 = new Vector3(coordX, transform.position.y , coordZ);
    }

    private void CheckIfTimeToDie()
    {
        if (transform.localScale.magnitude <= 0.1f)
        {
            objectsDestroyed += 1;
            Destroy(gameObject);
        }

        if (timeToDie <= Time.time)
        {
            //shrink              
            transform.localScale -= new Vector3(0.06f, 0.06f, 0.06f); //may need to modify this if the scale is different
            mAnimator.SetBool("mDying", true);
        }
    }
}

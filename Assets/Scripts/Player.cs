using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[RequireComponent(typeof (CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_SwimSpeed;
    [SerializeField] private float m_JumpSpeed;
    [SerializeField] float downSpeed;
    [SerializeField] private float m_GravityMultiplier;
    [SerializeField] private float cameraSensitivity;
    [SerializeField] private float cameraSmooth;
    [SerializeField] private Transform cameraDirection;
    [SerializeField] private Camera m_Camera;
    private Vector2 cameraDir;
    Rigidbody m_RigidBody;
    
    private bool m_Jump;
    private bool m_Swimming;
    private bool m_IsWalking;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private bool m_PreviouslyGrounded; //makes sure player is still grounded over small bumps
    private Vector3 m_OriginalCameraPosition;

    bool mHurt;
    bool inWater;

    //Weight variables
    float weight = 0;
    float weight_modifier = 0;

    // Invincibility timer
    float kInvincibilityDuration;
    float mInvincibleTimer;
    bool mInvincible;

    //oxygen timer
    float second;

    // Damage effects
    [SerializeField] float knockbackForce;
    [SerializeField] float knockbackTime;
    float knockbackCounter;
    Vector3 direction;

    //Lives
    public static int extraLives;

    // Reference to audio sources
    AudioSource mCoinSound;
    AudioSource mJetPackSound;
    AudioSource mTakeDamageSound;
    AudioSource mEnemySound;
    AudioSource mNitroSound;
    
    //Nitro Cylinder variables
    public static bool nitroCylinderActivate;
    bool timerActivated;

    // Use this for initialization
    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_CharacterController = GetComponent<CharacterController>();

        //m_Camera = Camera.main;
        m_OriginalCameraPosition = m_Camera.transform.localPosition;
        Cursor.lockState = CursorLockMode.Locked; // hide cursor
        m_Swimming = false;
        inWater = true;

        //initialization
        timerActivated = false;
        nitroCylinderActivate = false;
        extraLives = 2;

        // Get audio references
        AudioSource[] audioSources = GetComponents<AudioSource>();
        mCoinSound = audioSources[0];
        mJetPackSound = audioSources[1];
        mTakeDamageSound = audioSources[2];
        mEnemySound = audioSources[3];
        mNitroSound = audioSources[4];
    }
        
    // Update is called once per frame
    private void Update()
    {
        //increase weight the more gold you gather
        weight = Mathf.Clamp(Score.amountOfGold * 0.1f, 0f, 1f);
        weight_modifier = (1f + weight);

        ShowCursor();
        RotateView();
        JumpInput();
        CheckIfHurt();
        ResetOxygens();
        OxygenDepletion();
        NitroCylinder();
        MoveCharacter();
    }
    
    //invincibility stops after a number of seconds of knockback
    void CheckIfHurt()
    {
        if (knockbackCounter <= 0)
        {
            mInvincible = false;
        }
        else
        {
            mInvincible = true;
            knockbackCounter -= Time.deltaTime;
        }
    }

    //If you lost all oxygen but you still have extra lives left, reset oxygens to 2.
    void ResetOxygens()
    {
        if (LifeCounter.oxygens == 0 && !mInvincible)
        {
            Die();
            if (extraLives > -1)
            {
                LifeCounter.oxygens = 2;
            }
        }
    }

    //If you lose all oxygen, but still have extra lives, just spawn on boat
    //else you go to game over screen
    void Die()
    {
        Score.amountOfGold = 0; //lose the gold you've gathered
        extraLives -= 1;
        if (extraLives < 0)
        {
            //Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
        }
        transform.position = new Vector3(0f, 72.0f, 0f);
        Debug.Log("You have died.");
    }

    //All of the Triggers
    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Shark" && !mInvincible)
        {
            mEnemySound.Play();
            LifeCounter.oxygens -= 1;
            if (LifeCounter.oxygens >= 0)
            {
                TakeDamage(collision.transform.position);
            }
        }
        if (collision.tag == "Spider" && !mInvincible && !Spider.mSpawning)
        {
            mEnemySound.Play();
            LifeCounter.oxygens -= 1;
            if (LifeCounter.oxygens >= 0)
            {
                TakeDamage(collision.transform.position);
            }
        }
        if (collision.tag == "Gold1")
        {
            Destroy(collision.gameObject);
            Score.amountOfGold += 1;
            mCoinSound.Play();
        }
        if (collision.tag == "Gold2")
        {
            Destroy(collision.gameObject);
            Score.amountOfGold += 2;
            mCoinSound.Play();
        }
        if (collision.tag == "Gold3")
        {
            Destroy(collision.gameObject);
            Score.amountOfGold += 10;
            mCoinSound.Play();
        }
        if (collision.tag == "Edge1")
        {
            transform.position = new Vector2(-4.5f, transform.position.y);
        }
        if (collision.tag == "Edge2")
        {
            transform.position = new Vector2(19f, transform.position.y);
        }
        if (collision.tag == "AboveWater")
        {
            inWater = false;
        }
        if (collision.tag == "Water")
        {
            inWater = true;
            m_MoveDir = Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }
    }

    //simple force that applies when and enemy touches you. Alse sets invincibility to true
    public void TakeDamage(Vector3 enemyPosition)
    {
        if (!mInvincible)
        {
            knockbackCounter = knockbackTime;
            mTakeDamageSound.Play();
            direction = (transform.position - enemyPosition).normalized;
            mInvincible = true;
        }
    }

    void MoveCharacter()
    {
        ////Determine movement speed
        float moveSpeed;

        GetInput();

        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                            m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        //If you're invincible/hurt, you cant move
        if (!mInvincible)
        {
            moveSpeed = m_CharacterController.isGrounded ? m_WalkSpeed : m_SwimSpeed;
            
            //horizontal movement
            m_MoveDir.x = desiredMove.x * moveSpeed;
            m_MoveDir.z = desiredMove.z * moveSpeed;

            //Check if the player wants Player to swim (see input manager)
            //and set the value of "mSwimming" accordingly
            if (inWater && m_Jump) //check if in water, can swim if outside of water.
            {
                //Swimming functionality
                //LikeBalloon fight, add force upwards, need to keep pressing space bar.
                m_MoveDir.y = m_JumpSpeed/ weight_modifier;
                mJetPackSound.Play(); //bloop
                m_Jump = false;
                m_Swimming = true;
            }
        }
        else
        {
            moveSpeed = 0;
            m_MoveDir = direction * knockbackForce;
        }

        //apply gravity
        if (nitroCylinderActivate || !inWater)
        {
            m_MoveDir += Physics.gravity * 4f * Time.fixedDeltaTime;
        }
        else
        {
            m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }

        //update controller movement, camera position and cursor 
        if (transform.position != new Vector3(0f, 72f, 0f)) //stop moving momentarily (for teleportation when dying)
        {
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
        }
        else //allow movement again
        {
            transform.position = new Vector3(0f, 71.5f, 0f);
        }
        
        UpdateCameraPosition(moveSpeed);
    }

    //jump input method
    void JumpInput()
    {
        if (!mInvincible && (!m_Jump || m_Swimming) && inWater)
        {
            m_Jump = Input.GetButtonDown("Jump");
        }

        if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
        {
            m_MoveDir.y = 0f;
            m_Swimming = false;
        }
        if (!m_CharacterController.isGrounded && !m_Swimming && m_PreviouslyGrounded)
        {
            m_MoveDir.y = 0f;
        }

        m_PreviouslyGrounded = m_CharacterController.isGrounded;
    }
    
    //camera position method
    private void UpdateCameraPosition(float speed)
    {
        m_Camera.transform.localPosition = m_Camera.transform.localPosition;
    }

    //player movement input
    private void GetInput()
    {
        // Read input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
            
        m_Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }
    }

    //camera roation method
    private void RotateView()
    {
        Vector2 mouseDir = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //sensitivity
        mouseDir = Vector2.Scale(mouseDir, new Vector2(cameraSensitivity, cameraSensitivity));

        //smooth
        Vector2 lookDelta = new Vector2();
        lookDelta.x = Mathf.Lerp(lookDelta.x, mouseDir.x, 1.0f / cameraSmooth);
        lookDelta.y = Mathf.Lerp(lookDelta.y, mouseDir.y, 1.0f / cameraSmooth);
        cameraDir += lookDelta;

        cameraDir.y = Mathf.Clamp(cameraDir.y, -75.0f, 75.0f);

        m_Camera.transform.localRotation = Quaternion.AngleAxis(-cameraDir.y, Vector3.right);

        //rotate player
        this.transform.localRotation = Quaternion.AngleAxis(cameraDir.x, this.transform.up);
    }

    //make cursor invisible
    void ShowCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
    }

    //oxygen depletion
    private void OxygenDepletion()
    {
        if (inWater)
        {
            second += Time.deltaTime; //count 1 second
            if(second >= 1)
            {
                OxygenTimer.oxygenLeft -= 1; //remove 1 oxygen per second
                second = 0; //reset second
            }
        }
        else
        {
            OxygenTimer.oxygenLeft = 60;
        }

        if (OxygenTimer.oxygenLeft <= 0)
        {
            LifeCounter.oxygens -= 1;
            OxygenTimer.oxygenLeft = 60;
        }
    }

    //Nitro Cylinder functions will follow
    //this is the overall control of nitroCylinders
    void NitroCylinder()
    {
        Vector3 targetDir = cameraDirection.position - transform.position;

        if (Input.GetButton("NitroBoost") && Vector3.Dot(targetDir, Vector3.up) < -1f)
        {
            mNitroSound.Play(); //play sound
            nitroCylinderActivate = true; //nitro cylinder is activated
        }
        else
        {
            nitroCylinderActivate = false;
        }
    }
}


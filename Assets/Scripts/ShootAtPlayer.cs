using UnityEngine;
using System.Collections;

public class ShootAtPlayer : MonoBehaviour
{
    [SerializeField] Projectile projectilePrefab;
    Transform playerTransform;
    [SerializeField] float mFireCooldown;
    [SerializeField] float range;
    float timer;
    bool cooldownActive;

    void Start ()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update ()
    {
        Vector3 toPlayer = transform.position - playerTransform.position;
        if(!cooldownActive)
        {
            if (toPlayer.magnitude <= range)
            {
                Fire ();
            }
        }

        if(cooldownActive)
        {
            timer -= Time.deltaTime;
            if(timer <= 0.0f)
            {
                cooldownActive = false;
            }
        }
    }

    void Fire()
    {
        if (!Spider.mSpawning)
        {
            cooldownActive = true;
            timer = mFireCooldown;
            Instantiate(projectilePrefab, transform.position + new Vector3(0f, 5f, 0f), transform.rotation);
        }
    }
}

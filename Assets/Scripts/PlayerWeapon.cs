using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] GameObject item;
    [SerializeField] float mFireCooldown;
    [SerializeField] float mSpeed;

    float timer;
    bool cooldownActive;

    public static Vector3 direction;

    private void Start()
    {
        direction = transform.position;
    }

    void Update ()
    {
        direction = transform.position;

        if (!cooldownActive && Input.GetButtonDown("Fire1"))
        {
            cooldownActive = true;
            timer = mFireCooldown;
            GameObject projectile = Instantiate (item, transform.position, transform.rotation) as GameObject;
            projectile.transform.position = transform.position + Camera.main.transform.forward * 2;
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = Camera.main.transform.forward * mSpeed;

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
}

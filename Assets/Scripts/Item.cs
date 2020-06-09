using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    [SerializeField]
    float mLifespan;
    float timer = 0.0f;

    public static Transform item;
    public static bool itemDropped;

    private void Start()
    {
        item = transform;
        itemDropped = true;
    }

    void Update ()
    {
        timer += Time.deltaTime;
        if (timer > mLifespan)
        {
            itemDropped = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shark")
        {
            itemDropped = false;
            Destroy(gameObject);
        }
    }
}

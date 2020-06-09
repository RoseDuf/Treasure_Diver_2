using UnityEngine;

public class Gold : MonoBehaviour
{
    //GOLD TIMER

    [SerializeField]
    float upTime;

    float nextTimeToDestroy;

    private void Start()
    {
        nextTimeToDestroy = Time.time + upTime;
    }

    private void Update() {
        if (nextTimeToDestroy <= Time.time)
        {
            Destroy(gameObject);
            nextTimeToDestroy = Time.time + upTime;
        }
    }
}

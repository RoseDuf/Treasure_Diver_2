using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class ChangePostProcess : MonoBehaviour
{
    [SerializeField]
    PostProcessingProfile normal, fx;
    [SerializeField]
    Camera camera;

    PostProcessingBehaviour camImageFx;

    // Start is called before the first frame update
    void Start()
    {
        camImageFx = camera.GetComponent<PostProcessingBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camImageFx.profile = fx;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camImageFx.profile = normal;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{

    AudioSource mPointSound;
    public static int CurrentScore;

    private void Start()
    {
        CurrentScore = 0;
        // Get audio references
        AudioSource[] audioSources = GetComponents<AudioSource>();
        mPointSound = audioSources[0];
    }

    //On collision (not trigger) collect gold and update current score
    //Collision trigger why? I needed the tag of this collider to be "Above Water"
    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("YOU GOT SOME GOLD");
        if (collision.tag == "Player")
        {
            if(Score.amountOfGold > 0)
            {
                mPointSound.Play();
            }
            CurrentScore += Score.amountOfGold;
            Score.amountOfGold = 0;
        }
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name); //this is to reload the scene
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    //ESCAPE BUTTON

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            //Application.LoadLevel (0);
            SceneManager.LoadScene("MainMenu");
        }
    }
}

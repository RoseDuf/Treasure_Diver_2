using UnityEngine;
using UnityEngine.UI;

public class ExtraLives : MonoBehaviour
{
    //DISPLAYS EXTRA LIVES

    [SerializeField]
    Text livesText;

    // Start is called before the first frame update
    void Start()
    {
        livesText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        livesText.text = "Lives: " + Player.extraLives.ToString();
    }
}

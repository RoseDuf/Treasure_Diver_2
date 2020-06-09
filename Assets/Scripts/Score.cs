using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    //DISPLAY SCORE

    public static int amountOfGold;

    [SerializeField]
    Text scoreText;

    private void Start()
    {
        amountOfGold = 0;
        scoreText = GetComponent<Text>();
    }

    private void Update()
    {
        scoreText.text = "Points: " + Goal.CurrentScore.ToString();
    }
}

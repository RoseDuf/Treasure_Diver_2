using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmountOfGold : MonoBehaviour
{
    [SerializeField]
    Text amountOfGold;

    private void Start()
    {
        amountOfGold = GetComponent<Text>();
    }

    private void Update()
    {
        amountOfGold.text = "Amount of Gold: " + Score.amountOfGold.ToString();
    }
}

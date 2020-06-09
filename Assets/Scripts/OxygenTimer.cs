using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenTimer : MonoBehaviour
{
    //OXYGEN DISPLAY

    //levels
    public static int oxygenLeft;

    [SerializeField]
    Text oxygenText;

    private void Start()
    {
        oxygenText = GetComponent<Text>();

        //initialise
        oxygenLeft = 60;
    }

    // Update is called once per frame
    void Update()
    {
        oxygenText.text = "Oxygen: " + oxygenLeft.ToString();
    }
}

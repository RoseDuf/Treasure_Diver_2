using UnityEngine;

public class LifeCounter : MonoBehaviour
{
    //OXYGEN DISPLAY

    [SerializeField]
    GameObject oxygen1;
    [SerializeField]
    GameObject oxygen2;

    public static int oxygens;

    // Start is called before the first frame update
    void Start()
    {
        oxygens = 2;
        oxygen1.gameObject.SetActive(true);
        oxygen2.gameObject.SetActive(true);
    }

    //Show the number of oxygen tanks on display according to the number of oxygens you should have
    void Update()
    {
        switch (oxygens)
        {
            case 2:
                oxygen1.gameObject.SetActive(true);
                oxygen2.gameObject.SetActive(true);
                break;
            case 1:
                oxygen1.gameObject.SetActive(true);
                oxygen2.gameObject.SetActive(false);
                break;
            case 0:
                oxygen1.gameObject.SetActive(false);
                oxygen2.gameObject.SetActive(false);
                break;
        }
    }
}

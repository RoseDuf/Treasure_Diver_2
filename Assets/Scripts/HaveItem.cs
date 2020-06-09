using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HaveItem : MonoBehaviour
{
    [SerializeField]
    Text haveItem;

    private void Start()
    {
        haveItem = GetComponent<Text>();
    }

    private void Update()
    {
        if (!Item.itemDropped)
        {
            haveItem.text = "Item: Yes";
        }
        else
        {
            haveItem.text = "Item: No";
        }
    }
}

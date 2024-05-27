using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipmentItemUI : MonoBehaviour
{
    public Button button;
    public TMP_Text itemName;
    public Image itemImage;
    public InventoryItem item;
    public int amt;

    public void loadUI(UnityEngine.Events.UnityAction buttonAction)
    {
        button.onClick.RemoveAllListeners();
        itemName.text = item.obj.objectName + " x" + amt;
        itemImage.sprite = item.obj.uiImage;
        button.onClick.AddListener(buttonAction);
    }
}

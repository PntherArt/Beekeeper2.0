using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CatalogItemUI : MonoBehaviour
{
    public CatalogObject catalogObject;
    public Image itemDisplayImage;
    public TMP_Text itemNameText;
    public TMP_Text priceText;
    public int amt;
    public GameObject timesFive;

    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {

    }

    public void updateButtons()
    {
        Button x5Button = timesFive.GetComponent<Button>();
        // not interactable if not enough money
        button.interactable = GlobalVariables.money >= catalogObject.buyValue;
        x5Button.interactable = GlobalVariables.money >= catalogObject.buyValue * 5;
    }

    public void loadUI(UnityEngine.Events.UnityAction buttonAction, bool displayAmt = true, bool isCart = false)
    {
        // set up main button
        button.onClick.RemoveAllListeners();
        itemDisplayImage.sprite = catalogObject.uiImage;
        itemNameText.text = (displayAmt) ? catalogObject.name + " x" + amt : catalogObject.name;
        priceText.text = "$" + catalogObject.buyValue;
        button.onClick.AddListener(buttonAction);

        // set up x5 button
        Button x5Button = timesFive.GetComponent<Button>();
        x5Button.onClick.RemoveAllListeners();
        for (int i = 0; i < 5; i++)
            x5Button.onClick.AddListener(buttonAction);





    }
}

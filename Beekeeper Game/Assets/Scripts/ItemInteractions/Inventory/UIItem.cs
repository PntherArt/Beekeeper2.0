using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItem : MonoBehaviour
{
    public InventoryItem item;
    public TMP_Text slotText;
    private Image sprite;

    private void Awake()
    {
        sprite = GetComponent<Image>();
        UpdateItem(null);
    }

    public void UpdateItem(InventoryItem item)
    {
        this.item = item;

        if (this.item != null)
        {
            sprite.color = Color.white;
            sprite.sprite = this.item.obj.uiImage;
        }
        else
        {
            Color spritelessColor = Color.white;
            spritelessColor.a = 0f;
            sprite.color = spritelessColor;
            sprite.sprite = null;
        }
    }

    public void SetActiveSlot()
    {
        Color activeSlotColor = Color.white;
        //activeSlotColor.a = 0.2f;
        this.transform.parent.gameObject.GetComponent<Image>().color = activeSlotColor;
    }

    public void UnsetActiveSlot()
    {
        Color defaultSlotColor = Color.white;
        defaultSlotColor.a = 0.5f;
        this.transform.parent.gameObject.GetComponent<Image>().color = defaultSlotColor;
    }

    public void UpdateSlotText(int numItems)
    {
        slotText.text = numItems.ToString();
    }
}

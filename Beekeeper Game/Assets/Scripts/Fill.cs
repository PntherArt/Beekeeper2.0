using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fill : MonoBehaviour
{
    public Beehive hive;
    public ProductObj product;
    private float baseOffset;

    // Start is called before the first frame update
    void Start()
    {
        Image image = GetComponent<Image>();
        image.color = product.color;

        RectTransform rectTransform = GetComponent<RectTransform>();
        baseOffset = rectTransform.anchoredPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (hive) {
            if (hive.productDict.ContainsKey(product)){
                UpdateFill(hive.productDict[product]);
            } else {
                UpdateFill(0f);
            }
        }
    }

    void UpdateFill(float fullness) 
    {
        float height = fullness * 618f;
        float heightOffset = ((618f - height) / 2f) * 0.31f;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, baseOffset - heightOffset);
    }
}

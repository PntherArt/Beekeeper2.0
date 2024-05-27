using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIndicatorUI : MonoBehaviour
{
    Animator animator;
    public bool usesTexture = false;
    public Sprite usedTexture;
    public Image buttonBackground;
    public Image buttonIndicatorImage;
    public TMP_Text buttonText;
    public bool opened { get; private set; } = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (usesTexture)
        {
            buttonText.gameObject.SetActive(false);
            buttonIndicatorImage.gameObject.SetActive(true);
            buttonIndicatorImage.sprite = usedTexture;
        } else
        {
            buttonText.gameObject.SetActive(true);
            buttonIndicatorImage.gameObject.SetActive(false);
        }
    }

    private void OnValidate()
    {
        if (usesTexture)
        {
            buttonText.gameObject.SetActive(false);
            buttonIndicatorImage.gameObject.SetActive(true);
            buttonIndicatorImage.sprite = usedTexture;
        }
        else
        {
            buttonText.gameObject.SetActive(true);
            buttonIndicatorImage.gameObject.SetActive(false);
        }
    }

    public void open()
    {
        animator.SetBool("on", true);
        opened = true;
    }

    public void close()
    {
        animator.SetBool("on", false);
        opened = false;
    }

    public void setText(string text)
    {
        buttonText.text = text;
    }

    public void setSprite(Sprite sprite)
    {
        usedTexture = sprite;
    }

    // face the player
    void Update()
    {
        if (buttonBackground.color.a != 0)
        {
            Vector3 lookDir = transform.position - Camera.main.transform.position;

            lookDir.Normalize();
            GetComponent<RectTransform>().rotation = Quaternion.LookRotation(lookDir);
        }
        
    }
}

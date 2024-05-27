using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Alert : MonoBehaviour
{
    public string alertText = "[alert]";
    private const float alertLength = 1;
    public AnimationClip alertDisplayAnimation;
    public AnimationClip alertHideAnimation;
    public TMP_Text alertTextField;


    RectTransform rectTransform;
    Animator animator;


    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
    }

    public void setText(string _alertText)
    {
        alertText = _alertText;
        gameObject.name = _alertText;
        alertTextField.text = alertText;
    }

    public void display()
    {
        animator.Play("AlertDisplay");
        Debug.Log("Alert: " + alertText);
    }

    public IEnumerator afterDisplay()
    {
        yield return new WaitForSeconds(alertLength);
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip != alertHideAnimation)
            animator.Play("AlertHide");
    }


    public float getTotalScreenTime()
    {
        return alertDisplayAnimation.length + alertLength + alertHideAnimation.length;
    }
}

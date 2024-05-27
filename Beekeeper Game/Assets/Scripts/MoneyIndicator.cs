using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyIndicator : MonoBehaviour
{
    TMP_Text moneyText;

    private void Start()
    {
        moneyText = GetComponent<TMP_Text>();
        updateUI();
    }

    public void updateUI()
    {
        moneyText.text = "Money: " + GlobalVariables.money;
    }
}

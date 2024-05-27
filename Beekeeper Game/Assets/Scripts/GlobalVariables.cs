using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalVariables : MonoBehaviour
{
    public static int money = 100;
    public static int day = 1;

    public UnityEvent onChangeMoney;

    public void changeMoney(int amt)
    {
        money += amt;
        onChangeMoney.Invoke();
    }
}

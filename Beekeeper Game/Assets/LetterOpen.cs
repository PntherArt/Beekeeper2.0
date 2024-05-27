using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterOpen : MonoBehaviour
{

    public GameObject letterPage;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    public void Opened()
    {
        letterPage.SetActive(true);
    }

    public void Closed()
    {
        letterPage.SetActive(false);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookScripts : MonoBehaviour
{

    public GameObject Cover;
    public GameObject[] pages;
    int pageID;

    RaycastHit hit;


    private void Start()
    {
        Cover.GetComponent<GameObject>();
        pages[0].GetComponent<GameObject>();
        pages[1].GetComponent<GameObject>();
        pages[2].GetComponent<GameObject>();
        
    }


    private void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100.0f))
        {
            if (hit.collider.CompareTag("Book") && Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(BookCov());
            }
        }
            
    }

    public void RedTab()
    {
        pageID = 0;

        for (int i = 0; i < pages.Length; i++)
        {
            if (i == pageID)
            {
                pages[i].SetActive(false);
            }
            else
            {
                pages[i].SetActive(false);
            }

        }
        pages[pageID].SetActive(true);
    }

    public void YellowTab()
    {
        pageID = 1;

        for (int i = 0; i < pages.Length; i++)
        {
            if (i == pageID)
            {
                pages[i].SetActive(false);
            }
            else
            {
                pages[i].SetActive(false);
            }

        }
        pages[pageID].SetActive(true);


    }

    public void LightBlueTab()
    {
        pageID = 2;

        for (int i = 0; i < pages.Length; i++)
        {
            if (i == pageID)
            {
                pages[i].SetActive(false);
            }
            else
            {
                pages[i].SetActive(false);
            }

        }
        pages[pageID].SetActive(true);
    }

    public void CloseBook()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if (i == pageID)
            {
                pages[i].SetActive(false);
            }
            else
            {
                pages[i].SetActive(false);
            }

        }
    }


    IEnumerator BookCov()
    {
        Cover.SetActive(true);
        yield return new WaitForSeconds(2f);
        Cover.SetActive(false);
        RedTab();
    }


}

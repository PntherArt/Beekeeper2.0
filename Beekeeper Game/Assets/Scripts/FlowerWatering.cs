using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowerWatering : MonoBehaviour
{
    [Header("Watering Checkboxes")]
    public bool isWatered = false;
    public bool hasWaterCan = false;
    public bool isCanEmpty = false;
    private bool hasAlerted = false;

    [Header("Game Object References")]
    public GameObject pickUp;

    [Header("Watering Can Capacity + Press E")]
    public int waterLevel;
    private int defWaterLevel;

    [Header("Water Capacity UI")]
    public Slider uiWaterIndicator;
    public GameObject uiWaterSlider;

    [Header("Alerts")]
    public AlertManager alertManager;

    RaycastHit hit;


    private void Start()
    {
        pickUp.GetComponent<GameObject>();
        defWaterLevel = waterLevel;
        uiWaterSlider.GetComponent<GameObject>();
        uiWaterIndicator.GetComponent<Slider>().maxValue = waterLevel;
        uiWaterIndicator.GetComponent<Slider>().value = waterLevel;

    }


    private void Update()
    {

        uiWaterIndicator.GetComponent<Slider>().value = waterLevel;

        if (waterLevel == 0)
        {
            isCanEmpty = true;
            
            if(isCanEmpty == true && hasAlerted == false)
            {
                alertManager.queueAlert("Water Can is empty!");
                hasAlerted = true;

            }
        }
        
        if(pickUp.transform.childCount == 0)
        {
            hasWaterCan = false;
            uiWaterSlider.SetActive(false);
        }

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100.0f) && pickUp.transform.childCount != 0)
            {

                if (pickUp.transform.GetChild(0).CompareTag("Water Can") == true)
                {
                    uiWaterSlider.SetActive(true);    

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (hit.collider.CompareTag("River"))
                        {
                            print("River!");
                            FillTheCan();
                        }

                        if (hit.collider.CompareTag("Flower"))
                        {

                            print("Hit flower");
                            if (waterLevel != 0)
                            {
                                print(hit.transform.gameObject.name);

                                hasWaterCan = true;

                                if (isWatered == false)
                                {
                                    //pickUp.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                                    waterLevel--;
                                    StartCoroutine(FlowerSparkle());
                                }

                            print("Watering Active");
                        }
                    }

                } else if (pickUp.transform.GetChild(0).CompareTag("Water Can") == false)
                {
                    uiWaterSlider.SetActive(false);
                    hasWaterCan = false;
                }


            }

            

        }

    }


    public void FillTheCan()
    {
        if (pickUp.transform.GetChild(0).CompareTag("Water Can"))
        {
            hasWaterCan = true;

            if (Input.GetKey(KeyCode.E))
            {
                waterLevel = defWaterLevel;
                isCanEmpty = false;
                hasAlerted = false;
            }

        }

        if (pickUp.transform.GetChild(0).CompareTag("Water Can") == false)
        {
            hasWaterCan = false;
        }
    }


    IEnumerator FlowerSparkle()
    {
        pickUp.transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(3f);
        hit.transform.GetChild(0).transform.gameObject.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(5f);
        isWatered = false;

    }

}

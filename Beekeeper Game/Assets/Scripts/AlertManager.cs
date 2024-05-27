using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    public GameObject alertPrefab;
    public int edgePadding = 30;

    private Queue<Alert> alertQueue = new Queue<Alert>();
    private RectTransform rectTransform;

    bool displayed = false;

    private void OnGUI()
    {
        if (GUILayout.Button("Queue Test Alert"))
        {
            queueAlert("test alert!");
        }
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void queueAlert(string _alertText)
    {
        GameObject clone = Instantiate(alertPrefab, Vector3.zero, Quaternion.identity, transform);

        Alert alert = clone.GetComponent<Alert>();
        alert.setText(_alertText);

        alertQueue.Enqueue(alert);
        clone.SetActive(false);

        //Debug.Log("num of alerts in queue: " + alertQueue.Count);
    }

    public IEnumerator displayAlert(Alert alert)
    {
        float totalTime = alert.getTotalScreenTime();
        alert.display();
        yield return new WaitForSeconds(totalTime);
        Destroy(alert.gameObject);
        displayed = false;
    }

    void Update()
    {
        if (!displayed && alertQueue.Count != 0)
        {
            displayed = true;

            // dequeue the oldest queued alert
            Alert head = alertQueue.Dequeue();
            head.gameObject.SetActive(true);
            RectTransform alertRect = head.gameObject.GetComponent<RectTransform>();
            float vertSize = alertRect.sizeDelta.y;

            // set vertical pos
            float offset = edgePadding + vertSize * 0.5f;
            alertRect.anchoredPosition = new Vector2(0, Display.main.systemHeight * 0.5f - offset);

            // play alert
            StartCoroutine(displayAlert(head));
        }
    }
}

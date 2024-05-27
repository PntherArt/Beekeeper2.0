using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class JarIndicatorFill : MonoBehaviour
{
    public Beehive hive;
    private float height;

    void Start() {
        SpriteRenderer spriteRenderer  = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(221f/255f, 154f/255f, 24f/255f);
    }

    void Update()
    {
        UpdateFill(Mathf.Max(hive.productDict.Values.ToArray()));
    }

    void UpdateFill(float fullness) 
    {
        transform.localScale = new Vector3(1f, fullness, 1f);
        transform.localPosition = new Vector3(0, -3 * (1 - fullness), 0);
    }
}

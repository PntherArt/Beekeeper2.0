using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Flower : MonoBehaviour
{

    // The kind of this flower
    public FlowerObject flowerInfo;

    // The amount of product in this flower
    [SerializeField] float product = 0.005f;
    public float Nectar // read only public access to product
    {
        get => product;
        set
        {
            product = Mathf.Clamp(value, 0, flowerInfo.productPerDay);
        }
    }
    private void Awake()
    {
        this.gameObject.tag = "Flower";
    }
    private void OnEnable()
    {
        Instantiate(flowerInfo.flowerMesh, this.transform.position, Quaternion.AngleAxis(Random.Range(0, 90), Vector3.up), this.transform);
        // full of polen
        regenProduct();
    }

    // regen product in this flower back to max value
    public void regenProduct()
    {
        product = flowerInfo.productPerDay;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = flowerInfo.product.color;
        Vector3 flowerHeadPos = transform.position + new Vector3(0, flowerInfo.height, 0);
        Gizmos.DrawLine(transform.position, flowerHeadPos);
        Gizmos.DrawWireSphere(flowerHeadPos, flowerInfo.radius);
    }
}

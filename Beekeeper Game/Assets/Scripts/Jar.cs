using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jar : MonoBehaviour
{
    public float productAmt { get; private set; } = 0;
    public ProductObj productType { get; private set; } = null;
    public GameObject inside;



    void updateMat()
    {
        Material jarMat = inside.GetComponent<MeshRenderer>().material;
        if (productType != null)
        {
            jarMat.SetColor("_SideColor", productType.color);
            jarMat.SetColor("_TopColor", productType.color * 1.5f);
            jarMat.SetFloat("_Fill", Mathf.Clamp01(productAmt));
        }
        else
        {
            jarMat.SetColor("_SideColor", Color.white);
            jarMat.SetColor("_TopColor", Color.white);
            jarMat.SetFloat("_Fill", 0);
        }
    }

    public void setProduct(ProductObj setType, float setAmt)
    {
        productAmt = setAmt;
        productType = setType;
        updateMat();
    }

    public bool isNonEmpty()
    {
        return productType != null && productAmt != 0;
    }

    public (ProductObj, float) extractProduct()
    {
        if (productType != null)
        {
            (ProductObj, float) holding = (productType, productAmt);
            productType = null;
            productAmt = 0;
            updateMat();
            return holding;
        }
        return (null, 0);
    }

    public (ProductObj, float) getProductNoMutate() {
        if (productType != null)
        {
            return (productType, productAmt);
        }
        return (null, 0);
    }
}

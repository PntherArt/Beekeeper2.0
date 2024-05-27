using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisibleObject", menuName = "Beekeeper Game/VisibleObject", order = 0)]
public class VisibleObject : ScriptableObject
{
    // Abstract class for scriptable objects that have images
    public int id;
    public Sprite uiImage;
    public string objectName;
}

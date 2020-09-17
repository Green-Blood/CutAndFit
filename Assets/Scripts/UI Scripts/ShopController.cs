using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{

}

[System.Serializable]
public class Skin
{
    public int id, price;
    public bool isUnlocked;
    public GameObject lockObject;
    public Sprite skinSprite;
    public Image skin, selectedFrame;
    public Text priceText;
}

[System.Serializable]
public class Cutter
{
    public int id, price;
    public bool isUnlocked;
    public GameObject lockObject;
    public Sprite cutterSprite;
    public Image cutter, selectedFrame;
    public Text priceText;
}
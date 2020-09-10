using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CutterSkin", menuName = "Cutter/Cutter Skin", order = 1)]
public class CutterData : ScriptableObject
{
    public GameObject[] cutters;
    public Sprite icon;
    public int limit;
}

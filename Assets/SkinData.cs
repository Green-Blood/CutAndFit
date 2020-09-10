using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinData", menuName = "Skins/Skin", order = 1)]
public class SkinData : ScriptableObject
{
    public GameObject[] environment;
    public int shapeId;
    public Mesh hopheyMesh;
    public Material hopheyBodyMat;
    public Material hopheyCapMat;
    public Material particleMat;
    public AudioClip cutfx;
    public Sprite icon;
    public Sprite iconBg;
    public Color feverGlowColor = new Color(1f,1f,1f,1f);
}
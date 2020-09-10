using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "LevelData", menuName = "Leveling/Data", order = 1)]
public class LevelData : ScriptableObject
{
    public Mesh hopheyMesh;
    public Material[] hopheyMat;
    public Material handleMat;
    public Material particleMat;
    public AudioClip cutfx;
    public Sprite icon;
    [Header("Level1")]
    public Color groundColor;
    public Color gradientColor;
    public Color darkGradientColor;
    [Header("Level2")]
    public Color groundColor1;
    public Color gradientColor1;
    public Color darkGradientColor1;
    [Header("Level3")]
    public Color groundColor2;
    public Color gradientColor2;
    public Color darkGradientColor2;
    [Header("Fever")]
    public Color groundFeverColor;
    public Color gradientFeverColor;
    public Color darkGradientFeverColor;
    public Color feverFrameColor;
    public Color feverGlowColor;
    [Header("Other")]
    public Color accentColor;
    public Color slideColor;
    public Color levelTextColor;

    public Color feverSlide;
}
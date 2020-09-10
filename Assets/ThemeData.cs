using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "ThemeData", menuName = "Theme/Data", order = 1)]
public class ThemeData : ScriptableObject
{
    public string levelSelector;
    public GameObject[] environment;
    [Header("Level1")]
    public Color groundColor = new Color(1,1,1,1);
    public Color gradientColor = new Color(1, 1, 1, 1);
    public Color darkGradientColor = new Color(1, 1, 1, 1);
    [Header("Level2")]
    public Color groundColor1 = new Color(1, 1, 1, 1);
    public Color gradientColor1 = new Color(1, 1, 1, 1);
    public Color darkGradientColor1 = new Color(1, 1, 1, 1);
    [Header("Level3")]
    public Color groundColor2 = new Color(1, 1, 1, 1);
    public Color gradientColor2 = new Color(1, 1, 1, 1);
    public Color darkGradientColor2 = new Color(1, 1, 1, 1);
    [Header("Fever")]
    public Color groundFeverColor = new Color(1, 1, 1, 1);
    public Color gradientFeverColor = new Color(1, 1, 1, 1);
    public Color darkGradientFeverColor = new Color(1, 1, 1, 1);
    public Color feverFrameColor = new Color(1, 1, 1, 1);
    public Color feverGlowColor = new Color(1, 1, 1, 1);
    [Header("Other")]
    public Color accentColor = new Color(1, 1, 1, 1);
    public Color slideColor = new Color(1, 1, 1, 1);
    public Color levelTextColor = new Color(1, 1, 1, 1);

    public Color feverSlide = new Color(1, 1, 1, 1);
    public Color equipTextColor = new Color(1, 1, 1, 1);
}

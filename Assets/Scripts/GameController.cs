using System;
using System.Collections;
using System.Collections.Generic;
using UI_Scripts;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static bool canLoadLevel = true;
    public static bool isMainMenu = true;
    public static GameController gameController;
    public GameObject holeSlider;
    public GameObject holeEffect;
    public static SkinData currentSkin;
    public static CutterData currentCutter;
    public static ThemeData currentTheme;
    public static Material[] mats;
    //[HideInInspector]
    public static int levelVisuals;
    //[HideInInspector]
    public static int cutterVisuals;
    //[HideInInspector]
    public static int themeVisuals;
    public SkinData[] skins;
    public CutterData[] cutters;
    public ThemeData[] themes;
    public Transform[] handlers;
    public GameObject hophey;
    public GameObject cutter;
    public MeshRenderer hopheyMesh;
    public MeshCollider hopheyCollider;
    public Material groundMat;
    public SpriteRenderer gradient;
    public SpriteRenderer[] darkGradient;
    public ParticleSystem chopParticle;
    public GameObject groundObj;
    public GameObject mainMenu;
    public GameObject subMenu;
    public GameObject gameplayMenu;
    public GameObject cutterShop;
    public GameObject skinShop;
    public GameObject cutterToggle;
    public GameObject skinToggle;
    public Animation[] cutAnim;
    public Button nextLvlBtn;
    private bool _isNextLvl = true;
    [Header("For Level Designer")] public bool canMove = true;
    public float holeSize = 1.5f;
    public float comboHole = 0.0f;
    public int comboCount = 0;
    [Header("Level Settings")] public LevelsData levelsData;
    public LevelSettings currentLvl;
    [HideInInspector]
    public static int currentLvlNumber = 0;

    [Header("UI")] public Image prgImg;
    [SerializeField] private ProgressBar progressBarScript;
    [SerializeField] private ComboPoints comboPointsScript;


    private void Awake()
    {
        if(canLoadLevel)
            currentLvlNumber = PlayerPrefs.GetInt("currentLvl");
        // Should be changed
        currentLvl = levelsData.levels[currentLvlNumber];

        gameController = this;
        holeSize = currentLvl.startrHoleSize;
        comboHole = currentLvl.startrHoleSize;
        Application.targetFrameRate = 60;
        levelVisuals = PlayerPrefs.GetInt("visual");
        themeVisuals = UnityEngine.Random.Range(0, themes.Length);
        //levelVisuals = 2;
        //cutterVisuals = 7;
        currentTheme = themes[themeVisuals];
        currentSkin = skins[levelVisuals];
        currentCutter = cutters[cutterVisuals];
        if (currentLvl.levelTyp == LevelTyp.LimitedCut)
            currentLvl.limitScene = currentCutter.limit;
        hophey.GetComponent<MeshFilter>().mesh = currentSkin.hopheyMesh;
        hopheyCollider.sharedMesh = currentSkin.hopheyMesh;
        mats = hopheyMesh.materials;
        mats[0] = skins[levelVisuals].hopheyCapMat;
        mats[1] = skins[levelVisuals].hopheyBodyMat;
        hopheyMesh.materials = mats;
        chopParticle.GetComponent<ParticleSystemRenderer>().material = currentSkin.particleMat;

        int envId = UnityEngine.Random.Range(0, skins[levelVisuals].environment.Length);
        GameObject env1 = Instantiate(skins[levelVisuals].environment[envId], Vector3.zero, Quaternion.identity);
        env1.transform.parent = groundObj.transform;
        env1.transform.localPosition = Vector3.zero;
        //GameObject env2 = Instantiate(skins[levelVisuals].environment[envId], Vector3.zero, Quaternion.identity);
        //env2.transform.parent = groundObj.transform;
        //env2.transform.localPosition = new Vector3(0, 0, -11f);
        //GameObject env3 = Instantiate(skins[levelVisuals].environment[envId], Vector3.zero, Quaternion.identity);
        //env3.transform.parent = groundObj.transform;
        //env3.transform.localPosition = new Vector3(0, 0, -22f);

        for (int i = 0; i < handlers.Length; i++)
        {
            cutter = Instantiate(cutters[cutterVisuals].cutters[skins[levelVisuals].shapeId]);
            cutter.transform.parent = handlers[i];
            cutter.transform.localPosition = Vector3.zero;
            cutter.transform.localScale = Vector3.one;
            cutter.transform.localEulerAngles = Vector3.zero;
            cutAnim[i] = cutter.GetComponent<Animation>();
        }

        groundMat.SetColor("_Color", themes[themeVisuals].groundColor);
        gradient.color = themes[themeVisuals].darkGradientColor;
        foreach (SpriteRenderer sr in darkGradient)
        {
            sr.color = themes[themeVisuals].darkGradientColor;
        }
    }

    private void Start()
    {
        prgImg.fillAmount = 0.0f;
        comboCount = 1;
        _isNextLvl = true;
        if (isMainMenu)
        {
            if (currentLvlNumber == 0)
            {
                PlayerPrefs.SetInt("currentLvl", 1);
                currentLvlNumber = 1;
            }
        }
    }

    private void Update()
    {
        holeSlider.transform.position =
            new Vector3(Mathf.Lerp(holeSlider.transform.position.x, holeSize + 5f, Time.deltaTime * 5),
                holeSlider.transform.position.y, holeSlider.transform.position.z);
        holeEffect.transform.localScale =
            new Vector3(Mathf.Lerp(holeEffect.transform.localScale.x, holeSize, Time.deltaTime * 5),
                holeEffect.transform.localScale.y, holeEffect.transform.localScale.z);
        //if (ObjectMover.hopCount >= 10)
        //    Debug.LogError("You Lose! (hop count exceeded)");
        //if (ObjectMover.sliceCount >= 10)
        //    Debug.LogError("You Lose! (slice count exceeded)");

        if (prgImg.fillAmount >= 0.99f && _isNextLvl)
        {
            _isNextLvl = false;
            NextLevel();
        }
    }

    public void IncreaseMoveCount(LevelTyp typ)
    {
        if (currentLvl.levelTyp == typ)
            if (--currentLvl.limitScene <= 0)
                canMove = false;
    }

    public bool isIncreace = false;

    public void StartIncereaceHole()
    {
        if (isIncreace)
        {
            holeSize = ObjectMover.cutSize;
            if (holeSize < 2f)
                holeSize = 2f;
            isIncreace = false;
        }

//        StartCoroutine(IncereaceHole());
    }

    public IEnumerator IncereaceHole()
    {
        yield return new WaitForSeconds(0.2f);
        if (isIncreace)
        {
            holeSize = ObjectMover.cutSize;
            isIncreace = false;
        }
    }


    public void ShowPg(float cutSize)
    {
        float s = (1 + ((float) comboCount * (float) 0.1)) / currentLvl.cutN * cutSize / comboHole;
        prgImg.fillAmount += s;
        comboHole = holeSize;
    }

    #region Arbuz's part

    public void NextLevel()
    {
        canMove = false;
        Debug.Log("Congratulations");
        currentLvl = levelsData.levels[currentLvlNumber++];
        PlayerPrefs.SetInt("currentLvl", currentLvlNumber);
        PlayerPrefs.Save();
        nextLvlBtn.gameObject.SetActive(true);
        ObjectMover.cutSize = 0f;
        ObjectMover.cutSum = 0f;
        //progressBarScript.UpdateProgressBar();
    }

    #endregion

    public void NextLvlButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DeleteKeys()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OpenCutterShop(Boolean isOn)
    {
        if (isOn)
        {
            if (skinShop.activeInHierarchy)
            {
                skinShop.SetActive(false);
                skinToggle.GetComponent<Toggle>().isOn = false;
                skinToggle.transform.DOMoveY(-44f, 0.5f).SetEase(Ease.InOutCubic);
            }
            cutterToggle.transform.DOMoveY(0f, 0.5f).SetEase(Ease.InOutCubic);
            subMenu.SetActive(false);
            cutterShop.SetActive(true);
        }
        else
        {
            cutterToggle.transform.DOMoveY(-44f, 0.5f).SetEase(Ease.InOutCubic);
            cutterShop.SetActive(false);
            subMenu.SetActive(true);
        }
    }
    public void OpenSkinShop(Boolean isOn)
    {
        if (isOn)
        {
            if (cutterShop.activeInHierarchy)
            {
                cutterShop.SetActive(false);
                cutterToggle.GetComponent<Toggle>().isOn = false;
                cutterToggle.transform.DOMoveY(-44f, 0.5f).SetEase(Ease.InOutCubic);
            }
            skinToggle.transform.DOMoveY(0f, 0.5f).SetEase(Ease.InOutCubic);
            subMenu.SetActive(false);
            skinShop.SetActive(true);
        }
        else
        {
            skinToggle.transform.DOMoveY(-44f, 0.5f).SetEase(Ease.InOutCubic);
            skinShop.SetActive(false);
            subMenu.SetActive(true);
        }
    }

    public void Equip()
    {

    }
}

public enum LevelTyp
{
    NotLimited,
    LimitedMove,
    LimitedCut
}

[System.Serializable]
public class LevelSettings
{
    public LevelTyp levelTyp;
    public int limitScene = 10;
    public float startrHoleSize = 2.5f;
    public float deltaSize = 0.1f;
    public float comboRange = 0.1f;
    public int cutN = 5;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController gameController;
    public GameObject holeSlider;
    public GameObject holeEffect;
    public static SkinData currentSkin;
    public static CutterData currentCutter;
    public static ThemeData currentTheme;
    public int levelVisuals;
    public int cutterVisuals;
    public int themeVisuals;
    public SkinData[] skins;
    public CutterData[] cutters;
    public ThemeData[] themes;
    public Transform[] handlers;
    public GameObject hophey;
    public MeshRenderer hopheyMesh;
    public MeshCollider hopheyCollider;
    public Material groundMat;
    public SpriteRenderer gradient;
    public SpriteRenderer[] darkGradient;
    public ParticleSystem chopParticle;
    public GameObject groundObj;
    public Animation[] cutAnim;
    [Header("For Level Designer")]
    public bool canMove = true;
    public float holeSize = 1.5f;
    public float comboHole = 0.0f;
    public int comboCount=0;
    public LevelSettings currentLvl;
    public Image prgImg;
    private void Awake()
    {
        gameController = this;
        holeSize = currentLvl.startrHoleSize;
        comboHole = currentLvl.startrHoleSize;
        Application.targetFrameRate = 60;
        currentTheme = themes[themeVisuals];
        currentSkin = skins[levelVisuals];
        currentCutter = cutters[cutterVisuals];
        if (currentLvl.levelTyp == LevelTyp.LimitedCut)
            currentLvl.limitScene = currentCutter.limit;
        hophey.GetComponent<MeshFilter>().mesh = currentSkin.hopheyMesh;
        hopheyCollider.sharedMesh = currentSkin.hopheyMesh;
        Material[] mats = hopheyMesh.materials;
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
            GameObject cutter = Instantiate(cutters[cutterVisuals].cutters[skins[levelVisuals].shapeId]);
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
    }
    private void Update()
    {
        holeSlider.transform.position = new Vector3(Mathf.Lerp(holeSlider.transform.position.x, holeSize + 5f, Time.deltaTime * 5), holeSlider.transform.position.y, holeSlider.transform.position.z);
        holeEffect.transform.localScale = new Vector3(Mathf.Lerp(holeEffect.transform.localScale.x, holeSize, Time.deltaTime * 5), holeEffect.transform.localScale.y, holeEffect.transform.localScale.z);
        //if (ObjectMover.hopCount >= 10)
        //    Debug.LogError("You Lose! (hop count exceeded)");
        //if (ObjectMover.sliceCount >= 10)
        //    Debug.LogError("You Lose! (slice count exceeded)");

    }
    public void increaseMoveCount(LevelTyp typ)
    {
        if(currentLvl.levelTyp == typ)
        if (--currentLvl.limitScene <= 0)
            canMove = false;
    }
    public bool isIncreace = false;
    public void startIncereaceHole()
    {
        if (isIncreace)
        {
            holeSize = ObjectMover.cutSize;
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
    public void showPG(float cutSize)
    {
        float s = (1+((float)comboCount*(float)0.1)) / currentLvl.levelN * cutSize / comboHole;
        prgImg.fillAmount += s;
        comboHole = holeSize;
        Debug.Log(comboCount);
    }
}

public enum LevelTyp
{
    notLimited,
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
    public int levelN = 5;
}
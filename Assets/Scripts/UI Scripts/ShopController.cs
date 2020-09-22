using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class ShopController : MonoBehaviour
{
    public List<Skin> skins = new List<Skin>();
    public List<Cutter> cutters = new List<Cutter>();

    public int selectedSkin;
    public int selectedCutter;
    public GameController gameController;

    private void Start()
    {
        Refresh();
    }

    void Refresh()
    {
        for (int i = 0; i < skins.Count; i++)
        {
            skins[i].skin.sprite = skins[i].skinSprite;
            if (PlayerPrefs.GetInt("skin" + i) == 1)
            {
                skins[i].isUnlocked = true;
                skins[i].lockObject.SetActive(false);
            }
            else
            {
                skins[i].priceText.text = skins[i].price + "";
            }
        }
        for (int i = 0; i < cutters.Count; i++)
        {
            cutters[i].cutter.sprite = cutters[i].cutterSprite;
            if (PlayerPrefs.GetInt("cutter" + i) == 1)
            {
                cutters[i].isUnlocked = true;
                cutters[i].lockObject.SetActive(false);
            }
            else
            {
                cutters[i].priceText.text = cutters[i].price + "";
            }
        }
        if (skins.Any(p => p.id == GameController.levelVisuals))
            skins.First(s => s.id == GameController.levelVisuals).selectedFrame.enabled = true;
        if (cutters.Any(p => p.id == GameController.cutterVisuals))
            cutters.First(s => s.id == GameController.cutterVisuals).selectedFrame.enabled = true;
    }

    public void BuyCutter(int id)
    {
        if (cutters[id].price <= GameController.gem)
        {
            PlayerPrefs.SetInt("cutter" + id, 1);
            cutters[id].isUnlocked = true;
            SelectCutter(id);
            GameController.gem -= cutters[id].price;
            PlayerPrefs.SetInt("gem", GameController.gem);
            Refresh();
        }
    }

    public void BuySkin(int id)
    {
        if (skins[id].price <= GameController.gem)
        {
            PlayerPrefs.SetInt("skin" + id, 1);
            skins[id].isUnlocked = true;
            Select(id);
            GameController.gem -= skins[id].price;
            PlayerPrefs.SetInt("gem", GameController.gem);
            Refresh();
        }
    }

    public void Select(int id)
    {
        if (skins.Any(p => p.id == GameController.levelVisuals))
        {
            skins.First(s => s.id == GameController.levelVisuals).selectedFrame.enabled = false;
        }

        if (skins.Any(p => p.id == id))
        {
            skins.First(s => s.id == id).selectedFrame.enabled = true;
        }
        selectedSkin = id;
        GameController.levelVisuals = id;
        PlayerPrefs.SetInt("visual", id);
        GameController.currentSkin = gameController.skins[GameController.levelVisuals];
        gameController.hophey.GetComponent<MeshFilter>().mesh = GameController.currentSkin.hopheyMesh;
        GameController.mats = gameController.hopheyMesh.materials;
        GameController.mats[0] = gameController.skins[GameController.levelVisuals].hopheyCapMat;
        GameController.mats[1] = gameController.skins[GameController.levelVisuals].hopheyBodyMat;
        gameController.hopheyMesh.materials = GameController.mats;
        Destroy(gameController.cutter);
        for (int i = 0; i < gameController.handlers.Length; i++)
        {
            gameController.cutter = Instantiate(gameController.cutters[GameController.cutterVisuals].cutters[gameController.skins[GameController.levelVisuals].shapeId]);
            gameController.cutter.transform.parent = gameController.handlers[i];
            gameController.cutter.transform.localPosition = Vector3.zero;
            gameController.cutter.transform.localScale = Vector3.one;
            gameController.cutter.transform.localEulerAngles = Vector3.zero;
            gameController.cutAnim[i] = gameController.cutter.GetComponent<Animation>();
        }
    }

    public void SelectCutter(int id)
    {
        if (cutters.Any(p => p.id == GameController.cutterVisuals))
        {
            cutters.First(s => s.id == GameController.cutterVisuals).selectedFrame.enabled = false;
        }

        if (cutters.Any(p => p.id == id))
        {
            cutters.First(s => s.id == id).selectedFrame.enabled = true;
        }
        selectedCutter = id;
        GameController.cutterVisuals = id;
        PlayerPrefs.SetInt("cuttervisual", id);
        GameController.currentCutter = gameController.cutters[GameController.cutterVisuals];
        Destroy(gameController.cutter);
        for (int i = 0; i < gameController.handlers.Length; i++)
        {
            gameController.cutter = Instantiate(gameController.cutters[GameController.cutterVisuals].cutters[gameController.skins[GameController.levelVisuals].shapeId]);
            gameController.cutter.transform.parent = gameController.handlers[i];
            gameController.cutter.transform.localPosition = Vector3.zero;
            gameController.cutter.transform.localScale = Vector3.one;
            gameController.cutter.transform.localEulerAngles = Vector3.zero;
            gameController.cutAnim[i] = gameController.cutter.GetComponent<Animation>();
            if (gameController.currentLvl.levelTyp == LevelTyp.LimitedCut)
                gameController.currentLvl.limitScene = GameController.currentCutter.limit;
        }
    }
}

[System.Serializable]
public class Skin
{
    public int id, price;
    public bool isUnlocked;
    public GameObject lockObject;
    public Sprite skinSprite;
    public Image skin, selectedFrame;
    public TextMeshProUGUI priceText;
    public string name;
}

[System.Serializable]
public class Cutter
{
    public int id, price;
    public bool isUnlocked;
    public GameObject lockObject;
    public Sprite cutterSprite;
    public Image cutter, selectedFrame;
    public TextMeshProUGUI priceText;
}
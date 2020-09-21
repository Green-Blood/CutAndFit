using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class ShopController : MonoBehaviour
{
    public List<Skin> skins = new List<Skin>();
    public List<Cutter> cutters = new List<Cutter>();

    public int selectedSkin;
    public int selectedCutter;
    public GameController gameController;

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
    public Text priceText;
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
    public Text priceText;
}
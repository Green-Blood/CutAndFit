using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UI_Scripts;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public GameObject hophey;
    public GameController gameController;
    public ComboPoints comboPointsScript;    
    public bool isCut = false;
    public static bool isLoose = false;
    public static bool cutTrigger = false;
    public static bool isStart = true;
    public static bool isPlaying = false;
    public static float cutSize = 0f;
    public static float cutSum = 0f;
   // public float holeSize = 0f;
    public ParticleSystem chopParticle;
    public static int hopCount = 0;
    public static int sliceCount = 0;
    public static int comboMeter = 0;
    private GameObject _cuttedHophey;
    bool canPerfect = true;
    public static bool isPerfect = false;
    public static float diff;

    private void Start()
    {
        if (!GameController.isMainMenu)
        {
            hophey.transform.position = new Vector3(hophey.transform.position.x, hophey.transform.position.y, hophey.transform.position.z);
            //holeSize = GameController.gameController.holeSize;
            if (isStart)
            {
                isStart = false;
                StartHop();
            }
            else if (!isLoose)
                StartCoroutine(StartHopRoutine());

            GameController.gameController.mainMenu.SetActive(false);
            GameController.gameController.gameplayMenu.SetActive(true);
        }
        else
        {

        }
    }
    void StartHop()
    {
        if (!isCut)
            hophey.transform.DOMoveX(0f, 1f).SetEase(Ease.InOutCubic).OnComplete(IncreaseHop);
    }

    void IncreaseHop()
    {
        if (GameController.gameController.canMove)
            if (!isCut)
            {
                canPerfect = true;
                hophey.transform.DOMoveX(cutSum + GameController.gameController.holeSize + 0.8f, 1.2f).SetEase(Ease.InOutCubic).OnComplete(DecreaseHop);
                hopCount++;
                GameController.gameController.StartIncereaceHole();
                //Debug.Log("Forward Hop: " + (cutSize + holeSize + 0.8f));
            }
    }

    void DecreaseHop()
    {

        if (!isCut)
        {
            canPerfect = false;
            hophey.transform.DOMoveX(cutSum, 1.2f).SetEase(Ease.InOutFlash).OnComplete(IncreaseHop);
            //Debug.Log("Backward Hop: " + (cutSize));
            GameController.gameController.IncreaseMoveCount(LevelTyp.LimitedMove);
        }
    }

    public void stopTween()
    {
        hophey.transform.DOKill(false);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Ground")
    //    {
    //        Debug.LogError("Bumm");
    //    }
    //}

    public void StartRound()
    {
        hophey.transform.position = new Vector3(hophey.transform.position.x, hophey.transform.position.y, hophey.transform.position.z);
        //holeSize = GameController.gameController.holeSize;
        if (isStart)
        {
            isStart = false;
            StartHop();
        }
        else if (!isLoose)
            StartCoroutine(StartHopRoutine());

        GameController.gameController.mainMenu.SetActive(false);
        GameController.gameController.gameplayMenu.SetActive(true);
        GameController.isMainMenu = false;
        isPlaying = true;
    }

    private void Update()
    {
        if (isPlaying)
        {
            if (cutTrigger)
            {
                cutTrigger = false;

                _cuttedHophey = GameObject.FindGameObjectWithTag("cutted");

                hophey.transform.DOKill();
                if (hophey.transform.position.x > cutSum)
                {
                    chopParticle.Play();
                    cutSize = hophey.transform.position.x - cutSum;
                    cutSum = cutSize + cutSum;
                    Debug.LogError("Cut Size: " + cutSize);
                    Debug.LogError("Hole Size: " + GameController.gameController.holeSize);
                    if (cutSize / GameController.gameController.holeSize >= 1.0f - GameController.gameController.currentLvl.comboRange && cutSize / GameController.gameController.holeSize < 1.0f)
                    {
                        GameController.gameController.ShowPg(cutSize);
                        StartCoroutine(ResizeHole());
                        if (canPerfect)
                        {
                            GameController.gameController.comboCount++;
                            StartCoroutine(comboPointsScript.UpdateComboPoints(GameController.gameController.comboCount, 0.5f));
                        }
                        else
                        {
                            GameController.gameController.comboCount = 1;
                        }
                    }
                    else
                        if (cutSize < GameController.gameController.holeSize)
                    {
                        GameController.gameController.isIncreace = true;
                        GameController.gameController.comboCount = 1;
                    }
                    //Debug.Log("Cut Sum: " + cutSum);

                    if (cutSize <= GameController.gameController.holeSize)
                    {
                        //Debug.Log("Triggered");
                        if (cutSize > GameController.gameController.holeSize - GameController.gameController.holeSize / 10f && cutSize <= GameController.gameController.holeSize + GameController.gameController.holeSize / 10f)
                        {
                            if (canPerfect)
                            {
                                cutSize = GameController.gameController.holeSize;

                                //if (holeSize < 2.5f)
                                //    StartCoroutine(ResizeHole(cutSize + 0.3f));
                                //else
                                //    StartCoroutine(ResizeHole(cutSize));
                            }
                            else
                            {
                                cutSize = GameController.gameController.holeSize;
                            }
                        }
                        //else
                        //{
                        //    StartCoroutine(ResizeHole(cutSize));
                        //}
                    }
                    else if (cutSize > GameController.gameController.holeSize + 0.01f)
                    {
                        isLoose = true;
                        hophey.transform.DOKill();
                    }

                    diff = Mathf.Abs((_cuttedHophey.GetComponent<MeshFilter>().mesh.bounds.size.x * 20f) - GameController.gameController.holeSize);
                    //Debug.Log("Bounds " + _cuttedHophey.GetComponent<MeshFilter>().mesh.bounds.size.x);
                    //Debug.Log("Hole size " + holeSize);
                    Debug.Log("Difference " + diff);
                }
            }
            if (isLoose)
            {
                if (diff >= 0.05f)
                {
                    Debug.LogError("You Suck!");
                    isPlaying = false;
                    GameController.gameController.canMove = false;
                    gameController.restartMenu.SetActive(true);
                }
                else
                {
                    isLoose = false;
                }
            }
        }
    }

    IEnumerator StartHopRoutine()
    {
        yield return new WaitForSeconds(0f);

        if (!isCut)
            hophey.transform.DOMoveX(cutSum, 0.5f).SetEase(Ease.InOutCubic).OnComplete(IncreaseHop);
    }

    IEnumerator ResizeHole()
    {
        yield return new WaitForSeconds(0.5f);

        GameController.gameController.holeSize += GameController.gameController.currentLvl.startrHoleSize * GameController.gameController.currentLvl.deltaSize;
        Debug.Log(GameController.gameController.holeSize);
        if (GameController.gameController.holeSize < 2f)
            GameController.gameController.holeSize = 2f;
    }
}

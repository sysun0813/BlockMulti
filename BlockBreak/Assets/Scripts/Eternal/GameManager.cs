using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float groundY = -55.489f;
    public GameObject BallPreview, GameOverPanel, BallCountTextObj, BallPlusTextObj;
    public static GameManager instance;
    public Text bestScore, presentScore;

    Vector3 firstPos, secondPos, gap;
    public int score, timerCount, launchIndex;
    bool timerStart, isDie, isNewRecord, isBlockMoving;
    float timeDelay;

    void Awake()
    {
        GameManager.instance = this;
    }

    void Start()
    {
        //bestScore.text = "최고기록 : " + PlayerPrefs.GetInt("bestScore");
        presentScore.text = "현재점수 : 10";
    }





}

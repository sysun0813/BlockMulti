using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paddle : MonoBehaviour
{

    public string[] stageStr;
    public Sprite[] b;
    public GameObject p_Item;
    public SpriteRenderer p_ItemSr;
    public Text stageText;
    public Text scoreText;
    public GameObject life0;
    public GameObject life1;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;
    public Transform itemsTr;
    public Transform blocksTr;
    public BoxCollider2D[] blockCol;
    public GameObject[] ball;
    public Transform[] ballTr;
    public SpriteRenderer[] ballSr;
    public Rigidbody2D[] ballRg;
    public GameObject[] bullet;
    public SpriteRenderer paddleSr;
    public BoxCollider2D paddleCol;
    public GameObject magnet;
    public GameObject gun;

    private void Start()
    {

    }
}

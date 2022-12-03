using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paddle : MonoBehaviour
{

    [Multiline(12)]
    public string[] StageStr;
    public Sprite[] B;
    public Button startBtn;
    public GameObject P_Item;
    public SpriteRenderer P_ItemSr;
    public Text StageText;
    public Text ScoreText;
    public GameObject Life0;
    public GameObject Life1;
    public GameObject WinPanel;
    public GameObject GameOverPanel;
    public GameObject PausePanel;
    public Transform ItemsTr;
    public Transform BlocksTr;
    public BoxCollider2D[] BlockCol;
    public GameObject[] Ball;
    public Animator[] BallAni;
    public Transform[] BallTr;
    public SpriteRenderer[] BallSr;
    public Rigidbody2D[] BallRg;
    public GameObject[] Bullet;
    public SpriteRenderer PaddleSr;
    public BoxCollider2D PaddleCol;
    public GameObject Magnet;
    public GameObject Gun;

    bool isStart;
    public float paddleX;
    public float ballSpeed;
    float oldBallSpeed = 300;
    float paddleBorder = 2.262f;
    float paddleSize = 1.58f;
    int combo;
    int score;
    int stage;

#if (UNITY_ANDROID)
    void Awake() { Screen.SetResolution(1080, 1920, false); }
#else
    void Awake() { Screen.SetResolution(540, 960, false); }
#endif
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PausePanel.activeSelf) { PausePanel.SetActive(false); Time.timeScale = 1; }
            else { PausePanel.SetActive(true); Time.timeScale = 0; }
        }
    }

    // (-1 재시작, 0 다음 스테이지, 숫자 스테이지)
    public void AllReset(int _stage)
    {
        if (_stage == 0) stage++;
        else if (_stage != -1) stage = _stage;
        if (stage >= StageStr.Length) return;

        Clear();
        BlockGenerator();
        StartCoroutine("BallReset");
        startBtn.gameObject.SetActive(false);

        StageText.text = stage.ToString();
        score = 0;
        ScoreText.text = "0";
        PaddleSr.enabled = true;
        Life0.SetActive(true);
        Life1.SetActive(true);
        WinPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    public void GameStart()
    {
        stage = 1;
        startBtn.gameObject.SetActive(false);
        Clear();
        BlockGenerator();
        StartCoroutine("BallReset");


        StageText.text = stage.ToString();
        score = 0;
        ScoreText.text = "0";
        PaddleSr.enabled = true;
        Life0.SetActive(true);
        Life1.SetActive(true);
        WinPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }
    void BlockGenerator()
    {
        string currentStr = StageStr[stage].Replace("\n", "");
        currentStr = currentStr.Replace(" ", "");
        for (int i = 0; i < currentStr.Length; i++)
        {
            BlockCol[i].gameObject.SetActive(false);
            char A = currentStr[i];
            string currentName = "Block";
            int currentB = 0;

            if (A == '*') continue;
            else if (A == '8')
            {
                currentB = 8;
                currentName = "HardBlock0";
            }
            else if (A == '9')
            {
                currentB = Random.Range(0, 8);
            }
            else
            {
                currentB = int.Parse(A.ToString());
            }
            BlockCol[i].gameObject.name = currentName;
            BlockCol[i].gameObject.GetComponent<SpriteRenderer>().sprite = B[currentB];
            BlockCol[i].gameObject.SetActive(true);
        }
    }
    IEnumerator BallReset()
    {
        isStart = false;
        combo = 0;
        Ball[0].SetActive(true);
        Ball[1].SetActive(false);
        Ball[2].SetActive(false);
        BallAni[0].SetTrigger("Blink");
        BallTr[0].position = new Vector2(paddleX, -3.55f);

        StopCoroutine("InfinityLoop");
        yield return new WaitForSeconds(0.7f);
        StartCoroutine("InfinityLoop");
    }


    // 마우스 관련 무한 루프
    IEnumerator InfinityLoop()
    {
        while (true)
        {
            if (Input.GetMouseButton(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved))
            {
                paddleX = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.GetMouseButton(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position).x, -paddleBorder, paddleBorder);
                transform.position = new Vector2(paddleX, transform.position.y);
                if (!isStart) BallTr[0].position = new Vector2(paddleX, BallTr[0].position.y);
            }
            if (!isStart && (Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)))
            {
                isStart = true;
                ballSpeed = oldBallSpeed;
                BallRg[0].AddForce(new Vector2(0.1f, 0.9f).normalized * ballSpeed);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    // 볼이 충돌할 때
    public IEnumerator BallCollisionEnter2D(Transform ThisBallTr, Rigidbody2D ThisBallRg, Ball ThisBallCs, GameObject Col, Transform ColTr, SpriteRenderer ColSr, Animator ColAni)
    {
        Physics2D.IgnoreLayerCollision(2, 2);
        if (!isStart) yield break;

        switch (Col.name)
        {
            case "Paddle":
                ThisBallRg.velocity = Vector2.zero;
                ThisBallRg.AddForce((ThisBallTr.position - transform.position).normalized * ballSpeed);
                combo = 0;
                break;
            case "MagnetPaddle":
                ThisBallCs.isMagnet = true;
                ThisBallRg.velocity = Vector2.zero;
                float gapX = transform.position.x - ThisBallTr.position.x;
                while (ThisBallCs.isMagnet)
                {
                    if (Input.GetMouseButton(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved))
                        ThisBallTr.position = new Vector2(transform.position.x + gapX, ThisBallTr.position.y);

                    if (gameObject.name == "Paddle" || (Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)))
                    {
                        ThisBallRg.velocity = Vector2.zero;
                        ThisBallRg.AddForce((ThisBallTr.position - transform.position).normalized * ballSpeed);
                        ThisBallCs.isMagnet = false;
                    }
                    yield return new WaitForSeconds(0.01f);
                }
                break;

            case "DeathZone":
                ThisBallTr.gameObject.SetActive(false);
                BallCheck();
                break;

            case "HardBlock0":
                Col.name = "HardBlock1";
                ColSr.sprite = B[9];
                break;

            case "HardBlock1":
                Col.name = "HardBlock2";
                ColSr.sprite = B[10];
                break;

            case "HardBlock2":
            case "Block":
                BlockBreak(Col, ColTr, ColAni);
                break;
        }
    }

    // 아이템
    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(col.gameObject);
        switch (col.name)
        {
            case "Item_TripleBall":
                GameObject OneBall = BallCheck();
                for (int i = 0; i < 3; i++)
                {
                    if (OneBall.name == Ball[i].name) continue;
                    BallTr[i].position = OneBall.transform.position;
                    Ball[i].SetActive(true);
                    BallRg[i].velocity = Vector2.zero;
                    BallRg[i].AddForce(Random.insideUnitCircle.normalized * ballSpeed);
                }
                break;
            case "Item_Big":
                paddleSize = 2.42f;
                paddleBorder = 1.963f;
                StopCoroutine("Item_BigOrSmall");
                StartCoroutine("Item_BigOrSmall", false);
                break;
            case "Item_Small":
                paddleSize = 0.82f;
                paddleBorder = 2.521f;
                StopCoroutine("Item_BigOrSmall");
                StartCoroutine("Item_BigOrSmall", false);
                break;
            case "Item_SlowBall":
                StopCoroutine("Item_SlowBall");
                StartCoroutine("Item_SlowBall", false);
                break;
            case "Item_FireBall":
                StopCoroutine("Item_FireBall");
                StartCoroutine("Item_FireBall", false);
                break;
            case "Item_Magnet":
                StopCoroutine("Item_Magnet");
                StartCoroutine("Item_Magnet", false);
                break;
            case "Item_Gun":
                StopCoroutine("Item_Gun");
                StartCoroutine("Item_Gun", false);
                break;
        }
    }

    IEnumerator Item_BigOrSmall(bool skip)
    {
        if (!skip)
        {
            PaddleSr.size = new Vector2(paddleSize, PaddleSr.size.y);
            PaddleCol.size = new Vector2(paddleSize, PaddleCol.size.y);
            yield return new WaitForSeconds(7.5f);
        }
        paddleSize = 1.58f;
        paddleBorder = 2.262f;
        PaddleSr.size = new Vector2(paddleSize, PaddleSr.size.y);
        PaddleCol.size = new Vector2(paddleSize, PaddleCol.size.y);
    }

    IEnumerator Item_SlowBall(bool skip)
    {
        if (!skip)
        {
            for (int i = 0; i < 3; i++)
            {
                ballSpeed = 250;
                BallAddForce(BallRg[i]);
            }
            yield return new WaitForSeconds(7.5f);
        }
        for (int i = 0; i < 3; i++)
        {
            ballSpeed = oldBallSpeed;
            BallAddForce(BallRg[i]);
        }
    }

    IEnumerator Item_FireBall(bool skip)
    {
        if (!skip)
        {
            for (int i = 0; i < 3; i++)
            {
                BallSr[i].sprite = B[23];
                ParticleSystem.MainModule PS = BallTr[i].GetChild(0).GetComponent<ParticleSystem>().main;
                PS.startColor = Color.red;
            }
            for (int i = 0; i < BlockCol.Length; i++)
            {
                BlockCol[i].tag = "TriggerBlock";
                BlockCol[i].isTrigger = true;
            }
            yield return new WaitForSeconds(4);
        }
        for (int i = 0; i < 3; i++)
        {
            BallSr[i].sprite = B[22];
            ParticleSystem.MainModule PS = BallTr[i].GetChild(0).GetComponent<ParticleSystem>().main;
            PS.startColor = Color.white;
        }
        for (int i = 0; i < BlockCol.Length; i++)
        {
            BlockCol[i].tag = "Untagged";
            BlockCol[i].isTrigger = false;
        }
    }

    IEnumerator Item_Magnet(bool skip)
    {
        if (!skip)
        {
            gameObject.name = "MagnetPaddle";
            Magnet.SetActive(true);
            yield return new WaitForSeconds(5.5f);
            Magnet.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            Magnet.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            Magnet.SetActive(false);
            yield return new WaitForSeconds(0.25f);
            Magnet.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            Magnet.SetActive(false);
            yield return new WaitForSeconds(0.25f);
            Magnet.SetActive(true);
            yield return new WaitForSeconds(0.25f);
        }
        gameObject.name = "Paddle";
        Magnet.SetActive(false);
    }

    IEnumerator Item_Gun(bool skip)
    {
        if (!skip)
        {
            Gun.SetActive(true);
            for (int i = 0; i < 12; i++)
            {
                Bullet[i * 2].SetActive(true);
                Bullet[i * 2 + 1].SetActive(true);
                yield return new WaitForSeconds(0.34f);
            }
        }
        Gun.SetActive(false);
    }

    public void BlockBreak(GameObject Col, Transform ColTr, Animator ColAni)
    {
        ItemGenerator(Col.transform.position);
        score += (++combo > 3) ? 3 : combo;
        ScoreText.text = score.ToString();

        ColAni.SetTrigger("Break");
        StartCoroutine(ActiveFalse(Col));

        StopCoroutine("BlockCheck");
        StartCoroutine("BlockCheck");
    }

    //아이템 생성
    void ItemGenerator(Vector2 ColTr)
    {
        int rand = Random.Range(0, 1000);
        if (rand < 100)
        {
            string currentName = "";
            switch (rand % 7)
            {
                case 0: currentName = "Item_TripleBall"; break;
                case 1: currentName = "Item_Big"; break;
                case 2: currentName = "Item_Small"; break;
                case 3: currentName = "Item_SlowBall"; break;
                case 4: currentName = "Item_FireBall"; break;
                case 5: currentName = "Item_Magnet"; break;
                case 6: currentName = "Item_Gun"; break;
            }
            P_ItemSr.sprite = B[rand % 7 + 11];
            GameObject Item = Instantiate(P_Item, ColTr, Quaternion.identity);
            Item.name = currentName;
            Item.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 0.008f);
            Item.transform.SetParent(ItemsTr);
            Destroy(Item, 7);
        }
    }

    IEnumerator ActiveFalse(GameObject Col)
    {
        yield return new WaitForSeconds(0.2f);
        Col.SetActive(false);
    }

    GameObject BallCheck()
    {
        int ballCount = 0;
        GameObject ReturnBall = null;
        foreach (GameObject OneBall in GameObject.FindGameObjectsWithTag("Ball"))
        {
            ballCount++;
            ReturnBall = OneBall;
        }

        if (ballCount == 0)
        {
            if (Life1.activeSelf)
            {
                Life1.SetActive(false);
                StartCoroutine("BallReset");
            }
            else if (Life0.activeSelf)
            {
                Life0.SetActive(false);
                StartCoroutine("BallReset");
            }
            else
            {
                GameOverPanel.SetActive(true);
                Clear();
            }
        }

        return ReturnBall;
    }

    public void BallAddForce(Rigidbody2D ThisBallRg)
    {
        Vector2 dir = ThisBallRg.velocity.normalized;
        ThisBallRg.velocity = Vector2.zero;
        ThisBallRg.AddForce(dir * ballSpeed);
    }

    IEnumerator BlockCheck()
    {
        yield return new WaitForSeconds(0.5f);
        int blockCount = 0;
        for (int i = 0; i < BlocksTr.childCount; i++)
            if (BlocksTr.GetChild(i).gameObject.activeSelf) blockCount++;

        if (blockCount == 0)
        {
            WinPanel.SetActive(true);
            Clear();
        }
    }

    void Clear()
    {
        StopAllCoroutines();
        StartCoroutine("Item_BigOrSmall", true);
        StartCoroutine("Item_SlowBall", true);
        StartCoroutine("Item_FireBall", true);
        StartCoroutine("Item_Magnet", true);
        StartCoroutine("Item_Gun", true);

        for (int i = 0; i < 3; i++) Ball[i].SetActive(false);
        PaddleSr.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeBlockManager : MonoBehaviour {

    public GameObject ball, greenOrb, block;
    public Transform blockGroup;
    public Quaternion qi;
    public bool isBlockMove;

    void BlockGenerate()
    {
        int count;
        int randBlock = Random.Range(0, 24);
        if(GameManager.instance.score <= 10)
        {
            count = randBlock < 16 ? 1 : 2;
        }
        else if(GameManager.instance.score <= 20)
        {
            count = randBlock < 8 ? 1 : (randBlock < 16 ? 2 : 3);
        }
        else if(GameManager.instance.score <= 40)
        {
            count = randBlock < 9 ? 2 : (randBlock < 18 ? 3 : 4);
        }
        else
        {
            count = randBlock < 8 ? 2 : (randBlock < 16 ? 3 : (randBlock < 20 ? 4 : 5));
        }

        List<Vector3> spawnList = new List<Vector3>();
        for (int i = 0; i < 6; i++) spawnList.Add(new Vector3(-46.7f + i * 18.68f, 51.2f, 0));

        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, spawnList.Count);

            Transform tr = Instantiate(block, spawnList[rand], qi).transform;
            tr.SetParent(blockGroup);
            tr.GetChild(0).GetComponentInChildren<Text>().text = GameManager.instance.score.ToString();

            spawnList.RemoveAt(rand);
        }
        Instantiate(greenOrb, spawnList[Random.Range(0, spawnList.Count)], qi).transform.SetParent(blockGroup);

        isBlockMove = true;
        for(int i = 0; i < blockGroup.childCount; ++i)
        {
            StartCoroutine(BlockDown(blockGroup.GetChild(i)));
        }
        //StartCoroutine(BlockDown(blockGroup));
    }


    IEnumerator BlockDown(Transform tr)
    {
        yield return new WaitForSeconds(0.3f);

        if (isBlockMove)
        {
            
        }


    }

    void BlockColorChange(GameObject b)
    {
    }

}

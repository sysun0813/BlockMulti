using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour {

	public string[] StageStr;
	public BoxCollider2D[] BlockCol;
    public Sprite[] B;
    int stage;



    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
}

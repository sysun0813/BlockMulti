using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour {

	public GameObject single;
	public GameObject eternal;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SingleButtonOn()
    {
		//this.single.SetActive(false);
		SceneManager.LoadScene("Original");
    }

	public void EternalMode()
    {
		//this.eternal.SetActive(false);
		SceneManager.LoadScene("Swipe");
    }

}

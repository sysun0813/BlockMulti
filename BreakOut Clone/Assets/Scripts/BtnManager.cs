using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour {

	public GameObject single;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SingleButtonOn()
    {
		SceneManager.LoadScene("SampleScene");
    }

	public void ExitButtonOn()
    {
//#if UNITY_EDITOR
//			UnityEditor.EditorApplication.isPlaying = false;
//#elif UNITY_IOS
		
//#else
//		Application.Quit(0);
//#endif
	}
}

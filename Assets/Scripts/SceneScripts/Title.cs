﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ClickGameStart() {
		SceneManager.LoadScene("GameScene");
	}

	public void ClickExit() {
		Application.Quit();
	}

	public void ClickDelete() {
		PlayerPrefs.DeleteAll();
	}
}

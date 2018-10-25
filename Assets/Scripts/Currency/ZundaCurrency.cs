using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZundaCurrency : AbstractCurrency {


	// Use this for initialization
	void Start () {
		name = "ZND";
		myColor = Color.green;
		Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateTexts();
	}

	public void ClickOneBuyButton() {
		Trade(1, true);
	}

	public void ClickFiveBuyButton() {
		Trade(10, true);
	}

	public void ClickOneSellButton() {
		Trade(1, false);
	}

	public void ClickFiveSellButton() {
		Trade(10, false);
	}

	public void ChangeChart() {
		Game.Instance.ChangeMainCurrency(this);
	}
}

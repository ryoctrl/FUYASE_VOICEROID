using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AoiCurrency : AbstractCurrency {

	// Use this for initialization
	void Start () {
		name = "AOI";
		assetsText = GameObject.Find("AoiAssetsText").GetComponent<Text>();
		Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		assetsText.text = assets.ToString("F2");
	}

	public void ClickOneBuyButton() {
		Trade(1, true);
	}

	public void ClickFiveBuyButton() {
		Trade(5, true);
	}

	public void ClickOneSellButton() {
		Trade(1, false);
	}

	public void ClickFiveSellButton() {
		Trade(5, false);
	}

	public void ChangeChart() {
		Game.Instance.ChangeMainCurrency(this);
	}
}

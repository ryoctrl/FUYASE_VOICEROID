using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPanelScript : MonoBehaviour {

	private int needPrice = -1;
	private AbstractCurrency parentCurrency;
	private Text errorText;
	private Text priceText;
	private Button levelUpButton;

	void Start() {
		Text[] texts = GetComponentsInChildren<Text>();
		foreach(Text t in texts) {
			if(t.name == "LevelUpPriceText") priceText = t;
			else if(t.name == "ErrorMessage") errorText = t;
		}

		Button[] buttons = GetComponentsInChildren<Button>();
		foreach(Button b in buttons) {
			if(b.name == "LevelUpButton") levelUpButton = b;
		}
		InitializeIfNeeded();
	}

	public void setPriceAndCurrency(int price, AbstractCurrency c) {
		needPrice = price;
		parentCurrency = c;
		InitializeIfNeeded();
	}
		

	public void InitializeIfNeeded() {
		if(needPrice <= 0 || priceText == null) return;
		priceText.text = needPrice.ToString();
		if(needPrice <= Game.Instance.GetFiatAssets()) {
			errorText.gameObject.SetActive(false);
		} else {
			levelUpButton.enabled = false;
		}
	}
	


	public void ClickCancel() {
		Destroy(transform.gameObject);
	}

	public void ClickLevelUp() {
		Destroy(transform.gameObject);
		parentCurrency.LevelUp();
	}
}

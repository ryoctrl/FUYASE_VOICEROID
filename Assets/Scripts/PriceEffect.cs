using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceEffect : MonoBehaviour {

	private Text priceText;
	private RectTransform rectTransform;
	private bool start = false;
	private bool isPlus = false;

	// Use this for initialization
	void Start () {
		priceText = GetComponent<Text>();
		rectTransform = GetComponent<RectTransform>();
		ChangePrice();
	}
	
	// Update is called once per frame
	void Update () {
		if(!start) return;
		Effecting();
	}

	private void Effecting() {
		Vector3 pos = rectTransform.position;
		if(isPlus) pos.y += 0.25f;
		else pos.y -= 0.25f;
		rectTransform.position = pos;

		Color c = priceText.color;
		c.a -= 0.02f;
		priceText.color = c;

		CheckEffected();
	}

	private void CheckEffected() {
		float touka = priceText.color.a;
		if(touka > 0) return;
		Destroy(gameObject);
	}

	private float price = float.MaxValue;

	private void ChangePrice() {
		if(priceText == null || price == float.MaxValue) return;

		string priceStr = price.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("ja-JP"));
		priceStr = priceStr.Replace("\\", "￥");
		priceText.text = priceStr;
		Color c;
		if(price >= 0) {
			c = new Color32(0, 0, 255, 255);
			isPlus = true;
		} else {
			c = new Color32(255, 0, 0, 255);
			isPlus = false;
		}
		priceText.color = c;
		start = true;
	}

	public void SetPrice(float price) {
		this.price = price;
		ChangePrice();	
	}
}

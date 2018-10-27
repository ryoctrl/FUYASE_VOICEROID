using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YukariPrice : PriceSystem {
	public float initialPrice = 175.0f;
	private float volatilityIndex;
	private float modifyNum = 0;

	public List<Sprite> images;

	void Start () {
		base.Start();
		price = initialPrice;
	}

	public override List<Sprite> GetImages() {
		return images;
	}

	protected override void CalcPrice() {
		volatilityIndex = Random.Range(0, 100) + modifyNum;
		modifyNum = modifyNum >= 40 ? 40 : modifyNum;
		modifyNum = modifyNum <= -40 ? -40 : modifyNum;

		if(volatilityIndex > 50) {
			if(modifyNum < -10) modifyNum *= -0.5f;
			price += price * Random.Range(0.0f, 0.1f);
			modifyNum++;
		} else {
			if(modifyNum > 10) modifyNum *= -0.5f;
			price += price * Random.Range(-0.08f, 0.0f);
			modifyNum--;
		}
	}

	protected override void UpdateImage() {
		if(modifyNum > 30) {
			characterImage.sprite = images[0];
		} else if(modifyNum > 10) {
			characterImage.sprite = images[1];
		} else if(modifyNum > -10) {
			characterImage.sprite = images[2];
		} else if(modifyNum > -30) {
			PlayHimei();
			characterImage.sprite = images[3];
		} else {
			characterImage.sprite = images[4];
		}
	}
}
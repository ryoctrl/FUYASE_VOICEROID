using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalanceSystem : SingletonMonoBehaviour<BalanceSystem> {
	private float initialPrice = 200.0f;
	private float price;
	private float volatilityIndex;
	private float modifyNum = 0;

	public Sprite baddest;
	public Sprite badder;
	public Sprite normal;
	public Sprite good;
	public Sprite goodest;

	public GameObject mask;



	// Use this for initialization
	void Start () {
		price = initialPrice;
	}

	public void newTick() {
		calcPrice();
		UpdateAkn();
	}

	private GameObject currentAkn;
	private void UpdateAkn() {
		
		
	}

	private void calcPrice() {
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
}

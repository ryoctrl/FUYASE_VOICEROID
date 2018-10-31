using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TradePanel : MonoBehaviour {

	private InputField amountField;
	private bool isDecrement = false;
	private bool onClicking = false;
	private float timer = 0;
	private float holdTimer = 0;
	private float initialThreashold = 0.5f;
	private float threshold = 0f;
	private AbstractCurrency currency;

	// Use this for initialization
	void Start () {
		amountField = GetComponentInChildren<InputField>();
		amountField.onValidateInput += (string text, int charIndex, char addedChar) => {
			char ret = addedChar;
			if('0' <= ret && ret <= '9') {
				return ret;
			}
			return '\0';
		};
		threshold = initialThreashold;
	}

	public void SetCurrency(AbstractCurrency currency) {
		this.currency = currency;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(onClicking) {
			holdTimer += Time.deltaTime;
			if(threshold > 0.02f) threshold -= holdTimer / 50;
		}
		if(onClicking && timer > threshold) {
			int amount = int.Parse(amountField.text);
			if(isDecrement) {
				if(amount == 0) return;
				amount--;
			} else {
				amount++;
			}
			amountField.text = amount.ToString();
			timer = 0;
		}
	}

	public void OnPointerDown(BaseEventData data){
		if(data.selectedObject.name.StartsWith("Decrement")) {
			isDecrement = true;
		} else {
			isDecrement = false;
		}
		onClicking = true;
	}

	public void OnPointerUp(BaseEventData data) {
		onClicking = false;
		threshold = initialThreashold;
		holdTimer = 0;
	}

	public void OnClickBuy() {
		ExecTrade(true);
	}

	private void ExecTrade(bool isBuy) {
		int amount = int.Parse(amountField.text);
		currency.Trade(amount, isBuy);
	}

	public void OnClickSell() {
		ExecTrade(false);

	}
}

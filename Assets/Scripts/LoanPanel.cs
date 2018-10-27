using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoanPanel : MonoBehaviour {

	private bool sized = false;
	private bool smalled = false;
	private bool destroying = false;

	private Text errorMessage;
	private Button okButton;


	

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(0, 0, 0);
		sized = false;	

		foreach(Text t in GetComponentsInChildren<Text>()) {
			if(t.name == "ErrorMessage") errorMessage = t;
		}

		foreach(Button b in GetComponentsInChildren<Button>()) {
			if(b.name == "OkButton") okButton = b;
		}

		if(Game.Instance.GetFiatAssets() < 300000) {
			errorMessage.text = "資金が足りません!";
			okButton.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(smalled) Destroy(transform.gameObject);
		if(!sized && !destroying) {
			ToBigScale();
		} else if(destroying) {
			ToSmallScale();
		}
	}

	private void ToBigScale() {
		Vector3 scale = transform.localScale;
		if(scale.x >= 1 || scale.y >= 1) {
			sized = true;
			return;
		}
		scale.x += 0.05f;
		scale.y += 0.05f;
		transform.localScale = scale;
	}

	private void ToSmallScale() {
		Vector3 scale = transform.localScale;
		if(scale.x < 0 || scale.y < 0) {
			smalled = true;
			return;
		}
		scale.x -= 0.05f;
		scale.y -= 0.05f;
		transform.localScale = scale;
	}

	public void ClickCancel() {
		destroying = true;
	}

	public void ClickOK() {
		Game.Instance.ChangeAssets(-300000);
		Game.Instance.RepaymentLoan();
		destroying = true;
	}

	
}

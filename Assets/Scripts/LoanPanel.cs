using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoanPanel : MonoBehaviour {

	private bool sized = false;
	private bool smalled = false;
	private bool destroying = false;

	

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(0, 0, 0);
		sized = false;	
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

	}

	
}

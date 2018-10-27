using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MessagePanel : BasePanel {

	public delegate void OkDelegate();

	private Text messageText;
	private string message;

	// Use this for initialization
	void Start () {
		base.Start();
		messageText = GetComponentInChildren<Text>();
		if(message != null) messageText.text = message;

		foreach(Button b in GetComponentsInChildren<Button>()) {
			b.onClick.AddListener(() => {
				destroying = true;
			});
		}
	}

	void Update() {
		base.Update();
	}

	public void SetMessage(string message) {
		if(messageText != null) messageText.text = message;
		this.message = message;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InputPanel : BasePanel {

	public delegate void OkDelegate(string name);

	private Text messageText;
	private InputField inputField;
	private string message;

	private Button okButton;

	private OkDelegate okClicked;

	// Use this for initialization
	void Start () {
		base.Start();
		messageText = GetComponentInChildren<Text>();
		if(message != null) messageText.text = message;

		inputField = GetComponentInChildren<InputField>();

		foreach(Button b in GetComponentsInChildren<Button>()) {
			if(b.name == "OK") {
				okButton = b;
				if(okClicked != null) okButton.onClick.AddListener(() => {
					okClicked(inputField.text);
				});
			}
			
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

	public void SetOkClicked(OkDelegate okClicked) {
		this.okClicked = okClicked;
		if(okButton != null) {
			okButton.onClick.AddListener(() => {
			okClicked(inputField.text);
			});
		}
}}

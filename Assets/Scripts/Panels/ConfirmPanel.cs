using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConfirmPanel : BasePanel {

	public delegate void OkDelegate();

	private Text messageText;
	private Button ok;
	private Button cancel;
	private UnityAction okClicked;
	private UnityAction cancelClicked;
	private string message;

	// Use this for initialization
	void Start () {
		base.Start();
		messageText = GetComponentInChildren<Text>();
		if(message != null) messageText.text = message;
		foreach(Button b in GetComponentsInChildren<Button>()) {
			if(b.name == "OK") {
				ok = b;
				ok.onClick.AddListener(() => {
					destroying = true;
				});
				if(okClicked != null) ok.onClick.AddListener(okClicked);
			}else if(b.name == "Cancel") {
				cancel = b;
				b.onClick.AddListener(() => {
					destroying = true;
				});
				if(cancelClicked != null) cancel.onClick.AddListener(cancelClicked);
			}
		}
	}

	void Update() {
		base.Update();
	}

	public void SetOkDelegate(UnityAction okClicked) {
		if(ok != null) ok.onClick.AddListener(okClicked);
		this.okClicked = okClicked;
	}

	public void SetMessage(string message) {
		if(messageText != null) messageText.text = message;
		this.message = message;
	}

	public void SetCancelDelegate(UnityAction cancelClicked) {
		if(cancel != null) cancel.onClick.AddListener(cancelClicked);
		this.cancelClicked = cancelClicked;
	}
}

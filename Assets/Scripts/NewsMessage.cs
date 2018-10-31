using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsMessage : MonoBehaviour {

	private Text messageText;
	private Vector3 initialTextPosition;
	private bool playing = false;
	private RectTransform myRect;
	public string debugMessage;

	void Start () {
		myRect = GetComponent<RectTransform>();
		messageText = GetComponentInChildren<Text>();
		initialTextPosition = messageText.gameObject.transform.localPosition;
		messageText.gameObject.SetActive(false);
	}
	
	private Vector3 tmpPos;
	// Update is called once per frame
	void Update () {
		if(!playing) {
			SpawnNewMessage(debugMessage);
			return;
		}
		tmpPos = messageText.gameObject.transform.localPosition;
		tmpPos.x -= 0.75f;
		messageText.gameObject.transform.localPosition = tmpPos;
		if(tmpPos.x < -initialTextPosition.x) FinishPlaying();
	}

	public void SpawnNewMessage(string message) {
		if(playing) return;

		messageText.text = message;
		PlayMessage();
	}

	private void PlayMessage() {
		messageText.gameObject.SetActive(true);
		playing = true;
	}

	private void FinishPlaying() {
		playing = false;
		messageText.text = "";
		messageText.gameObject.transform.localPosition = initialTextPosition;
		messageText.gameObject.SetActive(false);
	}
}

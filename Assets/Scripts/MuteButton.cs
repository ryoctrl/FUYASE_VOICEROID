using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour {

	public Sprite toMuteImage;
	public Sprite toSpeakImage;

	private bool muting = false;

	private Image image;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		image.sprite = toMuteImage;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeSpeaker() {
		if(!muting) {
			muting = true;
			image.sprite = toSpeakImage;
		} else {
			muting = false;
			image.sprite = toMuteImage;
		}

		Game.Instance.SetMute(muting);
	}
}

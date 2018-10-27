using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class GameMission : MonoBehaviour {

	private Text missionText;
	private Image clearedImage;
	private AudioSource clearedAudio;
	public string missionName;

	// Use this for initialization
	void Start () {
		missionText = GetComponent<Text>();
		clearedImage = GetComponentInChildren<Image>();
		clearedAudio = GetComponent<AudioSource>();
		clearedImage.gameObject.SetActive(false);
		initialize();
	}

	public string GetName() {
		return missionName;
	}

	private void initialize() {
		Game.Instance.AddMission(this);
	}

	public void Cleared() {
		missionText.color = new Color32(43, 43, 43, 255);
		clearedImage.gameObject.SetActive(true);
		clearedAudio.Play();
	}
	
	
}

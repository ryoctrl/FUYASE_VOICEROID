using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	private VideoPlayer videoPlayer;
	public VideoClip[] videoClips;

	private bool setted = false;
	private bool proced = false;
	private bool isAssetsGT1000 = false;
	private bool isLoanPaymented = false;
	private bool isAssetsGT30 = false;

	public GameObject confirmPanel;

	// Use this for initialization
	void Start () {
		videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
		videoPlayer.isLooping = true;
		videoPlayer.loopPointReached += VideoEnded;
		setCondition(Game.isLoanPaymented, Game.isAssetsGT1000, Game.isAssetsGt30);
	}
	
	// Update is called once per frame
	void Update () {
		if(!proced && setted) {
			proced = true;
			if(!isAssetsGT1000 && !isLoanPaymented && !isAssetsGT30) videoPlayer.clip = videoClips[3];
			else if(isLoanPaymented && !isAssetsGT1000) videoPlayer.clip = videoClips[2];
			else if(!isLoanPaymented && isAssetsGT30) videoPlayer.clip = videoClips[1];
			else videoPlayer.clip = videoClips[0];

			videoPlayer.Play();
		}
	}

	public void VideoEnded(VideoPlayer p) {
		videoPlayer.isLooping = false;
		GameObject confirmP = Instantiate(confirmPanel);
		confirmP.transform.SetParent(GameObject.Find("Canvas").transform, false);
		ConfirmPanel confirm = confirmP.GetComponent<ConfirmPanel>();
		confirm.SetMessage("これで物語は完結となります。このままトレードを続けますか？");
		confirm.GetComponent<ConfirmPanel>().SetOkDelegate(() => {
			SceneManager.LoadScene("GameScene");
		});
		confirm.GetComponent<ConfirmPanel>().SetCancelDelegate(() => {
			SceneManager.LoadScene("TitleScene");
		});
	}

	public void setCondition(bool isLoanPaymented, bool isAssetsGT1000, bool isAssetsGT30) {
		this.isAssetsGT1000 = isAssetsGT1000;
		this.isAssetsGT30 = isAssetsGT30;
		this.isLoanPaymented = isLoanPaymented;
		this.setted = true;
	}
}

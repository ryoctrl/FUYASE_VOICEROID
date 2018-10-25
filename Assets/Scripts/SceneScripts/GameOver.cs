using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	private VideoPlayer videoPlayer;
	private bool played = false;

	// Use this for initialization
	void Start () {
		videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
		videoPlayer.isLooping = true;
		videoPlayer.loopPointReached += VideoEnded;
		if(videoPlayer.clip != null) {
			videoPlayer.Play();

		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void VideoEnded(VideoPlayer p) {
		SceneManager.LoadScene("TitleScene");
	}
}

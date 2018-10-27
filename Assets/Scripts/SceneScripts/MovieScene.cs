using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class MovieScene : MonoBehaviour {

	public string nextScene;
	private VideoPlayer videoPlayer;

	// Use this for initialization
	void Start () {
		videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
		videoPlayer.aspectRatio = VideoAspectRatio.Stretch;
		videoPlayer.isLooping = true;
		videoPlayer.loopPointReached += VideoEnded;
		if(videoPlayer.clip != null) {
			videoPlayer.Play();

		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Space)) {
			VideoEnded(videoPlayer);
		}
	}

	public void VideoEnded(VideoPlayer p) {
		SceneManager.LoadScene(nextScene);
	}
}

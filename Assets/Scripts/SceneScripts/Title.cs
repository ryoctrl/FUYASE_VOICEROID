using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

	public GameObject confirmPanel;
	public GameObject messagePanel;
	public GameObject inputPanel;
	public Text highScoreText;
	private GameObject canvas;


	// Use this for initialization
	void Start () {
		//PlayerPrefs.DeleteAll();

		canvas = GameObject.Find("Canvas");
		highScoreText = GameObject.Find("HighScoreText").GetComponent<Text>();
		highScoreInit();
		if(PlayerPrefs.HasKey("uid")) return;
		OpenRegisterUserName();
	}

	private void OpenRegisterUserName() {
		GameObject userRegistPanel = Instantiate(inputPanel);
		userRegistPanel.transform.SetParent(canvas.transform, false);
		userRegistPanel.GetComponent<InputPanel>().SetOkClicked((name) => {
			if(name == null || name == "") name = "NoName";
			PlayerPrefs.SetString("uid", name);
		});
	}

	private void highScoreInit() {
		if(!PlayerPrefs.HasKey("score")) {
			highScoreText.text = "前回のハイスコア(総資産):未クリア";
			return;
		} else {
			highScoreText.text = "前回のハイスコア(総資産):" + PlayerPrefs.GetFloat("score") + "円";
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ClickGameStart() {
		SceneManager.LoadScene("GameScene");
	}

	public void ClickGameStartWithTutorial() {
		SceneManager.LoadScene("Donyu1");
	}

	public void ClickExit() {
		Application.Quit();
	}

	public void ClickDelete() {
		GameObject confirm = Instantiate(confirmPanel);
		confirm.transform.SetParent(canvas.transform, false);
		confirm.GetComponent<ConfirmPanel>().SetMessage("セーブデータを削除してよろしいですか？");
		confirm.GetComponent<ConfirmPanel>().SetOkDelegate(() => {
			PlayerPrefs.DeleteAll();
			GameObject complete = Instantiate(messagePanel);
			complete.transform.SetParent(canvas.transform, false);
			complete.GetComponent<MessagePanel>().SetMessage("セーブデータを削除しました");
			OpenRegisterUserName();
		});
	}

	public void OpenRanking() {
		Application.OpenURL("https://fuyasevoiceroid.mosin.jp/score");
	}
}

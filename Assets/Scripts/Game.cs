using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : SingletonMonoBehaviour<Game> {
	public const float GAME_SPEED = 24f;

	private Text calendarText;
	private Text timeLimitText;
	private Text totalAssetsText;
	public Text assetsText;
	private Slider speedSlider;
	public GameObject panelPrefab;

	private int time = 0;
	private int loan = 100000;
	private float totalAssets;
	private float assets;
	private DateTime now;
	private DateTime limitTime;
	
	private GameObject pauseButton;
	private GameObject speedButton;

	private bool muting = false;
	private bool pausing = false;

	private int speed = 1;
	private float timer = 0;
	
	private List<PriceSystem> priceSystems = new List<PriceSystem>();
	private List<AbstractCurrency> currencies = new List<AbstractCurrency>();
	private AbstractCurrency mainCurrency;
	private Image characterImage;

	public void SetMute(bool muting) {
		this.muting = muting;
	}

	public bool GetMute() {
		return muting;
	}

	public float GetFiatAssets() {
		return assets;
	}

	void Start () {
		if(PlayerPrefs.HasKey("save")) {
			LoadGame();
		} else {
			assets = 100000;
			totalAssets = -assets;
			now = DateTime.Now;
			limitTime = now.AddYears(1);
		}
		
		characterImage = GameObject.Find("MainCharacterImage").GetComponent<Image>();

		calendarText = GameObject.Find("CalendarText").GetComponent<Text>();
		timeLimitText = GameObject.Find("TimelimitText").GetComponent<Text>();
		totalAssetsText = GameObject.Find("TotalAssetsText").GetComponent<Text>();
		speedSlider = GameObject.Find("SpeedSlider").GetComponent<Slider>();
		pauseButton = GameObject.Find("PauseButton");
	}

	void Update () {
		CheckLimit();
		UpdateStatus();
		if(pausing) return;

		timer += Time.deltaTime;
		if(timer > GAME_SPEED / speed) {
			foreach(AbstractCurrency currency in currencies) {
				currency.NewTick();
				currency.Mining();
			}
			//characterImage.sprite = mainCurrency.GetCurrentImage();
			timer = 0;
		}
		now = now.AddMinutes(1440 / GAME_SPEED * speed * Time.deltaTime);
	}

	public void AddCurrency(AbstractCurrency currency) {
		currencies.Add(currency);
		if(PlayerPrefs.HasKey("save")) LoadCurrency(currency);
		if((PlayerPrefs.HasKey("MainCurrency") && PlayerPrefs.GetString("MainCurrency") == currency.GetName()) || mainCurrency == null) {
			ChangeMainCurrency(currency);
		}
	}

	public void ChangeMainCurrency(AbstractCurrency currency) {
		mainCurrency = currency;
		Chart.Instance.SetPrices(mainCurrency.GetPrices(), mainCurrency.GetName(), mainCurrency.GetColor());
	}

	public void Pause() {
		pausing = !pausing;
		pauseButton.GetComponentInChildren<Text>().text = pausing ? "▶" : "||";
	}
	
	public void SpeedChange() {
		speed = (int)speedSlider.value;
	}

	public void ChangeAssets(float change) {
		assets += change;
	}

	private void UpdateStatus(){
		UpdateCalendar();
		UpdateTimeLimit();
		UpdateTotalAssets();
		UpdateAssets();
	}

	private void UpdateCalendar() {
		string calText = "";
		calText += now.Year.ToString().Substring(2, 2);
		calText += "年";
		calText += now.Month.ToString("D2");
		calText += "月";
		calText += now.Day.ToString("D2");
		calText += "日 ";
		calText += now.Hour.ToString("D2");
		calText += ":";
		calText += now.Minute.ToString("D2");
		calendarText.text = calText;
	}

	private void UpdateTimeLimit() {
		TimeSpan ts = limitTime - now;
		string calText = "残り時間";
		calText += ts.Days / 30;
		calText += "か月と";
		calText += ts.Days % 30;
		calText += "日";
		timeLimitText.text = calText;
	}

	private void UpdateTotalAssets() {
		float totalAsettsAsFiat = assets - loan;
		foreach(AbstractCurrency ac in currencies) totalAsettsAsFiat += ac.GetPrice() * ac.GetAssets(); 
		totalAssetsText.text = totalAsettsAsFiat.ToString("#,0") + "yen";
	}

	private void UpdateAssets() {
		assetsText.text = assets.ToString("#,0") + "yen";
	}

	private void CheckLimit() {
		if(now - limitTime > TimeSpan.Zero) {
			SceneManager.LoadScene("GameOverScene");
		}
	}

	public void SaveGame() {

		PlayerPrefs.SetInt("save", 1);
		PlayerPrefs.SetString("now", now.ToString("yyyy/MM/dd HH:mm:ss"));
		PlayerPrefs.SetString("limitTime", limitTime.ToString("yyyy/MM/dd HH:mm:ss"));
		PlayerPrefs.SetFloat("fiat", assets);
		PlayerPrefs.SetString("MainCurrency", mainCurrency.GetName());
		foreach(AbstractCurrency currency in currencies) {
			PlayerPrefs.SetFloat(currency.GetName() + "Assets", currency.GetAssets());
			PlayerPrefs.SetInt(currency.GetName() + "Lv", currency.GetLv());
			PlayerPrefs.SetString(currency.GetName() + "Prices", currency.ToString());
		}
	}

	public void LoadGame() {
		string nowStr = PlayerPrefs.GetString("now");
		string limitStr = PlayerPrefs.GetString("limitTime");
		float assets = PlayerPrefs.GetFloat("fiat");
		if(nowStr == null || limitStr == null) return;
		this.now = DateTime.Parse(nowStr);
		this.limitTime = DateTime.Parse(limitStr);
		this.assets = assets;
	}

	private void LoadCurrency(AbstractCurrency currency) {
		currency.SetAssets(PlayerPrefs.GetFloat(currency.GetName() + "Assets"));
		currency.SetLv(PlayerPrefs.GetInt(currency.GetName() + "Lv"));
		currency.SetPrices(PlayerPrefs.GetString(currency.GetName() + "Prices"));
	}

	public void ClickSaveAndExit() {
		SaveGame();
		SceneManager.LoadScene("TitleScene");
	}

	public void ClickLoan() {
		GameObject loanPanel = Instantiate(panelPrefab);
		loanPanel.transform.SetParent(GameObject.Find("MainBack").transform, false);
		// GameObject levelupPanel = Instantiate(panelPrefab);
		// levelupPanel.transform.SetParent(transform, false);
		// levelupPanel.GetComponent<LevelUpPanelScript>().setPriceAndCurrency(100 * level, this);
	}
}
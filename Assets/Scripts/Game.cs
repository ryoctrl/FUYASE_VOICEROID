using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Game : SingletonMonoBehaviour<Game> {
	public const float GAME_SPEED = 2f;

	private GameObject mainPanel;
	private RectTransform mainRect;
	private GameObject statusPanel;
	private RectTransform statusRect;

	private Text calendarText;
	private Text timeLimitText;
	private Text totalAssetsText;
	public Text assetsText;

	private int time = 0;
	private int goal = 100000;
	private float totalAssets;
	private float assets;
	private DateTime now = DateTime.Now;
	private DateTime limitTime;
	
	private GameObject pauseButton;
	private GameObject speedButton;

	private bool muting = false;

	public void SetMute(bool muting) {
		this.muting = muting;
	}

	public bool GetMute() {
		return muting;
	}

	private bool pausing = false;

	private int speed = 1;
	
	private List<PriceSystem> priceSystems = new List<PriceSystem>();
	private List<AbstractCurrency> currencies = new List<AbstractCurrency>();
	private AbstractCurrency mainCurrency;
	private Image characterImage;

	public void AddCurrency(AbstractCurrency currency) {
		currencies.Add(currency);
		if(mainCurrency == null) {
			ChangeMainCurrency(currency);
		}
	}

	public void ChangeMainCurrency(AbstractCurrency currency) {
		mainCurrency = currency;
		Chart.Instance.SetPrices(mainCurrency.GetPrices(), mainCurrency.GetName(), mainCurrency.GetColor());

	}

	void Start () {
		assets = 100000;
		totalAssets = assets;

		limitTime = now.AddYears(1);
		mainPanel = GameObject.Find("MainBack");
		mainRect = mainPanel.GetComponent<RectTransform>();
		characterImage = GameObject.Find("MainCharacterImage").GetComponent<Image>();

		statusPanel = GameObject.Find("TopBack");
		statusRect = statusPanel.GetComponent<RectTransform>();

		calendarText = GameObject.Find("CalendarText").GetComponent<Text>();
		timeLimitText = GameObject.Find("TimelimitText").GetComponent<Text>();
		totalAssetsText = GameObject.Find("TotalAssetsText").GetComponent<Text>();
		pauseButton = GameObject.Find("PauseButton");
		speedButton = GameObject.Find("SpeedButton");
	}

	public void Pause() {
		if(pausing) {
			pauseButton.GetComponentInChildren<Text>().text = "||";
			pausing = false;
		} else {
			pauseButton.GetComponentInChildren<Text>().text = "▶";
			pausing = true;
		}
	}
	
	public void SpeedChange() {
		if(speed == 1) {
			speed++;
		} else if(speed == 2) {
			speed++;
		} else if(speed == 3) {
			speed = 1;	
		}

		string buttonText = "▶";
		for(int i = 1; i < speed; i++) buttonText += "▶";

		speedButton.GetComponentInChildren<Text>().text = buttonText;
	}

	public float GetFiatAssets() {
		return assets;
	}

	public void ChangeAssets(float change) {
		assets += change;
	}

	
	private float timer = 0;
	// Update is called once per frame
	void Update () {
		if(pausing) return;
		timer += Time.deltaTime;
		if(timer > GAME_SPEED / speed) {
			foreach(AbstractCurrency currency in currencies) {
				currency.NewTick();
				currency.Mining();
			}
			//Chart.Instance.AddPrice(mainCurrency.GetPrice());
			//characterImage.sprite = mainCurrency.GetCurrentImage();
			timer = 0;
		}
		now = now.AddMinutes(1440 / GAME_SPEED * speed * Time.deltaTime);
		UpdateStatus();
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
		calText += " ";
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
		float totalAsettsAsFiat = assets;
		foreach(AbstractCurrency ac in currencies) totalAsettsAsFiat += ac.GetPrice() * ac.GetAssets(); 
		totalAssetsText.text = totalAsettsAsFiat.ToString("#,0") + "yen";

	}

	private void UpdateAssets() {
		assetsText.text = assets.ToString("#,0") + "yen";
	}
}

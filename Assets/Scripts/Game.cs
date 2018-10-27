using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Game : SingletonMonoBehaviour<Game> {
	public static bool isLoanPaymented;
	public static bool isAssetsGT1000;
	public static bool isAssetsGt30;
	public float GAME_SPEED = 24f;
	//public const float GAME_SPEED = 1f;

	private Text calendarText;
	private Text timeLimitText;
	private Text totalAssetsText;
	public Text assetsText;
	private Slider speedSlider;
	public GameObject panelPrefab;
	public GameObject priceEffect;

	private int loan = 100000;
	private float assets;
	private DateTime now;
	private DateTime limitTime;
	
	private GameObject pauseButton;
	private GameObject speedButton;

	private bool pausing = false;
	private bool cleared = false;

	private int speed = 1;
	private float timer = 0;
	
	private List<AbstractCurrency> currencies = new List<AbstractCurrency>();
	private AbstractCurrency mainCurrency;
	private Image characterImage;
	public Sprite nikkori;
	private AudioSource[] characterVoices;
	private AudioSource bgm;

	///セッティング関連
	///時間がないためとりあえずの実装で。
	private Slider bgmSlider;
	private Slider characterSlider;
	//private Slider movieSlider;

	private GameObject settingPanel;

	public float GetFiatAssets() {
		return assets;
	}

	void Start () {	
		characterImage = GameObject.Find("MainCharacterImage").GetComponent<Image>();

		calendarText = GameObject.Find("CalendarText").GetComponent<Text>();
		timeLimitText = GameObject.Find("TimelimitText").GetComponent<Text>();
		totalAssetsText = GameObject.Find("TotalAssetsText").GetComponent<Text>();
		speedSlider = GameObject.Find("SpeedSlider").GetComponent<Slider>();
		pauseButton = GameObject.Find("PauseButton");

		bgmSlider = GameObject.Find("BgmSlider").GetComponent<Slider>();
		characterSlider = GameObject.Find("CharacterSlider").GetComponent<Slider>();
		//movieSlider = GameObject.Find("MovieSlider").GetComponent<Slider>();
		characterVoices = GameObject.Find("Canvas").GetComponentsInChildren<AudioSource>();
		bgm = GameObject.Find("BGM").GetComponent<AudioSource>();
		settingPanel = GameObject.Find("SettingsPanel");
		settingPanel.SetActive(false);

		if(PlayerPrefs.HasKey("save")) {
			LoadGame();
		} else {
			assets = 100000;
			now = DateTime.Now;
			limitTime = now.AddYears(1);
		}	
		LoadEnvironment();
	}

	private List<GameMission> missions = new List<GameMission>();
	public void AddMission(GameMission m) {
		missions.Add(m);
		if(isAssetsGTsenman) assetsGTsenman();
	}

	void Update () {
		KeyListen();
		CheckMission();
		CheckLimit();
		UpdateStatus();
		if(pausing) return;

		timer += Time.deltaTime;
		if(timer > GAME_SPEED / speed) {
			foreach(AbstractCurrency currency in currencies) {
				currency.NewTick();
				currency.Mining();
			}
			timer = 0;
		}
		now = now.AddMinutes(1440 / GAME_SPEED * speed * Time.deltaTime);
	}

	private bool pausingSetting = false;
	private void KeyListen() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			MenuProc();
		}

		// if(Input.GetKeyDown(KeyCode.F12)) {
		// 	if(GAME_SPEED == 1f) {
		// 		GAME_SPEED = 24f;
		// 	} else {
		// 		GAME_SPEED = 1f;
		// 	}
		// }
	}

	private void MenuProc() {
		if(settingPanel.activeSelf) {
			settingPanel.SetActive(false);
			if(pausingSetting) {
				pausingSetting = false;
				return;
			}
		} else {
			settingPanel.SetActive(true);
			if(pausing) {
				pausingSetting = true;
				return;
			}
		}
		Pause();
	}

	private float ISSENMAN = 10000000;

	private void CheckMission() {
		if(isAssetsGTsenman) return;
			
		if(loanPaymented && calcTotalAssets() >= ISSENMAN) {
			assetsGTsenman();
		}
	}

	private void assetsGTsenman() {
		isAssetsGTsenman = true;
		foreach(GameMission m in missions) {
			if(m.GetName() == "1000") m.Cleared();
		}
	}

	public void RepaymentLoan() {
		Destroy(GameObject.Find("LoanButton").gameObject);
		loanPaymented = true;
		characterImage.sprite = nikkori;
		foreach(GameMission m in missions) {
			if(m.GetName() == "repayment") m.Cleared();
		}
	}

	private bool loanPaymented = false;
	private bool isAssetsGTsenman = false;

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
		GameObject effect = Instantiate(priceEffect);
		effect.transform.SetParent(assetsText.gameObject.transform, false);
		effect.GetComponent<PriceEffect>().SetPrice(change);
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
		totalAssetsText.text = calcTotalAssets().ToString("#,0") + "yen";
	}

	private float calcTotalAssets() {
		float totalAsettsAsFiat = assets - loan;
		foreach(AbstractCurrency ac in currencies) totalAsettsAsFiat += ac.GetPrice() * ac.GetAssets(); 
		return totalAsettsAsFiat;
	}

	private void UpdateAssets() {
		assetsText.text = assets.ToString("#,0") + "yen";
	}

	private void CheckLimit() {
		if(cleared) return;
		if(now - limitTime > TimeSpan.Zero) {
			float totalAsettsAsFiat = calcTotalAssets();
			isLoanPaymented = loanPaymented;
			isAssetsGT1000 = totalAsettsAsFiat > 10000000;
			isAssetsGt30 = isAssetsGT1000 ? isAssetsGT1000 : totalAsettsAsFiat > 300000;
			cleared = true;
			SaveGame();
			ScoreRecord();
			StartCoroutine(RegisterToServer());
			SceneManager.LoadScene("GameOverScene");
		}
	}

	private void ScoreRecord() {
		PlayerPrefs.SetFloat("score", calcTotalAssets());
	}

	private IEnumerator RegisterToServer() {
		if(Application.internetReachability == NetworkReachability.NotReachable) yield return null;

		WWWForm form = new WWWForm();
		form.AddField("uid", PlayerPrefs.GetString("uid", "NoName"));
		form.AddField("score", calcTotalAssets().ToString("F2"));
		UnityWebRequest www = UnityWebRequest.Post("https://fuyasevoiceroid.mosin.jp/score/create", form);

		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		} else {
			Debug.Log("Score regist complete!");
		}
	}

	public void SaveGame() {
		PlayerPrefs.SetInt("save", 1);
		PlayerPrefs.SetString("now", now.ToString("yyyy/MM/dd HH:mm:ss"));
		PlayerPrefs.SetString("limitTime", limitTime.ToString("yyyy/MM/dd HH:mm:ss"));
		PlayerPrefs.SetFloat("fiat", assets);
		PlayerPrefs.SetString("MainCurrency", mainCurrency.GetName());
		PlayerPrefs.SetInt("loanPayment", loanPaymented ? 1 : -1);
		PlayerPrefs.SetInt("1000man", isAssetsGTsenman ? 1 : -1);
		PlayerPrefs.SetInt("cleared", cleared ? 1 : -1);
		foreach(AbstractCurrency currency in currencies) {
			PlayerPrefs.SetFloat(currency.GetName() + "Assets", currency.GetAssets());
			PlayerPrefs.SetInt(currency.GetName() + "Lv", currency.GetLv());
			PlayerPrefs.SetString(currency.GetName() + "Prices", currency.ToString());
		}
	}

	private void SaveEnvironment() {
		PlayerPrefs.SetInt("environment", 1);
		PlayerPrefs.SetFloat("bgm", bgmSlider.value);
		PlayerPrefs.SetFloat("character", characterSlider.value);
	}
	
	private void LoadEnvironment() {
		if(!PlayerPrefs.HasKey("environment")) return;
		float bgm = PlayerPrefs.GetFloat("bgm");
		float character = PlayerPrefs.GetFloat("character");
		bgmSlider.value = bgm;
		this.bgm.volume = bgm;
		characterSlider.value = character;
		foreach(AudioSource voice in characterVoices) voice.volume = character;
	}

	public void LoadGame() {
		string nowStr = PlayerPrefs.GetString("now");
		string limitStr = PlayerPrefs.GetString("limitTime");
		float assets = PlayerPrefs.GetFloat("fiat");
		if(nowStr == null || limitStr == null) return;	
		this.now = DateTime.Parse(nowStr);
		this.limitTime = DateTime.Parse(limitStr);
		this.assets = assets;
		if(PlayerPrefs.GetInt("loanPayment") == 1) RepaymentLoan();
		if(PlayerPrefs.GetInt("1000man") == 1) assetsGTsenman();
		if(PlayerPrefs.GetInt("cleared") == 1) cleared = true;
	}

	private void LoadCurrency(AbstractCurrency currency) {
		currency.SetAssets(PlayerPrefs.GetFloat(currency.GetName() + "Assets"));
		currency.SetLv(PlayerPrefs.GetInt(currency.GetName() + "Lv"));
		currency.SetPrices(PlayerPrefs.GetString(currency.GetName() + "Prices"));
	}

	public void ClickSaveButton() {
		SaveGame();
	}

	public void ClickSaveAndExitButton() {
		SaveGame();
		Exit();
	}

	public void ClickExitButton() {
		Exit();
	}

	private void Exit() {
		SceneManager.LoadScene("TitleScene");
	}

	public void ClickMenu() {
		MenuProc();
	}

	public void ClickLoan() {
		GameObject loanPanel = Instantiate(panelPrefab);
		loanPanel.transform.SetParent(GameObject.Find("MainBack").transform, false);
	}

	public void ChangeSlider() {
		float bgmVol = bgmSlider.value;
		float characterVol = characterSlider.value;
		SaveEnvironment();
		this.bgm.volume = bgmVol;
		foreach(AudioSource voice in characterVoices) voice.volume = characterVol; 
	}
}
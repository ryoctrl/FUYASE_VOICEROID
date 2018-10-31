using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractCurrency : MonoBehaviour {
	protected PriceSystem priceSystem;
	protected Text assetsText;
	protected Text valuationText;
	protected Text priceText;
	protected Text levelText;
	protected Text messageText;
	protected Text percentageText;
	protected Color myColor;
	protected float assets;
	public float miningEfficiency;
	protected float averageAcquisitionPrice;
	public GameObject levelUpPrefab;
	protected int level = 1;
	protected string currencyName;
	protected TradePanel trader;

	protected void Initialize() {
		Text[] objects = GetComponentsInChildren<Text>();
		foreach(Text obj in objects) {
			if(obj.name == "AssetsText") assetsText = obj;
			else if(obj.name == "ValuationText") valuationText = obj;
			else if(obj.name == "PriceText") priceText = obj;
			else if(obj.name == "LevelText") levelText = obj;
			else if(obj.name == "PercentageText") percentageText = obj;
			else if(obj.name == "MessageText") messageText = obj;
		}
		priceSystem = GetComponent<PriceSystem>();
		assets = 0;
		Game.Instance.AddCurrency(this);

		assetsText.color = myColor;
		valuationText.color = myColor;

		trader = GetComponentInChildren<TradePanel>();
		trader.SetCurrency(this);
	}

	protected void UpdateTexts() {
		assetsText.text = assets.ToString("F2");
		valuationText.text = (assets * priceSystem.GetPrice()).ToString("F2");
		priceText.text = priceSystem.GetPrice().ToString("F2");
		UpdatePercentage();
		UpdateLevelText();
	}

	private List<float> percentagePrices = null;
	private float latest, old;
	protected void UpdatePercentage() {
		percentagePrices = GetPrices();
		if(percentagePrices.Count < 7) return;
		latest = percentagePrices[percentagePrices.Count - 1];
		old = percentagePrices[percentagePrices.Count - 7];
		if(latest >= old) {
			percentageText.color = new Color32(0, 11, 255, 255);
			percentageText.text = "▲ " + (((latest - old) / old) * 100).ToString("F2");
		} else {
			percentageText.color = new Color(255, 0, 0, 255);
			percentageText.text = "▼ " + (((latest - old) / old) * 100).ToString("F2");
		}
	}

	public Color GetColor() {
		return myColor;
	}

	public string GetName() {
		return currencyName;
	}

	public int GetLv() {
		return level;
	}

	public void SetLv(int level) {
		this.level = level;
		UpdateLevelText();
	}

	public void SetAssets(float assets) {
		this.assets = assets;
	}

	public List<float> GetPrices() {
		return priceSystem.GetPrices();
	}

	public void Mining() {
		assets += miningEfficiency * level;
	}

	public void NewTick() {
		priceSystem.NewTick();
	}

	public float GetPrice() {
		return priceSystem.GetPrice();
	}

	public float GetAssets() {
		return assets;
	}

	public Sprite GetCurrentImage() {
		return priceSystem.GetCurrentImage();
	}

	public void Trade(int amount, bool isBuy) {
		float needAssets = amount * priceSystem.GetPrice();
		float allPrice = assets * averageAcquisitionPrice;
		if(isBuy) {
			if(needAssets <= Game.Instance.GetFiatAssets()) {
				Game.Instance.ChangeAssets(-needAssets);
				assets += amount;
				averageAcquisitionPrice = (allPrice + needAssets) / (amount + assets);
			}
		} else {
			float amountFlt = amount <= assets ? amount : assets;
			assets -= amountFlt;
			needAssets = amountFlt * priceSystem.GetPrice();
			Game.Instance.ChangeAssets(needAssets);
			averageAcquisitionPrice = (allPrice - needAssets) / (assets - amount);
		}
	}

	public void ClickLevelUp() {
		GameObject levelupPanel = Instantiate(levelUpPrefab);
		levelupPanel.transform.SetParent(transform, false);
		levelupPanel.GetComponent<LevelUpPanelScript>().setPriceAndCurrency(1000 * level, this);
	}

	public void LevelUp() {
		level++;
		UpdateLevelText();
	}

	private void UpdateLevelText() {
		levelText.text = (level * miningEfficiency).ToString() + "/日";
	}

	public override string ToString() {
		string result = priceSystem.ToString();
		return result;
	}

	public void SetPrices(string pricesText) {
		priceSystem.SetPrices(pricesText);
	}

	
}

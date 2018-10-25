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
	protected Color myColor;
	protected float assets;
	public float miningEfficiency;
	protected float averageAcquisitionPrice;
	public GameObject levelUpPrefab;
	protected int level = 1;
	protected string name;

	protected void Initialize() {
		Text[] objects = GetComponentsInChildren<Text>();
		foreach(Text obj in objects) {
			if(obj.name == "AssetsText") assetsText = obj;
			else if(obj.name == "ValuationText") valuationText = obj;
			else if(obj.name == "PriceText") priceText = obj;
			else if(obj.name == "LevelText") levelText = obj;
			//else Debug.Log(obj.name);
		}
		priceSystem = GetComponent<PriceSystem>();
		assets = 0;
		Game.Instance.AddCurrency(this);

		assetsText.color = myColor;
		valuationText.color = myColor;
	}

	protected void UpdateTexts() {
		assetsText.text = assets.ToString("F2");
		valuationText.text = (assets * priceSystem.GetPrice()).ToString("F2");
		if(priceText == null) Debug.Log("priceText is null!");
		else priceText.text = priceSystem.GetPrice().ToString("F2");
	}

	public Color GetColor() {
		return myColor;
	}

	public string GetName() {
		return name;
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

	protected void Trade(int amount, bool isBuy) {
		float needAssets = amount * priceSystem.GetPrice();
		float allPrice = assets * averageAcquisitionPrice;
		if(isBuy) {
			if(needAssets <= Game.Instance.GetFiatAssets()) {
				Game.Instance.ChangeAssets(-needAssets);
				assets += amount;
				averageAcquisitionPrice = (allPrice + needAssets) / (amount + assets);
			}
		} else {
			if(amount <= assets) {
				assets -= amount;
				Game.Instance.ChangeAssets(needAssets);
				averageAcquisitionPrice = (allPrice - needAssets) / (assets - amount);
			}
		}
		//Debug.Log(name + "コインの平均取得価格 : " + averageAcquisitionPrice);
	}

	public void ClickLevelUp() {
		GameObject levelupPanel = Instantiate(levelUpPrefab);
		levelupPanel.transform.SetParent(transform, false);
		levelupPanel.GetComponent<LevelUpPanelScript>().setPriceAndCurrency(100 * level, this);
	}

	public void LevelUp() {
		level++;
		UpdateLevelText();
	}

	private void UpdateLevelText() {
		levelText.text = level.ToString();
	}

	public override string ToString() {
		return priceSystem.ToString();
	}

	public void SetPrices(string pricesText) {
		priceSystem.SetPrices(pricesText);
		foreach(float p in priceSystem.GetPrices()) Debug.Log(p);
	}

	
}

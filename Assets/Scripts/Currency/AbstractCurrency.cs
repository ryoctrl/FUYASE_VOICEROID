using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractCurrency : MonoBehaviour {
	protected PriceSystem priceSystem;
	protected Text assetsText;
	protected float assets;
	public float miningEfficiency;
	protected string name;

	protected void Initialize() {
		priceSystem = GetComponent<PriceSystem>();
		Game.Instance.AddCurrency(this);
		assets = 0;
	}

	public string GetName() {
		return name;
	}

	public List<float> GetPrices() {
		return priceSystem.GetPrices();
	}

	public void Mining() {
		assets += miningEfficiency;
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
		if(isBuy) {
			if(needAssets <= Game.Instance.GetFiatAssets()) {
				Game.Instance.ChangeAssets(-needAssets);
				assets += amount;
			}
		} else {
			if(amount <= assets) {
				assets -= amount;
				Game.Instance.ChangeAssets(needAssets);
			}
		}
	
	}

	
}

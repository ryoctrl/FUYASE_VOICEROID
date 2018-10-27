using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PriceSystem : MonoBehaviour {
	protected float price;
	protected Image characterImage;
	protected Text priceText;
	protected AudioSource himeiSource;
	protected List<float> prices = new List<float>();

	public void Start () {
		characterImage = gameObject.GetComponentInChildren<Image>();
		himeiSource = gameObject.GetComponent<AudioSource>();
	}

	public List<float> GetPrices() {
		return prices;
	}

	public float GetPrice() {
		return price;
	}
	
	public void NewTick() {
		CalcPrice();
		AddPrice();
		UpdateImage();
	}

	private void AddPrice() {
		if(prices.Count == 48) {
			prices.RemoveAt(0);
		}
		prices.Add(price);
		Chart.Instance.Changed();
	}

	public Sprite GetCurrentImage() {
		return characterImage.sprite;
	}

	protected void PlayHimei() {
		himeiSource.Play();
	}

	public abstract List<Sprite> GetImages();
	protected abstract void CalcPrice();
	protected abstract void UpdateImage();

	public override string ToString() {
		string result = "";
		foreach(float p in prices) {
			result += p.ToString("F2");
			result += ",";
		}
		result = result.Length > 1 ? result.Substring(0, result.Length - 1) : result;
		return result;
	}

	public void SetPrices(string pricesText) {
		if(pricesText == null) return;
		string[] prices = pricesText.Split(',');
		this.prices = new List<float>();
		foreach(string price in prices) {
			float p = float.Parse(price);
			this.prices.Add(p);
			this.price = p;
		}
	}
}

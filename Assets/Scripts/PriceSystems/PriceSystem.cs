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
		priceText = gameObject.GetComponentInChildren<Text>();
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
		UpdatePriceText();
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

	protected void UpdatePriceText() {
		priceText.text = price.ToString("F2");
	}

	public abstract List<Sprite> GetImages();
	protected abstract void CalcPrice();
	protected abstract void UpdateImage();
}

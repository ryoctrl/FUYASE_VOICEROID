﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Chart : SingletonMonoBehaviour<Chart> {
	private const int DEFAULT_NUM_OF_CHART = 48;
	private List<float> prices = new List<float>();
	private GameObject chartPanel;
	private float panelHeight = 0;
	private float panelWidth = 0;
	private Text highestText;
	private Text middleText;
	private Text lowestText;
	private Text chartNameText;

	private RectTransform panelRect;

	private List<GameObject> lines = new List<GameObject>();
	private bool changed = false;

	public void SetPrices(List<float> prices, string name, Color color) {
		this.prices = prices;
		chartNameText.text = name + "/JPY";
		chartNameText.color = color;
		changed = true;
	}

	public void Changed() {
		changed = true;
	}

	void Start () {
		highestText = GameObject.Find("HighestPrice").GetComponent<Text>();
		middleText = GameObject.Find("MiddlePrice").GetComponent<Text>();
		lowestText = GameObject.Find("LowestPrice").GetComponent<Text>();
		chartNameText = GameObject.Find("ChartNameText").GetComponent<Text>();
		chartPanel = GameObject.Find("ChartPanel");
		panelRect = chartPanel.GetComponent<RectTransform>();
		UpdateRect();
	}
	
	void Update () {
		PointerOnChart();
		UpdateRect();
		DrawChart();
	}


	
	private void UpdateRect() {
		panelHeight = panelRect.rect.height;
		panelWidth = panelRect.rect.width;
	}

	private void DrawChart() {
		if(!changed) return;
		foreach(GameObject line in lines) Destroy(line.gameObject);
		lines = new List<GameObject>();
		if(prices.Count < 2) return;
		float max = prices.Max();
		float min = prices.Min();
		highestText.text = max.ToString("F2");
		lowestText.text = min.ToString("F2");
		middleText.text = ((max + min) / 2).ToString("F2");

		float diff = max - min;
		// float numOfChart = prices.Count > DEFAULT_NUM_OF_CHART ? DEFAULT_NUM_OF_CHART : prices.Count;
		// Debug.Log(numOfChart);
		// for(int i = 0; i < numOfChart - 1; i++) {
		for(int i = 0; i < prices.Count - 1; i++) {
			float startPrice = prices[i];
			float endPrice = prices[i + 1];
			Vector3 start = new Vector3(panelWidth / (prices.Count + 1) * (i + 1), panelHeight *  (startPrice - min) / diff, 90);
			Vector3 end = new Vector3(panelWidth / (prices.Count + 1) * (i + 2), panelHeight * (endPrice - min) / diff, 90);
			lines.Add(AddLine(start, end));
		}
		changed = false;
	}

	private GameObject AddLine(Vector3 start, Vector3 end) {
		start = chartPanel.transform.TransformPoint(start);
		end = chartPanel.transform.TransformPoint(end);
		start.z = 90;
		end.z = 90;
		GameObject lineObj = new GameObject();
		lineObj.transform.SetParent(chartPanel.transform);
		LineRenderer line = lineObj.AddComponent<LineRenderer>();
		line.material = new Material(Shader.Find("Standard"));

		line.positionCount = 2;
		line.startWidth = 0.5f;
		line.endWidth = 0.5f;
		line.SetPosition(0, start);
		line.SetPosition(1, end);
		Color c = start.y > end.y ? Color.red : Color.blue;
		line.material.color = c;
		return lineObj;
	}

	private bool pointerOnChart = false;

	public void PointerEnter(BaseEventData data) {
		pointerOnChart = true;		
	}
	
	public void PointerExit(BaseEventData data) {
		pointerOnChart = false;
	}

	private List<GameObject> pointerLines = new List<GameObject>();
	private void PointerOnChart() {
		foreach(GameObject line in pointerLines) Destroy(line.gameObject);
		if(!pointerOnChart) return;

		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 90;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);
		mousePos = chartPanel.transform.InverseTransformPoint(mousePos);
	
		pointerLines.Add(AddLine(new Vector3(mousePos.x, 0, 0), new Vector3(mousePos.x, panelHeight, 0)));
	}
}

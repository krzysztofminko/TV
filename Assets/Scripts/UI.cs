using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

	public static UI instance;

	[Header("References")]
	public Canvas gameCanvas;
	public TextMeshProUGUI timeText;
	public Canvas mainMenu;
	public TextMeshProUGUI playersText;
	public Slider playersSlider;
	public TextMeshProUGUI sizeText;
	public Slider sizeSlider;
	public TextMeshProUGUI bonusesText;
	public Slider bonusesSlider;
	public TextMeshProUGUI speedText;
	public Slider speedSlider;
	public TextMeshProUGUI durationText;
	public Slider durationSlider;
	public Button startButton;

	private void Awake()
	{
		instance = this;
		mainMenu.gameObject.SetActive(true);
		gameCanvas.gameObject.SetActive(false);
		Game.sfx["MenuBG"].Play();
	}

	private void Update()
	{
		timeText.text = "Time left: " + (int)Game.instance.time;
	}

	public void StartGame()
	{
		mainMenu.gameObject.SetActive(false);
		gameCanvas.gameObject.SetActive(true);
		Game.sfx["MenuBG"].Stop();
		Game.sfx["GameBG"].Play();
		Game.sfx["StartGame"].Play();
		Game.instance.StartGame();
	}

	public void MainMenu()
	{
		mainMenu.gameObject.SetActive(true);
		gameCanvas.gameObject.SetActive(false);
		Game.sfx["MenuBG"].Play();
		Game.sfx["GameBG"].Stop();
		Game.instance.MainMenu();
	}


	public void SetPlayers()
	{
		Game.instance.InitPlayers((int)playersSlider.value);
		playersText.text = "Players: " + Game.instance.players;
		Game.sfx["SliderMove"].Play();
	}

	public void SetSize()
	{
		Game.instance.InitGame((int)sizeSlider.value * 4, Game.instance.players);
		sizeText.text = "Size: " + Game.instance.size + "x" + Game.instance.size;
		Game.sfx["SliderMove"].Play();
	}

	public void SetBonuses()
	{
		Game.instance.bonuses = (int)bonusesSlider.value;
		bonusesText.text = "Bonuses: " + Game.instance.bonuses;
		Game.sfx["SliderMove"].Play();
	}

	public void SetSpeed()
	{
		Game.instance.aiSpeed = speedSlider.value * 0.1f;
		speedText.text = "AI speed: x" + Game.instance.aiSpeed;
		Game.sfx["SliderMove"].Play();
	}

	public void SetDuration()
	{
		Game.instance.duration = (int)durationSlider.value;
		durationText.text = "Duration: " + Game.instance.duration;
		Game.sfx["SliderMove"].Play();
	}
}

using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class CardPickerManager : Control
{
	[Export] Button readyButton;

	[Export] AudioStreamPlayer audioPlayer;

	[Export] Control PrepGUI;

	[Export] Button resetButtonP, resetButtonN;
	[Export] Button randomButtonP, randomButtonN;

	[Export] Control cardContainerP, cardContainerN;

	List<CardPicker> cardPickersP, cardPickersN;

	[Export] Label cardDescP;
	[Export] Label cardDescN;


	public GameManager gameManager;

	public int orderP = 0;
	public int orderN = 0;

	private static Random rng = new Random();

	bool canReady = false;
	public bool started = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cardPickersP = cardContainerP.GetChildren().Cast<CardPicker>().ToList();
		cardPickersN = cardContainerN.GetChildren().Cast<CardPicker>().ToList();

		foreach (CardPicker picker in cardPickersP) 
		{ 
			picker.manager = this;
		}
		foreach (CardPicker picker in cardPickersN) { picker.manager = this; }

		resetButtonN.Pressed += ResetN;
		resetButtonP.Pressed += ResetP;

		randomButtonN.Pressed += RandomN;
		randomButtonP.Pressed += RandomP;

		readyButton.Pressed += Start;

		GlobalEffects.PositiveCardList = cardPickersP.Select(o => o.card).ToList();
		GlobalEffects.NegativeCardList = cardPickersN.Select(o => o.card).ToList();
	}

	public void SetTooltipP(string text)
	{
		cardDescP.Text = text;
		cardDescN.Text = "";
	}
	public void SetTooltipN(string text)
	{
		cardDescP.Text = "";
		cardDescN.Text = text;
	}

	public void PlayBeep()
    {
		audioPlayer.Play();
    }

	public List<int> GetRandomOrder(int count)
    {
		return Enumerable.Range(0, count).OrderBy(x => rng.Next()).ToList();
	}

	public void RandomP()
    {
		ResetP();
		foreach (int i in GetRandomOrder(cardPickersP.Count))
        {
			cardPickersP[i].Select();
		}
		PlayBeep();
	}
	public void RandomN()
	{
		ResetN();
		foreach (int i in GetRandomOrder(cardPickersN.Count))
		{
			cardPickersN[i].Select();
		}
		PlayBeep();
	}

	public int GetOrderP(CardPicker picker)
	{
		int o = orderP;
		orderP++;

		GlobalEffects.CardOrderPositive.Add(cardPickersP.IndexOf(picker));

		return o;
	}
	public int GetOrderN(CardPicker picker)
	{
		int o = orderN;
		orderN++;

		GlobalEffects.CardOrderNegative.Add(cardPickersN.IndexOf(picker));

		return o;
	}

	public void ResetP()
    {
		foreach (CardPicker c in cardPickersP)
		{
			c.Reset();
		}
		orderP = 0;
		GlobalEffects.CardOrderPositive.Clear();
		PlayBeep();
	}
	public void ResetN()
	{
		foreach (CardPicker c in cardPickersN)
		{
			c.Reset();
		}
		orderN = 0;
		GlobalEffects.CardOrderNegative.Clear();
		PlayBeep();
	}

	bool CheckReady()
	{
		return cardPickersP.All(o => o.isSelected) && cardPickersN.All(o => o.isSelected);
	}

	public void Start()
    {
		StartGame();
    }

    private async void StartGame()
	{
		cardPickersN.ForEach(o => o.Disabled = true);
		cardPickersP.ForEach(o => o.Disabled = true);

		gameManager.SetupGameUI();

		Color transparent = new("#000000", 0.0f);
		var t = CreateTween();
		t.TweenProperty(PrepGUI, "modulate", transparent, 2.0);
		t.Play();

		await ToSignal(GetTree().CreateTimer(3.0), "timeout");
		gameManager.StartGame();
		PrepGUI.QueueFree();
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		canReady = CheckReady();
		readyButton.Disabled = !canReady;
	}
}

using Godot;
using System;
using System.Collections.Generic;

public partial class CardManager : Node
{
	[Export] Control GUI;
	[Export] Timer secondsTimer;

	[Export] AudioStreamPlayer audioPlayer;
	[Export] AudioStream tickSound;
	[Export] AudioStream warningSound;
	[Export] AudioStream swapSound;

	public CardDisplayManager cardDisplayManager;


    Player player;

	public int secondsSoFar = 0;

	public List<int> positiveOrder;
	public List<int> negativeOrder;

	public int index = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cardDisplayManager = GUI.GetNode("./CardDisplayManager") as CardDisplayManager;

		player = GetTree().Root.GetNode<Player>("World/Player");

		player.OnDeath += StopTimer;
		secondsTimer.Timeout += ProcessSecond;
	}
	public void SetupAndStart()
	{
		StartEffects();
		StartTimer();
	}
	public void InitCards()
    {
		foreach (Card card in GlobalEffects.PositiveCardList) { card.SetPlayer(player); }
		foreach (Card card in GlobalEffects.NegativeCardList) { card.SetPlayer(player); }

		positiveOrder = GlobalEffects.CardOrderPositive;
		negativeOrder = GlobalEffects.CardOrderNegative;
	}

	public void StartTimer()
	{
		SetUIDirty();
		secondsTimer.Start();
	}


	public void StopTimer()
    {
		secondsTimer.Stop();
    }

	public void CycleCards()
    {
		Cleanup();

		index++;
		if (index >= positiveOrder.Count) index = 0;
		SetUIDirty();

		StartEffects();
	}

    private void StartEffects()
	{
		GlobalEffects.PositiveCardList[positiveOrder[index]].StartEffect();
		GlobalEffects.NegativeCardList[negativeOrder[index]].StartEffect();
	}

    void Cleanup()
	{
		GlobalEffects.PositiveCardList[positiveOrder[index]].EndEffect();
		GlobalEffects.NegativeCardList[negativeOrder[index]].EndEffect();
	}

    public void SetUIDirty()
	{
		cardDisplayManager.SetDescriptions(
			GlobalEffects.PositiveCardList[positiveOrder[index]].cardDescription,
			GlobalEffects.NegativeCardList[negativeOrder[index]].cardDescription);

		cardDisplayManager.SetClockRotation(secondsSoFar);

		SendCards();
	}

	void SendCards() 
    {
		var posList = new List<Card>();
		var negList = new List<Card>();

		int j = index;
		for(int i = 0; i < 5; i++)
		{
			posList.Add(GlobalEffects.PositiveCardList[positiveOrder[j]]);
			negList.Add(GlobalEffects.NegativeCardList[negativeOrder[j]]);
			j++;
			if (j >= positiveOrder.Count) j = 0;
		}


		posList.Reverse();
		cardDisplayManager.SetCards(posList, negList);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ProcessSecond()
	{ 
		secondsSoFar++;

		AudioStream stream = secondsSoFar switch
		{
			< 7 => tickSound,
			>= 7 and < 10 => warningSound,
			>= 10 => swapSound
		};

		audioPlayer.Stream = stream;
		audioPlayer.Play();

		if (secondsSoFar > 9)
		{
			CycleCards();
			secondsSoFar = 0;
		}

		SetUIDirty();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		if(Input.IsActionJustPressed("debug"))
        {
			player.damageTakenMultiplier = 0f;
			CycleCards();
        }
    }
}

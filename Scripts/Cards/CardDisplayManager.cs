using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CardDisplayManager : Node
{
	List<CardDisplay> positiveCardNodes;
	List<CardDisplay> negativeCardNodes;

	[Export] public Node positiveCardRoot;
	[Export] public Node negativeCardRoot;

	[Export] Label positiveLabel, negativeLabel;
	[Export] TextureRect clockHand;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{													 
		positiveCardNodes = positiveCardRoot.GetChildren(true).Cast<CardDisplay>().ToList();
		negativeCardNodes = negativeCardRoot.GetChildren(true).Cast<CardDisplay>().ToList();
	}

	public void SetDescriptions(string positiveMessage, string negativeMessage)
    {
		positiveLabel.Text = positiveMessage;
		negativeLabel.Text = negativeMessage;
    }

	public void SetClockRotation(int seconds)
    {
		clockHand.Rotation = seconds * Mathf.DegToRad(36);
    }

	public void SetCards(List<Card> positiveCards, List<Card> negativeCards)
	{
		for (int i = 0; i < 5; i++)
		{
			positiveCardNodes[i].SetIcon(positiveCards[i].cardImage);
			negativeCardNodes[i].SetIcon(negativeCards[i].cardImage);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

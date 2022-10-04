using Godot;
using System;

public partial class CardPicker : Button
{
	[Export] CardDisplay cardDisplay;
	[Export] public Card card;
	[Export] Label orderLabel;
	[Export] bool isPositive = false;

	public CardPickerManager manager;

	public bool isSelected = false;

	Color pressedCol = new Color("#545454", 1.0f);
	Color unpressedCol = new Color("#FFFFFF", 1.0f);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		card.Init();
		cardDisplay.SetIcon(card.cardImage);
		Pressed += Select;
		Pressed += Beep;
		MouseEntered += EventMouseEntered;
	}

	public void EventMouseEntered()
    {
		if (isPositive) manager.SetTooltipP(card.GetText());
		else manager.SetTooltipN(card.GetText());
    }

	public void SetOrder(int order)
    {
		if (order < 0) orderLabel.Text = "";
		else orderLabel.Text = (order+1).ToString();
    }

	public void Reset()
    {
		isSelected = false;
		SetOrder(-1);
    }

    public void Beep()
    {
		manager.PlayBeep();
	}

	public void Select()
    {
		if (isSelected) return;
		isSelected = true;
		SetOrder((isPositive) ? manager.GetOrderP(this) : manager.GetOrderN(this));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		cardDisplay.Modulate = isSelected ? pressedCol : unpressedCol;
	}
}

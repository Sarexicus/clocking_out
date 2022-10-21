using Godot;
using System;
using System.Collections.Generic;
using Discord;

public partial class GlobalEffects : Node
{

    public static float EnemyWeaponFireRateModifier = 1.0f;
	public static bool GameOver = false;

	public static List<int> CardOrderPositive = new List<int>();
	public static List<int> CardOrderNegative = new List<int>();

	public static List<Card> PositiveCardList = new List<Card>();
	public static List<Card> NegativeCardList = new List<Card>();

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }
}

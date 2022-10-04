using Godot;
using System;
using System.Collections.Generic;

public partial class GlobalEffects : Node
{
	public static float EnemyWeaponFireRateModifier = 1.0f;
	public static bool GameOver = false;

	public static List<int> CardOrderPositive = new List<int>();
	public static List<int> CardOrderNegative = new List<int>();

	public static List<Card> PositiveCardList = new List<Card>();
	public static List<Card> NegativeCardList = new List<Card>();
}

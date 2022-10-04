using Godot;
using System;

public partial class Wave : Node
{
	[Export] public int maxEnemies;
	[Export] public Godot.Collections.Array<int> enemies;
}

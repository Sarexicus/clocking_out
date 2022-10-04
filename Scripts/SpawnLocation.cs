using Godot;
using System;

public partial class SpawnLocation : Node2D
{
	[Export] Area2D area;

    public override void _Ready()
    {
        Modulate = (new Color("#000000", 0.0f));
    }

    public bool IsEnemyNearby()
    {
		var oBodies = area.GetOverlappingBodies();
		foreach (Node2D body in oBodies)
        {
			if(body is HealthObject)
            {
				return true;
            }
        }
		return false;
    }
}

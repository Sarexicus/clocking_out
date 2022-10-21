using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	[Export] Label valueLabel;

    int health, maxHealth;

	public void SetHealth(int value, bool instant = false)
    {
        health = value;
        if (instant) Value = value;
        SetUIDirty();
    }

	public void SetMaxHealth(int value, bool instant = false)
    {
        maxHealth = value;
        if (instant) MaxValue = value;
        SetUIDirty();
    }

    void SetUIDirty()
    {
        valueLabel.Text = $"{health}/{maxHealth}";
    }

    public override void _Process(double delta)
    {
        Value = Mathf.MoveToward((float)Value, health, 2);
        MaxValue = Mathf.MoveToward((float)MaxValue, maxHealth, 4);
    }
}

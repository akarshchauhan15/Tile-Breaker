using Godot;
using System;

public partial class Bullet : Area2D
{
    float Speed = 600;

    public override void _Ready()
    {
        BodyEntered += OnCollided;
    }
    public override void _Process(double delta)
    {
        Position += Speed * (float)delta * Vector2.Up;
    }
    private void OnCollided(Node2D body)
    {
        if (body is not Tile)
            return;

        ((Tile)body).OnHit();
        QueueFree();
    }
}

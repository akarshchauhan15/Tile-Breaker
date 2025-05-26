using Godot;

public partial class Hud : Control
{
    public override void _Ready()
    {
        GetNode<Button>("StartButton").Pressed += StartGame;
        GetNode<Button>("PlaceTileButton").Pressed += 
    }
}

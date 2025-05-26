using Godot;

public partial class Hud : Control
{
    Button PlayButton;
    Button PlaceTileButton;
    Main main;
    public override void _Ready()
    {
        PlayButton = GetNode<Button>("PlayButton");
        PlaceTileButton = GetNode<Button>("PlaceTileButton");
        main = GetParent<Main>();
    }
    public void EndGame(bool Win)
    {
        Main.isPlaying = false;
        Main.TilePlaced = false;

        PlayButton.Show();
        PlaceTileButton.Show();
    }
}

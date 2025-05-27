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
        /*
        Main.isPlaying = false;

        if (Player.Lives <= 0)
        {
            PlaceTileButton.Show();
            Main.TilePlaced = false;

            if (Win)
                GetNode<Label>("Label/WinLabel").Show();
            else
                GetNode<Label>("Label/LoseLabel").Show();
        }

        PlayButton.Show();
        */

        Main.isPlaying = false;
        Player.Lives--;

        if (Player.Lives < 0 || Win)
        {
            if (Win)
                GetNode<Label>("Label/WinLabel").Show();
            else
                GetNode<Label>("Label/LoseLabel").Show();

            PlaceTileButton.Show();
            Main.TilePlaced = false;

            return;
        }

            PlayButton.Show();
    }
}

using Godot;
using Godot.Collections;

public partial class Hud : Control
{
    Button PlayButton;
    Button PlaceTileButton;
    AnimationPlayer Anim;
    Main main;

    Array<AudioStreamPlayer> Audios = new Array<AudioStreamPlayer>();

    public override void _Ready()
    {
        PlayButton = GetNode<Button>("PlayButton");
        PlaceTileButton = GetNode<Button>("PlaceTileButton");
        Anim = GetNode<AnimationPlayer>("AnimationPlayer");
        main = GetParent<Main>();

        GetNode<Button>("StartScreen/LaunchButton").Pressed += EnterGame;

        foreach (AudioStreamPlayer Audio in GetNode<Node>("Audio").GetChildren())
            Audios.Add(Audio);
    }
    public void EndGame(bool Win)
    {
        Main.isPlaying = false;
        Player.Lives--;

        if (Player.Lives <= 0 || Win)
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
    public void PlaySound(string SoundName)
    {
        AudioStreamPlayer Stream = GetNodeOrNull<AudioStreamPlayer>("Audio/" + SoundName);

        if (Stream != null)
            Stream.Play();
        else
            GD.Print("Error: Sound not found!");
    }
    private void EnterGame() => Anim.Play("EnterGame");
}

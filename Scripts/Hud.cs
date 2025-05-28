using Godot;
using Godot.Collections;

public partial class Hud : Control
{
    Button PlayButton;
    Button PlaceTileButton;
    Main main;

    static Array<AudioStreamWav> Audios;

    public override void _Ready()
    {
        PlayButton = GetNode<Button>("PlayButton");
        PlaceTileButton = GetNode<Button>("PlaceTileButton");
        main = GetParent<Main>();

        GetAudioFiles();
    }
    public void EndGame(bool Win)
    {
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
    public static void PlaySound(int Sound)
    {
        AudioStreamPlayer p = new AudioStreamPlayer();
        p.Play();
    }
    private void GetAudioFiles()
    {
        string Path = "res://Assets/Sound";
        DirAccess dir = DirAccess.Open(Path);

        dir.ListDirBegin();
        string fileName = dir.GetNext();

        while (!string.IsNullOrEmpty(fileName))
        {
            if (!dir.CurrentIsDir() && fileName.GetExtension().ToLower() == "wav")
            {
                Audios.Add(GD.Load<AudioStreamWav>(Path + "/" + fileName));
            }
            fileName = dir.GetNext();
        }

        dir.ListDirEnd();
    }
}

using Godot;
using Godot.Collections;

public partial class Hud : Control
{
    Button PlayButton;
    Button PlaceTileButton;
    AnimationPlayer Anim;
    Main main;
    Button SoundButton;

    Array<AudioStreamPlayer> Audios = new Array<AudioStreamPlayer>();
    Tween tween;

    public override void _Ready()
    {
        PlayButton = GetNode<Button>("PlayButton");
        PlaceTileButton = GetNode<Button>("PlaceTileButton");
        Anim = GetNode<AnimationPlayer>("AnimationPlayer");
        main = GetParent<Main>();
        SoundButton = GetNode<CheckButton>("Slides/SettingsSlide/ColorRect/Settings/SoundButton");

        GetNode<Button>("StartScreen/LaunchButton").Pressed += EnterGame;
        SoundButton.Toggled += SoundButtonToggled;

        foreach (AudioStreamPlayer Audio in GetNode<Node>("Audio").GetChildren())
            Audios.Add(Audio);
        SetSettings();
    }
    public void EndGame(bool Win)
    { 
        Player.Lives--;
        ShowLifeInformation(Player.Lives);

        if (Player.Lives <= 0 || Win)
        {
            if (Win)
                GetNode<Label>("Label/WinLabel").Show();
            else
                GetNode<Label>("Label/LoseLabel").Show();

            Main.isPlaying = false;
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
    private void ShowLifeInformation(int Lives)
    {
        Panel HealthPanel = GetNode<Panel>("LifeIndicator");
        HealthPanel.GetNode<Label>("Lives").Text = Lives.ToString();

        if (tween != null)
            tween.Kill();

        tween = CreateTween();
        tween.TweenProperty(HealthPanel, "position", new Vector2(0, 720 - HealthPanel.Size.Y), 0.4 * Mathf.InverseLerp(720 - HealthPanel.Size.Y, 720, HealthPanel.Position.Y)).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
        tween.TweenProperty(HealthPanel, "position", new Vector2(0, 720), 0.4).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quad).SetDelay(1);
    }
    private void SetSettings()
    {
        Main.CurrentPreset = SettingsSlide.AllTilePresets[(int) ConfigController.config.GetValue("Settings", "Tileset", 0)];
        SoundButton.ButtonPressed = (bool) ConfigController.config.GetValue("Settings", "Sounds", true);
    }
    private void SoundButtonToggled(bool Value)
    { 
        AudioServer.SetBusMute(1, !Value);
        ConfigController.SaveSetting("Settings", "Sounds", Value);
    }
    private void EnterGame() => Anim.Play("EnterGame");
}
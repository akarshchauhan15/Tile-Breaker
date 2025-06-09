using Godot;
using Godot.Collections;
using System;

public partial class Hud : Control
{
    Button PlayButton;
    Button PlaceTileButton;
    AnimationPlayer Anim;
    Main main;
    Button SoundButton;
    public static Label ScoreLabel;

    Array<AudioStreamPlayer> Audios = new Array<AudioStreamPlayer>();
    Tween tween;

    public override void _Ready()
    {
        PlayButton = GetNode<Button>("PlayButton");
        PlaceTileButton = GetNode<Button>("PlaceTileButton");
        Anim = GetNode<AnimationPlayer>("AnimationPlayer");
        main = GetParent<Main>();
        ScoreLabel = GetNode<Label>("Slides/SettingsSlide/ColorRect/Score");
        SoundButton = GetNode<CheckButton>("Slides/SettingsSlide/ColorRect/Settings/SoundButton");

        GetNode<Button>("StartScreen/LaunchButton").Pressed += EnterGame;
        SoundButton.Toggled += SoundButtonToggled;

        foreach (AudioStreamPlayer Audio in GetNode<Node>("Audio").GetChildren())
            Audios.Add(Audio);

        SetSettings();
        SetScoreList(true);
    }
    public void EndGame(bool Win)
    {
        Player.Lives--;

        if (Player.Lives <= 0 || Win)
        {
            AddScore(Player.Lives * 120);
            if (Win)
            {
                GetNode<Label>("Label/WinLabel").Show();
                ScoreController.AddScores(Player.Score);
                SetScoreList(false);
            }
            else
                GetNode<Label>("Label/LoseLabel").Show();

            Main.isPlaying = false;
            Main.TilePlaced = false;
            PlaceTileButton.Show();
            
            GetNode<Slide>("Slides/RetrySlide").Hide();
        }
        else
        {
            ShowLifeInformation(Player.Lives);
            PlayButton.Show();
        }
    }
    public void PlaySound(string SoundName)
    {
        AudioStreamPlayer Stream = GetNodeOrNull<AudioStreamPlayer>("Audio/" + SoundName);

        if (Stream != null)
            Stream.Play();
        else
            GD.Print("Error: Sound not found!");
    }
    public static void AddScore(int Score)
    {
        if (!Main.isPlaying)
            return;
        Player.Score += Score;
        ScoreLabel.Text = Player.Score.ToString();
    }
    private void ShowLifeInformation(int Lives)
    {
        Panel HealthPanel = GetNode<Panel>("LifeIndicator");
        HealthPanel.GetNode<Label>("Lives").Text = Lives.ToString();

        if (tween != null)
            tween.Kill();

        tween = CreateTween();
        tween.TweenProperty(HealthPanel, "position", new Vector2(0, 720 - HealthPanel.Size.Y), 0.3 * Mathf.InverseLerp(720 - HealthPanel.Size.Y, 720, HealthPanel.Position.Y)).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
        tween.TweenProperty(HealthPanel, "position", new Vector2(0, 720), 0.3).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quad).SetDelay(0.7);
    }
    private void SetScoreList(bool FirstLoad)
    {
        ScoreController.LoadScores();

        Tuple<string, string> Score = ScoreController.GetScoreListString();
        GetNode<Label>("Slides/ScoreSlide/ColorRect/ScoreList").Text = Score.Item1;
        GetNode<Label>("Slides/ScoreSlide/ColorRect/DateList").Text = Score.Item2;

        GetNode<Label>("Slides/ScoreSlide/ColorRect/NoScorePrompt").Visible = FirstLoad && Score.Item1 == "";
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
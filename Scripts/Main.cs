using Godot;
using System;

public partial class Main : CanvasLayer
{
    [Signal]
    public delegate void OnGameStartedEventHandler();

    public static bool isPlaying = false;
    public static bool TilePlaced = true;

    public static TilePreset CurrentPreset = TilePresets.TENxTWELVE;
    TilePreset AppliedPreset = CurrentPreset;

    PackedScene Tile = (PackedScene) ResourceLoader.Load("res://Scenes/tile.tscn");
    PackedScene MainStrikerScene = (PackedScene) ResourceLoader.Load("res://Scenes/striker.tscn");

    Hud HUD;
    Node TileContainer;
    Player player;
    Striker MainStriker;

    public override void _Ready()
    {   
        TileContainer = GetNode<Node>("PlayingArea/Tiles");
        HUD = GetNode<Hud>("HUD");

        HUD.GetNode<Button>("PlayButton").Pressed += OnStartButtonPressed;
        
        Button SetButton = HUD.GetNode<Button>("PlaceTileButton");
        SetButton.Pressed += OnTilePlaceButtonPressed;
        SetButton.Hide();

        player = GetNode<Player>("PlayingArea/Player");
        OnGameStarted += player.OnGameStarted;

        GetNode<SettingsSlide>("HUD/Slides/SettingsSlide").TilePresetChanged += SetTile;

        AddMainStriker(false);
        SetTile();
    }
    private void OnStartButtonPressed()
    {
        if (Player.Lives <= 0)
            return;
        
        isPlaying = true;

        Random random = new Random();
        MainStriker.Direction = new Vector2((float)(random.NextSingle()-0.5), -1).Normalized();
        HUD.GetNode<Button>("PlayButton").Hide();

        //HUD.RetrySlide.Show();
        //HUD.SettingsSlide.Hide();
    }
    public void OnTilePlaceButtonPressed()
    {
        if (TilePlaced)
        return;

        foreach (Node striker in GetNode<Node>("PlayingArea/Strikers").GetChildren())
            striker.CallDeferred(Striker.MethodName.Free);
        foreach (Node drop in GetNode<Node>("PlayingArea/Drops").GetChildren())
            drop.CallDeferred(BaseDrop.MethodName.Free);

        SetTile();
        TilePlaced = true;

        HUD.GetNode<Button>("PlaceTileButton").Hide();
        HUD.GetNode<Label>("Label/WinLabel").Hide();
        HUD.GetNode<Label>("Label/LoseLabel").Hide();

        GetNode<Player>("PlayingArea/Player").GlobalPosition = new Vector2(640, 657);

        Player.Lives = 3;
        AddMainStriker(true);

        HUD.GetNode<Button>("PlayButton").Show();
        EmitSignal(SignalName.OnGameStarted);
    }
    public void AddMainStriker(bool win)
    {
        if (Player.Lives <= 0)
            return;

        MainStriker = (Striker)MainStrikerScene.Instantiate();
        MainStriker.GlobalPosition = new Vector2(640, 580);

        GetNode<Node>("PlayingArea/Strikers").AddChild(MainStriker);

        Tween tween = MainStriker.CreateTween();
        tween.SetParallel();
        tween.TweenProperty(MainStriker.GetNode<Sprite2D>("Striker"), "scale", new Vector2(0.05f, 0.05f), 0.5).From(Vector2.Zero).SetEase(Tween.EaseType.InOut);
    }
    private void SetTile()
    {
        if (AppliedPreset != CurrentPreset)
        {
            foreach (Node Tile in GetNode<Node>("PlayingArea/Tiles").GetChildren())
                Tile.Free();
            AppliedPreset = CurrentPreset;
        }
        int Row = 0;

        while (Row < CurrentPreset.TotalTiles.X)
        {
            int Column = 0;

            while (Column < CurrentPreset.TotalTiles.Y){
                PlaceTile(Row, Column);
                Column++;
            }
            Row++;
        }
    }
    public void PlaceTile(int Row, int Column)
    {
        String Name = $"Tile_{Row}_{Column}";
        if (TileContainer.HasNode(Name))
        return;

        Tile CurrentTile = (Tile) Tile.Instantiate();

        CurrentTile.GlobalPosition = new Vector2(
            CurrentPreset.TileOffset.X + (CurrentPreset.TileSize.X + CurrentPreset.TileSpacing.X) * Column,
            CurrentPreset.TileOffset.Y + (CurrentPreset.TileSize.Y + CurrentPreset.TileSpacing.Y) * Row);
        
        CurrentTile.GetNode<MeshInstance2D>("Style").SelfModulate = TileData.TileColors[Mathf.Abs(6 - Row % 13)];
        CurrentTile.Name = Name;
        CurrentTile.Row = Row;

        if (isPlaying)
        {
            Tween tween = CurrentTile.CreateTween();
            tween.SetParallel();

            tween.TweenProperty(CurrentTile, "modulate:a", 1, 0.3).From(0);
            tween.TweenProperty(CurrentTile, "scale", Vector2.One, 0.2).From(Vector2.Zero).SetEase(Tween.EaseType.In);
        }
        CurrentPreset.SetSize(CurrentTile);
        TileContainer.CallDeferred(Node.MethodName.AddChild, CurrentTile);

        CurrentTile.RespawnTile += PlaceTile;
        CurrentTile.AllTileDissapeared += HUD.EndGame;
    }
}
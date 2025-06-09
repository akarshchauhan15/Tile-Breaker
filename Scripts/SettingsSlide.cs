using Godot;

public partial class SettingsSlide : Slide
{
    [Signal]
    public delegate void TilePresetChangedEventHandler();

    public static TilePreset[] AllTilePresets = [TilePresets.SEVENxSEVEN, TilePresets.EIGHTxNINE, TilePresets.TENxTEN, TilePresets.TENxTWELVE];
    public override void _Ready()
    {
        base._Ready();
        foreach (Button button in GetNode<GridContainer>("ColorRect/Settings/GridContainer").GetChildren())
            button.Pressed += () => OnTileSetSelected(button.Name.ToString().ToInt() - 1);;

        GetNode<Label>("ColorRect/Settings/Version").Text = "VERSION " + ProjectSettings.GetSetting("application/config/version").ToString();
    }
    private void OnTileSetSelected(int Selected)
    {
        if (Main.isPlaying || !Main.TilePlaced)
            return;

        Main.CurrentPreset = AllTilePresets[Selected];
        EmitSignal(SignalName.TilePresetChanged);

        ConfigController.SaveSetting("Settings", "Tileset", Selected);
    }
}

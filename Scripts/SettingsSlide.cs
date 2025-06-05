using Godot;
using System;

public partial class SettingsSlide : Slide
{
    TilePreset[] AllTilePresets = [TilePresets.SEVENxSEVEN, TilePresets.EIGHTxNINE, TilePresets.TENxTWELVE];
    public override void _Ready()
    {
        foreach (Button button in GetNode<GridContainer>("ColorRect/Settings/GridContainer").GetChildren())
            button.Pressed += () => OnTileSetSelected(button.Name.ToString().ToInt());;
    }
    private void OnTileSetSelected(int Selected)
    {

    }
}

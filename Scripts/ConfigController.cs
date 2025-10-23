using Godot;

public partial class ConfigController : Node
{
    public static ConfigFile config = new ConfigFile();

    public static string path = "res://settings.ini";

    public override void _Ready()
    {
        if (!FileAccess.FileExists(path))
            ResetSettings();
        else
        {
            config.Load(path);
            GetTree().Root.GetNode<Control>("Main/HUD/InitialPage").QueueFree();
        }
    }
    public static void ResetSettings()
    {
        config.SetValue("Settings", "Tileset", 0);
        config.SetValue("Settings", "Sounds", true);
        config.SetValue("Settings", "Particles", true);
        config.SetValue("Settings", "MaxFps", 180);
        config.SetValue("Settings", "Vsync", true);

        config.Save(path);
        AppendComment();
    }
    public static void SaveSetting(string Section, string Key, Variant Value)
    {
        config.SetValue(Section, Key, Value);
        config.Save(path);
        AppendComment();
    }
    private static void AppendComment()
    {
        string Text = FileAccess.GetFileAsString(path);
        using var File = FileAccess.Open(path, FileAccess.ModeFlags.Write);

        string Comment = """ 


            # These settings can be changed by modifying their values.
            # Delete this file to reset settings to default.
            # MaxFps are clamped between 24 and 480. Values not lying in this range will be overridden.
            """;
        File.StoreString(Text + Comment);
    }

}

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

        config.Save(path);
    }
    public static void SaveSetting(string Section, string Key, Variant Value)
    {
        config.SetValue(Section, Key, Value);
        config.Save(path);
    }
}

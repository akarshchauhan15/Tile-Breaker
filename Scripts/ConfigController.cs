using Godot;

public partial class ConfigController : Node
{
    public static ConfigFile Config = new ConfigFile();
    public static string Path = "res://settings.ini";

    public override void _Ready()
    {
        if (!FileAccess.FileExists(Path))
        {
            
            Config.SetValue("LastSelected", "Given", 0);
            Config.SetValue("LastSelected", "Return", 2);
            Config.SetValue("LastSelected", "ElementCollectionType", 0);
            Config.SetValue("LastSelected", "ElementCollectionList", 0);
            Config.SetValue("Settings", "Theme", 0);
            Config.Save(Path);
        }
        else
            Config.Load(Path);
    }
    public static void SaveSettings(string Section, string Key, Variant Value)
    {
        Config.SetValue(Section, Key, Value);
        Config.Save(Path);
    }
}

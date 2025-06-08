using Godot;

public class TileData
{
    public enum TileTag
    {
        None,
        NewStriker,
        Explosion,
        CrossExplosion,
        Respawn,
        CrossRespawn
    }
    public enum TileDrop 
    {
        None,
        Saver,
        Rigidify,
        Extender,
        Bullet
    }
    public static string[] TileDropDisplay = 
        [
        "SAVER",
        "RIGID",
        "EXTEND",
        "GUN"
        ];
    public static Color[] TileColors =
        [
        new Color("#b751bc"),
        new Color("#dc3d5a"),
        new Color("#ec7200"),
        new Color("#ced639"),
        new Color("#3cc148"),
        new Color("#325bcd"),
        new Color("#7a29cf"),
        ];
}
public class TilePreset
{ 
    public Vector2 TotalTiles { get; set; }
    public Vector2 TileSize { get; set; }
    public Vector2 TileSpacing { get; set; }
    public Vector2 TileOffset { get; set; }

    public TilePreset(Vector2 totalTiles, Vector2 tileSize, Vector2 tileSpacing, Vector2 tileOffset)
    {
        TotalTiles = totalTiles;
        TileSize = tileSize;
        TileSpacing = tileSpacing;
        TileOffset = tileSize/2 + tileOffset;
    }
    public void SetSize(Tile tile)
    {
        RectangleShape2D Rect = new RectangleShape2D();
        Rect.Size = TileSize;

        tile.GetNode<CollisionShape2D>("CollisionShape2D").Shape = Rect;

        QuadMesh mesh = new QuadMesh();
        mesh.Size = TileSize;
        tile.GetNode<MeshInstance2D>("Style").Mesh = mesh;

        OccluderPolygon2D occluderPolygon2D = new OccluderPolygon2D();

        occluderPolygon2D.Polygon = new Vector2[] {
            new Vector2(- TileSize.X/2, - TileSize.Y/2),
            new Vector2(- TileSize.X/2,  TileSize.Y/2),
            new Vector2( TileSize.X/2,  TileSize.Y/2),
            new Vector2( TileSize.X/2, - TileSize.Y/2),
        };

        tile.GetNode<LightOccluder2D>("LightOccluder2D").Occluder = occluderPolygon2D;
    }
}
public class TilePresets
{
    public static TilePreset SEVENxSEVEN = new TilePreset(new Vector2(7, 7), new Vector2(150, 40), new Vector2(15, 10), new Vector2(66, 80));
    public static TilePreset EIGHTxNINE = new TilePreset(new Vector2(8, 9), new Vector2(120, 36), new Vector2(10, 9), new Vector2(66, 80));
    public static TilePreset TENxTEN = new TilePreset(new Vector2(10, 10), new Vector2(106, 32), new Vector2(8, 8), new Vector2(80, 80));
    public static TilePreset TENxTWELVE = new TilePreset(new Vector2(10, 12), new Vector2(90, 27), new Vector2(8, 8), new Vector2(60, 60));
    public static TilePreset ONExFOUR = new TilePreset(new Vector2(1, 4), new Vector2(1000, 80), new Vector2(15, 10), new Vector2(100, 80));
}


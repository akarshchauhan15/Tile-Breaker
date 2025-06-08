using Godot;
using System;

public partial class Tile : StaticBody2D
{   
    [Signal]
    public delegate void RespawnTileEventHandler(int Row, int Column);
    [Signal]
    public delegate void AllTileDissapearedEventHandler(bool Win);

    PackedScene Striker = ResourceLoader.Load<PackedScene>("res://Scenes/striker.tscn");
    PackedScene DropScene = ResourceLoader.Load<PackedScene>("res://Scenes/base_drop.tscn");
    CollisionShape2D Collision;

    TileData.TileTag tileTag = TileData.TileTag.None;
    TileData.TileDrop tileDrop = TileData.TileDrop.None;

    public int Row;

    public override void _Ready()
    {
        Collision = GetNode<CollisionShape2D>("CollisionShape2D");
        SetTagAndDrop();   
    }
    public void OnHit(Vector2 HitDirection)
    {
        PlayDeathAnimation(HitDirection);
        if (tileTag == TileData.TileTag.None)
        return;

        if (tileTag == TileData.TileTag.NewStriker){
            Node container = GetNode<Node>("../../Strikers");

            if (container.GetChildCount() >= 3)
            return;
        
            Striker CurrentStriker = (Striker)Striker.Instantiate();
            container.CallDeferred(Node.MethodName.AddChild, CurrentStriker);
            
            CurrentStriker.Direction = new Vector2(1, (float) new Random().NextDouble() - 1).Normalized();
            CurrentStriker.GlobalPosition = GlobalPosition;
            return;
        }

        Node Container = GetParent<Node>();
        Vector2I Coord = GetCoordinates(Name);
        Vector2I[] Directions = [Vector2I.Up, Vector2I.Down, Vector2I.Left, Vector2I.Right];

        if (tileTag == TileData.TileTag.Explosion)
        {
            foreach (Vector2I Neighbour in Directions)
            {
                Tile NeighbourTile = GetTile(Container, Coord + Neighbour);

                if (NeighbourTile == null)
                continue;

                NeighbourTile.CallDeferred(Tile.MethodName.PlayDeathAnimation, Neighbour);
            }
        }
        else if (tileTag == TileData.TileTag.CrossExplosion)
        {          
            foreach (Vector2I Direction in Directions)
            {
                Vector2I NewCoord = Coord;
                bool TileInRange = true;

                while (TileInRange)
                {
                    NewCoord += Direction;
                    TileInRange = IsTileInRange(NewCoord);
                    if (!TileInRange)
                    break;

                    Tile ExistingTile = GetTile(Container, NewCoord);
                    if (ExistingTile == null)
                    continue;
                        
                    ExistingTile.CallDeferred(Tile.MethodName.PlayDeathAnimation, Direction);
                }
            }
        }
        else if (tileTag == TileData.TileTag.Respawn)
        {
            foreach (Vector2I Neighbour in Directions)
            {
                Vector2I NewCoord = Coord + Neighbour;
                bool ExistingTile = HasTile(Container, Coord);
                bool TileInRange = IsTileInRange(NewCoord);

                if (ExistingTile || !TileInRange)
                continue;

                EmitSignal(SignalName.RespawnTile, [NewCoord.X, NewCoord.Y]);
            }
        }
        else if (tileTag == TileData.TileTag.CrossRespawn)
        {
            foreach (Vector2I Direction in Directions)
            {
                Vector2I NewCoord = Coord;
                bool TileInRange = true;

                while (TileInRange)
                {
                    NewCoord += Direction;
                    TileInRange = IsTileInRange(NewCoord);
                    if (!TileInRange)
                    break;

                    bool ExistingTile = HasTile(Container, NewCoord);
                    if (ExistingTile)
                    continue;
                        
                    EmitSignal(SignalName.RespawnTile, [NewCoord.X, NewCoord.Y]);
                }
            }
        }
        if (tileDrop == TileData.TileDrop.None)
            return;

        if (GetNode<Timer>("../../DropCooldownTimer").TimeLeft == 0)
        AddDrop();
    }
    public void PlayDeathAnimation(Vector2 HitDirection)
    {
        Hud.AddScore(10);
        Collision.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);

        Tween tween = CreateTween();
        tween.Finished += OnTweenFinished; 
        tween.SetParallel();

        tween.TweenProperty(this, "modulate:a", 0, 0.2);
        tween.TweenProperty(this, "scale", new Vector2(0.6f, 0.6f), 0.2).SetEase(Tween.EaseType.In);
        tween.TweenProperty(this, "global_position", HitDirection * 10, 0.1).AsRelative().SetTrans(Tween.TransitionType.Quad);
    }
    public Vector2I GetCoordinates(String name)
    {
        String[] Name = name.ToString().Split("_");    
        Vector2I Coord = new Vector2I(Name[1].ToInt(), Name[2].ToInt());

        return Coord;
    }
    private Tile GetTile(Node Container, Vector2I Coords)
    {
        Tile tile =  Container.GetNodeOrNull<Tile>($"Tile_{Coords.X}_{Coords.Y}");

        return tile;
    }
    private bool HasTile(Node Container, Vector2I Coords)
    {
        bool HasTile = Container.HasNode($"Tile_{Coords.X}_{Coords.Y}");

        return HasTile;
    }
    private bool IsTileInRange(Vector2I Coords)
    {
        return Coords.X >= 0 && Coords.Y >= 0 && Coords.X <= Main.CurrentPreset.TotalTiles.X - 1 && Coords.Y <= Main.CurrentPreset.TotalTiles.Y - 1 ;  
    }
    private void AddDrop()
    {
        Node DropsContainer = GetTree().Root.GetNode("Main/PlayingArea/Drops");

        if (DropsContainer.GetChildCount() >= 3)
            return;

        BaseDrop Drop = DropScene.Instantiate<BaseDrop>();
        Drop.tileDrop = tileDrop;
        Drop.GlobalPosition = GlobalPosition;

        DropsContainer.CallDeferred(Node.MethodName.AddChild, Drop);
        GetNode<Timer>("../../DropCooldownTimer").Start();
    }
    private void SetTagAndDrop()
    {
        Random random = new Random();
        int TileTag = random.Next(1, 15);
        int TileDrop = random.Next(1, 20);

        switch (TileTag)
        {
            case 1:
            tileTag = TileData.TileTag.NewStriker;
            break;

            case 2:
            tileTag = TileData.TileTag.Explosion;
            break;

            case 3:
            tileTag = TileData.TileTag.CrossExplosion;
            break;

            case 4:
            tileTag = TileData.TileTag.Respawn;
            break;

            case 5:
            tileTag = TileData.TileTag.CrossRespawn;
            break;
        }
        switch (TileDrop)
        {
            case 1:
                tileDrop = TileData.TileDrop.Saver;
                break;
            case 2:
                tileDrop = TileData.TileDrop.Extender;
                break;
            case 3:
                tileDrop = TileData.TileDrop.Rigidify;
                break;
            case 4:
                tileDrop = TileData.TileDrop.Bullet;
                break;
        }
    }
    private void OnTweenFinished()
    {
        if (GetParent().GetChildCount() <= 1)
            EmitSignal(SignalName.AllTileDissapeared, true);
        Free();
    }
}

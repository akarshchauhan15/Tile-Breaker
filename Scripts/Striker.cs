using Godot;

public partial class Striker : CharacterBody2D
{
    [Signal]
    public delegate void OnAllStrikerDissappearedEventHandler(bool Win);

    int Speed = 750;
    public int BreakLeft;

    public Vector2 Direction = Vector2.Zero;
    public TileData.TileDrop CurrentPower = TileData.TileDrop.None;

    AudioStreamPlayer BounceEffect;
    Hud HUD;
    PackedScene ParticlesScene;
    public CpuParticles2D FireParticles;
    public Sprite2D Texture;

    public override void _Ready()
    {
        HUD = GetTree().Root.GetNode<Hud>("Main/HUD");
        OnAllStrikerDissappeared += HUD.EndGame;

        Main main = HUD.GetParent<Main>();
        OnAllStrikerDissappeared += main.AddMainStriker;

        GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D").ScreenExited += OnScreenExited;

        ParticlesScene = ResourceLoader.Load<PackedScene>("res://Scenes/particles.tscn");
        FireParticles = GetNode<CpuParticles2D>("FireParticles");

        BounceEffect = GetNode<AudioStreamPlayer>("Sound/Bounce");
        Texture = GetNode<Sprite2D>("Striker");
    }
    public override void _Process(double delta)
    {
        KinematicCollision2D Collision = MoveAndCollide(Speed * Direction * (float) delta);

        if (Collision != null)
        {
            Node2D Collider = (Node2D)Collision.GetCollider();
            Vector2 Normal = Collision.GetNormal();

            if (Collider is Player)
                Direction = (Direction.Bounce(Normal) + new Vector2(Mathf.Clamp((GlobalPosition - Collider.GlobalPosition).X, -30, 30) / 30, 0)).Normalized();
            else
            {
                if (Collider is Tile)
                {
                    ((Tile)Collider).OnHit();

                    if (CurrentPower == TileData.TileDrop.Rigidify)
                    {
                        if (BreakLeft <= 0)
                        {
                            CurrentPower = TileData.TileDrop.None;
                            Texture.Modulate = Colors.White;
                            FireParticles.Emitting = false;
                        }
                        BreakLeft -= 1;
                        return;
                    }
                }
                Direction = Direction.Bounce(Normal);
            }

            BounceEffect.Play();

            if (CurrentPower == TileData.TileDrop.Saver && Collider.Name == "Down")
            {
                CurrentPower = 0;
                Texture.Modulate = Colors.White;
                SetCollisionMaskValue(5, false);

                return;
            }
            EmitParticles(Normal, Collider);
        }
    }
    private void OnScreenExited()
    {
        if (GetParent().GetChildCount() <= 1)  
            EmitSignal(SignalName.OnAllStrikerDissappeared, false);
        QueueFree();
    }
    private void EmitParticles(Vector2 Normal, Node2D Tile) 
    {
        CpuParticles2D Particles = ParticlesScene.Instantiate<CpuParticles2D>();

        Gradient gradient = new Gradient();
        if (Tile is Tile)
        {
            gradient.AddPoint(0f, TileData.TileColors[((Tile)Tile).Row%7]);
            gradient.AddPoint(0.6f, TileData.TileColors[((Tile)Tile).Row%7].Lightened(0.2f));       
        }
        else
        {
            gradient.AddPoint(0f, Texture.Modulate);
            gradient.AddPoint(0.6f, Texture.Modulate.Lightened(0.2f));
        }
        gradient.AddPoint(1f, Colors.White);
        Particles.ColorRamp = gradient;

        Particles.GlobalPosition = GlobalPosition;
        Particles.Direction = Normal;

        HUD.GetParent().AddChild(Particles);
        Particles.Emitting = true;
    }
}

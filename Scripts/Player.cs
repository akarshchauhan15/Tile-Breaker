using Godot;

public partial class Player : CharacterBody2D
{
    public float Lenght;
    CollisionShape2D CollisionShape;
    Sprite2D Sprite;

    PackedScene BulletScene;
    Main main;

    public int BulletCount = 0;
    public static int Lives = 3;
    public static int Score = 0;
    public override void _Ready()
    {
        CollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        Sprite = GetNode<Sprite2D>("Player");
        Lenght = Sprite.Texture.GetSize().X * Sprite.Scale.X/2;

        BulletScene = ResourceLoader.Load<PackedScene>("res://Scenes/bullet.tscn");
        main = GetTree().Root.GetNode<Main>("Main");

        GetNode<Timer>("BulletTimer").Timeout += AddBullets;
    }
    public override void _Process(double delta)
    {
        if (Main.isPlaying){
       
            Vector2 Pos;

            Pos = new Vector2(GetGlobalMousePosition().X, 660);
            Pos =  new Vector2(Mathf.Clamp(Pos.X, Lenght, 1280 - Lenght), Pos.Y);

            GlobalPosition = Pos;
        }
    }
    public void TransformSize(bool IncreaseSize)
    {
        RectangleShape2D rectShape = new RectangleShape2D();
        if (IncreaseSize)
        {
            rectShape.Size = new Vector2(291, 15);
            CollisionShape.SetDeferred(CollisionShape2D.PropertyName.Shape, rectShape);
            Sprite.Scale = new Vector2(1.5f, 1);

            Timer timer = new Timer();
            timer.OneShot = true;
            timer.Timeout += () => TransformSize(false); ;

            GetNode<Node>("Timers").AddChild(timer);
            timer.Start(10);     
        }
        else 
        {
            rectShape.Size = new Vector2(194, 15);
            CollisionShape.SetDeferred(CollisionShape2D.PropertyName.Shape, rectShape);
            Sprite.Scale = Vector2.One;
        }
        Lenght = Sprite.Texture.GetSize().X * Sprite.Scale.X/2;
    }
    public void OnGameStarted()
    {
        foreach (Timer timer in GetNode<Node>("Timers").GetChildren())      
            timer.Free();

        RectangleShape2D rectShape = new RectangleShape2D();
        rectShape.Size = new Vector2(194, 15);
        CollisionShape.SetDeferred(CollisionShape2D.PropertyName.Shape, rectShape);
        Sprite.Scale = Vector2.One;
    }
    public void AddBullets()
    {
        int Count = 2;
        float Disp = 70;

        Hud HUD = main.GetNode<Hud>("HUD");

        while (Count > 0)
        {
            HUD.PlaySound("Bullet");
            Bullet Bullet = BulletScene.Instantiate<Bullet>();
            Bullet.SetDeferred(Node2D.PropertyName.GlobalPosition, GlobalPosition + new Vector2(Disp, -20));
            main.GetNode<Node>("PlayingArea/Drops").CallDeferred(Node.MethodName.AddChild, Bullet);

            Count--;
            Disp = -Disp;
        }
        
        BulletCount--;

        if (BulletCount > 0)
        GetNode<Timer>("BulletTimer").Start();
    }
}

using Godot;
using System;
public partial class BaseDrop : Area2D
{
    public TileData.TileDrop tileDrop;
    Node2D Play;
    Label DisplayText;

    int Speed;
    public override void _Ready()
    {
        Play = GetTree().Root.GetNode<Node2D>("Main/PlayingArea");
        DisplayText = GetNode<Label>("Texture/Label");
        Speed = new Random().Next(100, 120);

        VisibleOnScreenNotifier2D Visible = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        Visible.ScreenExited += QueueFree;

        BodyEntered += OnCollision;

        DisplayText.Text = TileData.TileDropDisplay[(int)tileDrop - 1];
    }
    public override void _Process(double delta)
    {
        Position += Vector2.Down * Speed * (float)delta;
    }
    private void OnCollision(Node2D body)
    {
        QueueFree();
        if (!(body is Player))
            return;

        Hud.AddScore(40);
        GetNode<Hud>("../../../HUD").PlaySound("DropPickup");

        Node StrikerContainer;
        StrikerContainer = Play.GetNode<Node>("Strikers");

        Player player;
        player = Play.GetNode<Player>("Player");

        switch (tileDrop)
        {
            case TileData.TileDrop.Saver:
                foreach (Striker striker in StrikerContainer.GetChildren())
                {
                    if (striker.CurrentPower != TileData.TileDrop.None)
                        continue;

                    striker.SetCollisionMaskValue(5, true);
                    striker.CurrentPower = TileData.TileDrop.Saver;

                    Tween tween = striker.CreateTween();
                    tween.TweenProperty(striker.Texture, "modulate", Colors.LimeGreen, 0.3);
                }
                break;

            case TileData.TileDrop.Rigidify:  
                foreach (Striker striker in StrikerContainer.GetChildren())
                {
                    if (striker.CurrentPower != TileData.TileDrop.None)
                        continue;

                    striker.CurrentPower = TileData.TileDrop.Rigidify;
                    striker.BreakLeft = new Random().Next(3, 6);

                    Tween tween = striker.CreateTween();
                    tween.TweenProperty(striker.Texture, "modulate", Colors.Orange, 0.1);
                    striker.FireParticles.Emitting = true;
                }
                break;

            case TileData.TileDrop.Extender:
                Sprite2D Sprite = player.GetNode<Sprite2D>("Player");
                if (Sprite.Scale != Vector2.One)
                    break;

                player.TransformSize(true);
                break;

            case TileData.TileDrop.Bullet:
                if (player.GetNode<Timer>("BulletTimer").TimeLeft != 0)
                    break;

                player.SetDeferred(Player.PropertyName.BulletCount, new Random().Next(3, 6));
                player.CallDeferred(Player.MethodName.AddBullets);
                break;
        }
    }
}
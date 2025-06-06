using Godot;

[GlobalClass]
public partial class Slide : Control
{
    [Signal]
    public delegate void MotionCompletedEventHandler(bool Hidden);
    enum Direction {Left, Right, Up, Down};

    [ExportGroup("Properties")]
    [Export]
    Direction Slide_Direction = Direction.Left;
    [Export]
    float Slide_Time = 0.3f;

    [ExportSubgroup("Delay")]
    [Export]
    float Appear_Delay = 0;
    [Export]
    float Hide_Delay = 0;

    public Vector2 HiddenPos;
    public Vector2 OpenPos;
    Vector2 CaseSize;

    float Progression;

    Tween tween;
    public override void _Ready()
    {
        MouseEntered += OnMouseEntered;
        GetNode<ColorRect>("ColorRect").MouseExited += OnMouseExited;

        CaseSize = GetNode<ColorRect>("ColorRect").Size;

        foreach (Control child in GetChildren(true)) 
            child.MouseFilter = MouseFilterEnum.Pass;
       
        HiddenPos = GlobalPosition;
        GetOpenPos();
    }
    public void GetOpenPos() 
    {
        switch (Slide_Direction)
        {
            case (Direction.Left):
                OpenPos = new Vector2(GlobalPosition.X - CaseSize.X, GlobalPosition.Y);
                Progression = CaseSize.X;
                break;
            case (Direction.Right):
                OpenPos = new Vector2(GlobalPosition.X + CaseSize.X, GlobalPosition.Y);
                Progression = CaseSize.X;
                break;
            case (Direction.Up):
                OpenPos = new Vector2(GlobalPosition.X, GlobalPosition.Y - CaseSize.Y);
                Progression = CaseSize.Y;
                break;
            case (Direction.Down):
                OpenPos = new Vector2(GlobalPosition.X, GlobalPosition.Y + CaseSize.Y);
                Progression = CaseSize.Y;
                break;
        }
    }
    public void OnMouseEntered()
    {
        Kill();
        tween = CreateTween();
        tween.Finished += EmitSignal;
        tween.TweenProperty(this, "global_position", OpenPos, Mathf.Lerp(0, Slide_Time, Mathf.Abs((GlobalPosition - OpenPos).Length()) / Progression)).SetTrans(Tween.TransitionType.Sine).SetDelay(Appear_Delay); ;
    }
    public void OnMouseExited()
    {
        Kill();
        tween = CreateTween();
        tween.Finished += EmitSignal;

        float Del;
        if (GlobalPosition == OpenPos) Del = Hide_Delay;
        else Del = 0;
        tween.TweenProperty(this, "global_position", HiddenPos, Mathf.Lerp(0, Slide_Time, Mathf.Abs((GlobalPosition - HiddenPos).Length()) / Progression)).SetTrans(Tween.TransitionType.Sine).SetDelay(Del);
    }
    public void Kill() 
    {
        if (tween != null)      
            tween.Kill(); 
    }
    public void EmitSignal()
    {
        bool Exited = true;
        if (GlobalPosition != HiddenPos)
            Exited = false;

        EmitSignal(SignalName.MotionCompleted, Exited);
    }
}

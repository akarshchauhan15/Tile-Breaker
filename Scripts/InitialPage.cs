using Godot;

public partial class InitialPage : Control
{
    Button NextButton;
    Button PreviousButton;
    Label Counter;
    int CurrentPage = 1;

    public override void _Ready()
    {
        NextButton = GetNode<Button>("NextButton");
        PreviousButton = GetNode<Button>("PreviousButton");
        Counter = GetNode<Label>("Counter");

        NextButton.Pressed += () => ChangePage(true);
        PreviousButton.Pressed += () => ChangePage(false);
    }
    private void ChangePage(bool MoveForward)
    {
        GetNode<Control>("Pages/" + CurrentPage.ToString()).Hide();

        CurrentPage += MoveForward ? 1 : -1;
        CurrentPage = Mathf.Clamp(CurrentPage, 1, 4);

        if (CurrentPage == 4)
        {
            Destroy();
            return;
        }
        GetNode<Control>("Pages/" + CurrentPage.ToString()).Show();

        Counter.Text = CurrentPage.ToString() + "/3";
        PreviousButton.Visible = CurrentPage != 1;
    }
    private void Destroy()
    {
        PreviousButton.MouseFilter = MouseFilterEnum.Ignore;
        NextButton.MouseFilter = MouseFilterEnum.Ignore;

        Tween tween = CreateTween();
        tween.Finished += () => QueueFree();

        tween.TweenProperty(this, "modulate:a", 0f, 0.4).SetTrans(Tween.TransitionType.Quad);
    }
}

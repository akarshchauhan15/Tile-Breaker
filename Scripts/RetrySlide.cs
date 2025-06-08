using Godot;
using System;

public partial class RetrySlide : Slide
{
    Button RetryButton;

    bool PressedOnce = false;
    string[] Texts = ["Retry?", "Sure?", "Done!"];

    public override void _Ready()
    {
        base._Ready();

        RetryButton = GetNode<Button>("ColorRect/Button");
        RetryButton.Pressed += RetryButtonPressed;

        MotionCompleted += ResetButton;
    }

    private void RetryButtonPressed()
    {
        if (!PressedOnce)
        {
            RetryButton.Text = Texts[1];
            PressedOnce = true;

            return;
        }

        RetryButton.Text = Texts[2];
        RetryButton.Disabled = true;

        Player.Lives = 0;
        GetNode<Hud>("../../").EndGame(false);
    }
    private void ResetButton(bool Hidded)
    {
        if (Hidded)
        {
            PressedOnce = false;
            RetryButton.Text = Texts[0];
            RetryButton.Disabled = true;

            return;
        }

        RetryButton.Disabled = false;
    }
}

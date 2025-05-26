using Godot;

public partial class Particles : CpuParticles2D
{
    public override void _Ready()
    {
        Finished += QueueFree;
    }
}

using Godot;

public partial class StateManager: Node
{
    private IState state;

    public override void _PhysicsProcess(double delta)
    {
        state?.PhysicsProcess(delta);
    }

    public override void _Process(double delta)
    {
        state?.Process(delta);

        ProcessNextState();
    }

    public void ChangeState(IState state)
    {
        this.state = state;
        state.Start();
    }

    private void ProcessNextState()
    {
        var nextState = state?.NextState();

        if (nextState is not null)
        {
            state = nextState;
            state.Start();
        }
    }
}
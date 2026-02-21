public interface IState
{
    void Start();
    void Process(double delta);
    void PhysicsProcess(double delta);

    IState NextState();
}
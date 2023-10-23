namespace AI
{
    public abstract class State<T>
    {
        public abstract void Enter(T aiController);
        public abstract void Execute(T aiController, double delta);
        public abstract void Exit(T aiController);
    }
}

using Godot;

namespace AI
{
    public class StateController<T>
    {
        public T Owner { get; set; }
        public State<T> CurrentState { get; set; }

        //a record of the last state the agent was in
        public State<T> PreviousState { get; set; }

        //this is called every time the FSM is updated
        public State<T> GlobalState { get; set; }

        public StateController(T owner)
        {
            Owner = owner;
            CurrentState = null;
            PreviousState = null;
            GlobalState = null;
        }
        public void SetCurrentState(State<T> state) { CurrentState = state; }
        public void SetGlobalState(State<T> state) { GlobalState = state; }
        public void SetPreviousState(State<T> state) { PreviousState = state; }

        public void Update(double delta)
        {
            //if a global state exists, call its execute method, else do nothing
            //if (GlobalState != null) { GlobalState.Execute(Owner); }
            if (CurrentState != null) { CurrentState.Execute(Owner, delta); }
        }
        public void ChangeState(State<T> newState)
        {
            PreviousState = CurrentState;
            CurrentState.Exit(Owner);
            CurrentState = newState;

            CurrentState.Enter(Owner);
        }
        public void RevertToPreviousState()
        {
            if (PreviousState == null)
            {
                GD.Print("Error: Cannot revert to previous state. PreviousState is null");
                return;
            }
            ChangeState(PreviousState);
        }
        public bool IsInState(State<T> st)
        {
            if (st.GetType() == CurrentState.GetType())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

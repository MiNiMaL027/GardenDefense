using AI;
using Controllers;
using Godot;
using Pawns;
using Pawns.BattlePlants;
using Pawns.Monsters;
using System;

public partial class TestMonsterAIController : AIController
{
    public StateController<AIController> StateMachine { get; set; }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        AttackRangeSquared = AttackRange*AttackRange;
        EnemyType = typeof(BaseBattlePlant);
        AreaLineOfSight = GetNode<Area3D>("TestMonster/AreaLineOfSight");
        Pawn = GetNode<Pawn>("TestMonster");
        Pawn.Died += deathListener;
        Pawn.Controller = this;
        AreaLineOfSight.BodyEntered += AreaLineOfSight_BodyEntered;
        AreaLineOfSight.BodyExited += AreaLineOfSight_BodyExited;
        StateMachine = new StateController<AIController>(this);
        StateMachine.CurrentState = new DefaultMonsterRun();
        StateMachine.CurrentState.Enter(this);
    }

    

    private void deathListener()
    {
        QueueFree();
    }


    public override void UpdateAI(double delta)
    {
        StateMachine.Update(delta);
    }

    public override void ChangeState(State<AIController> newState)
    {
        StateMachine.ChangeState(newState);
    }
}

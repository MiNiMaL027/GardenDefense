using Farm.Scripts.Components;
using Farm.Scripts.Enums;
using Godot;

namespace Farm.Scripts.BattlePlants
{
    public abstract partial class BaseBattlePlant : Pawn
    {
        public BattlePlantClass Class { get; set; }    
        public AttackType AttackType { get; set; }
        public int Lvl { get; set; } = 1;
        public int TimeToGrow { get; set; }
        public Timer AttackTimer { get; set; }
      
        //public AudioStreamPlayer3D AudioStream { get; set; }    

        public override void _Ready()
        {
            init();

            //AudioStream = GetNode<AudioStreamPlayer3D>(" ");

            AttackTimer = new Timer();
            AddChild(AttackTimer);
            AttackTimer.OneShot = false;
            AttackTimer.Autostart = false;
            AttackTimer.WaitTime = AttackSpeed;
            AttackTimer.Timeout += Attack;
        }

        public abstract void Attack();

        public void StopAttack()
        {
            AttackTimer.Stop();
        }

        public void StartAttack()
        {
            AttackTimer.Start();
        }
    }
}

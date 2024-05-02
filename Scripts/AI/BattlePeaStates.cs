//using Godot;
//using Controllers;
//using Pawns.BattlePlants;

//namespace AI
//{
//    public partial class BattlePeaIdle : State<AIController>
//    {
//        public override void Enter(AIController aiController)
//        {
//        }

//        public override void Execute(AIController aiController, double delta)
//        {
//            if (aiController.CanDealDamageToEnemy())
//            {
//                aiController.ChangeState(new BattlePeaAttack());
//            }
//        }

//        public override void Exit(AIController aiController)
//        {
//        }
//    }
//    public partial class BattlePeaAttack : State<AIController>
//    {
//        public override void Enter(AIController aiController)
//        {
//        }
//        public override void Execute(AIController aiController, double delta)
//        {
//            if (aiController.CanAttack == true)
//            {
//                if (aiController.CanDealDamageToEnemy() == false)
//                {
//                    aiController.ChangeState(new BattlePeaIdle());
//                }
//                else
//                {
//                    aiController.Pawn.IsAttacking = true;
//                    aiController.CanAttack = false;
//                }
//            }
//        }
//        public override void Exit(AIController aiController)
//        {
//        }
//    }
//}

//using Controllers;
//using Godot;
//using Pawns.Monsters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AI
//{
//    public partial class BattleCarrotIdle : State<AIController>
//    {
//        public override void Enter(AIController aiController)
//        {
//        }

//        public override void Execute(AIController aiController, double delta)
//        {
//            if (aiController.CanDealDamageToEnemy())
//            {
//                aiController.ChangeState(new BattleCarrotAttack());
//            }
//        }

//        public override void Exit(AIController aiController)
//        {
//        }
//    }
//    public partial class BattleCarrotAttack : State<AIController>
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
//                    aiController.ChangeState(new BattleCarrotIdle());
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

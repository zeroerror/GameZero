using GamePlay.Core;
using System;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateTransition_Any2Dead : GameStateTransition
    {
        public override string fromState => "Any";
        public override string toState => "Dead";

        public override bool Condition()
        {
            return false;
        }

    }
}

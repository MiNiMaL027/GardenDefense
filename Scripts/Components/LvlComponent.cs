using Godot;
using System;

namespace Components
{
    public partial class LvlComponent : Node
    {
        int currentLvl = 1;
        public int CurrentLvl {
            get { return currentLvl; }
            set {
                EmitSignal(SignalName.LvlUp, value);
                LvlUpMethod.Invoke();
                PointsToNextLvl *= 2;
                currentLvl = value;
            }
        }
        int currentPoints;
        public int CurrentPoints
        {
            get { return currentPoints; }
            set {   
                
                if (value >= PointsToNextLvl)
                {
                    CurrentLvl++;
                    value -= PointsToNextLvl;
                }

                currentPoints = Math.Max(0, value);
            }
        }
        [Export]
        public int PointsToNextLvl { get; set; } = 1;

        public Action LvlUpMethod { get; set; }

        [Signal]
        public delegate void LvlUpEventHandler(int currentLvl);

        public void AddPoints(int pointCount = 1)
        {
            CurrentPoints += pointCount;
        }
    }
}

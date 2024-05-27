using Enums;
using Godot;
using System;
namespace Widgets.BattleAgent
{
    public partial class LvlButton : Button
    {
        [Export]
        public int LvlNumber { get; set; }
        [Export]
        public int SlotsCount { get; set; }

        private LvlButtonState lvlButtonState;
        public LvlButtonState LvlButtonState {
            get { return lvlButtonState; }
            set
            {
                switch (value)
                {
                    case LvlButtonState.Active:
                        Activate();
                        break;
                    case LvlButtonState.Completed:
                        Completed();
                        break;
                    case LvlButtonState.Disabled:
                        Disable();
                        break;
                }

                lvlButtonState = value;
            }
        }
      
        public override void _Ready()
        {
            Text = LvlNumber.ToString();
            Pressed += LvlButton_Pressed;
        }

        private void LvlButton_Pressed()
        {
            if (GetParent().HasNode("PlantTransferWindow"))
            {
                PlantTransferWindow window = GetParent().GetNode<PlantTransferWindow>("PlantTransferWindow");
                if (window != null)
                {
                    GetParent().RemoveChild(window);
                    window.QueueFree();
                }
            }

            var transferWindow = Scenes.Widgets.PlantTransfer.PlantTransferWindow();
            GetParent().AddChild(transferWindow);
            transferWindow.Init(LvlNumber, SlotsCount);         
        }

        public void Init(LvlButtonState state)
        {
            LvlButtonState = state;          
        }

        public void Activate()
        {
            Disabled = false;

            Icon = null;
        }

        public void Completed()
        {
            Disabled = false;

            Icon = ResourceLoader.Load<Texture2D>("res://raw assets/Images/10.png");
        }

        public void Disable()
        {
            Disabled = true;

            Icon = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Lock.png");
            Text = "";
        }
    }
}


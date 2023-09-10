using Godot;
namespace Farm.Scripts.Widgets.ToolTip
{
    public partial class BaseTooltip : Control
    {
        public Timer viewTimer;
        public int timeToView = 2;

        public override void _Ready()
        {
            viewTimer = new Timer();
            viewTimer.OneShot = true;
            viewTimer.Autostart = false;
            viewTimer.WaitTime = timeToView;

            AddChild(viewTimer);

            viewTimer.Timeout += ViewTimer_Timeout;
        }

        protected virtual void ViewTimer_Timeout()
        {
            Visible = true;
        }

        public void PostInit()
        {
            Visible = false;

            viewTimer.Start(0);
        }

        public void HideTooltip()
        {
            viewTimer.Stop();
            QueueFree();
        }
    }
}

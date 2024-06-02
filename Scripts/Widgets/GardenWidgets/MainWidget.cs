using Controllers;
using Godot;
using Widgets.Global;

namespace Widgets.GardenWidgets
{
    public partial class MainWidget : Control
    {
        public InfoWindow InfoWindow { get; set; }
        
        public override void _Ready()
        {
            InfoWindow = GetNode<InfoWindow>("InfoWindow");
        }
        public virtual void OpenInventory() { }

        public virtual void ToggleInventory() { }
        public virtual void Init(PlayerController playerController)
        {
        }
        public virtual void CloseInventory() { }
    }
}

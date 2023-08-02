using System;
using Godot;

namespace Widgets.ContextMenu
{
    public partial class ItemContextMenu:Control
    {
        protected VBoxContainer container;
        Item targetItem;
        public override void _Ready()
        {
            container = GetNode<VBoxContainer>("VBoxContainer");
        }
        public virtual void Init(Item item, bool isInInventory)
        {
            targetItem= item;

            Button Details = new Button();
            Details.Name = "Details";
            Details.Text = "Details";
            Details.Pressed += Details_Pressed;
            container.AddChild(Details);

            if(isInInventory == false)
            {
                Button MoveToInventory = new Button();
                MoveToInventory.Name = "Move to inventory";
                MoveToInventory.Text = "Move to inventory";
                MoveToInventory.Pressed += MoveToInventory_Pressed;
                container.AddChild(MoveToInventory);
            }

            Button Sell = new Button();
            Sell.Name = "Sell";
            Sell.Text = "Sell";
            Sell.Pressed += Sell_Pressed;
            container.AddChild(Sell);

            Button Delete = new Button();
            Delete.Name = "Delete";
            Delete.Text = "Delete";

            Delete.Pressed += Delete_Pressed;
            container.AddChild(Delete);
        }

        public virtual void Delete_Pressed()
        {
            targetItem.QueueFree();
            GD.Print("Delete");
            QueueFree();
        }

        public virtual void Sell_Pressed()
        {
            GD.Print("Sell");
            QueueFree();
        }

        public virtual void MoveToInventory_Pressed()
        {
            GD.Print("Movetoinventory");
            QueueFree();
        }

        public virtual void Details_Pressed()
        {
            GD.Print("Details");
            QueueFree();
        }
    }
}

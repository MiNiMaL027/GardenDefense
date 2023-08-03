using System;
using Godot;

namespace Widgets.ContextMenu
{
    public partial class ItemContextMenu:Control
    {
        protected HBoxContainer container;
        Item targetItem;
        public override void _Ready()
        {
            container = GetNode<HBoxContainer>("VBoxContainer");
        }
        public virtual void Init(Item item, bool isInInventory)
        {
            targetItem = item;

            TextureButton Details = new TextureButton();
            Details.Name = "Details";
            Details.TextureNormal = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/Detail.png");
            Details.Pressed += Details_Pressed;
            container.AddChild(Details);

            if(isInInventory == false)
            {
                TextureButton MoveToInventory = new TextureButton();
                MoveToInventory.Name = "Move to inventory";
                MoveToInventory.TextureNormal = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/MoveToBag.png");
                MoveToInventory.Pressed += MoveToInventory_Pressed;
                container.AddChild(MoveToInventory);
            }

            TextureButton Sell = new TextureButton();
            Sell.Name = "Sell";
            Sell.TextureNormal = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/Sell.png");
            Sell.Pressed += Sell_Pressed;
            container.AddChild(Sell);

            TextureButton Delete = new TextureButton();
            Delete.Name = "Delete";
            Delete.TextureNormal = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/Deletel.png");

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

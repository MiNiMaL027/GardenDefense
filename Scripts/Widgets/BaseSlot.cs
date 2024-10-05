using Godot;
using Items;
using System.Collections.Generic;
using Widgets.Inventory;
using Widgets.ToolTip;

namespace Widgets
{
    public abstract partial class BaseSlot : Control
    {
        public ItemTooltip itemTooltip;

        public ItemDatabaseRow ItemDatabaseRow
        {
            get
            {
                return itemDatabaseRow;
            }
            set
            {

                itemDatabaseRow = value;
                if (value != null)
                {
                    TextureRect.Texture = ResourceLoader.Load<Texture2D>(value.TextureSpritePath);
                }
                else
                {
                    TextureRect.Texture = null;
                }
            }
        }
        private ItemDatabaseRow itemDatabaseRow;
        public TextureRect TextureRect { get; set; }
        public Label LabelAmount { get; set; }

        TextureRect BackgroundTextureRect;

        Tween tween;

        public bool CanBeEmpty = false;
        public bool IsEmpty
        {
            get
            {
                return ItemDatabaseRow == null;
            }
        }

        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;

                if (amount > 1) //display count of items
                {
                    LabelAmount.Text = amount.ToString();
                }
                else if (amount == 1) //text shouldn't be displayed
                {
                    LabelAmount.Text = "";
                }
                else //remove from screen if amount 0
                {
                    if(!CanBeEmpty)
                        QueueFree();
                    else
                        Empty();
                }
            }
        }

        protected int amount;

        private void InventorySlot_Exit()
        {
            tween.Stop();
            TextureRect.Scale = new Vector2(1f, 1f);
            TextureRect.PivotOffset = Vector2.Zero;
            BackgroundTextureRect.SelfModulate = new Color(1, 1, 1);
        }

        private void InventorySlot_Enter()
        {
            tween = CreateTween();
            tween.SetTrans(Tween.TransitionType.Quart);
            tween.SetEase(Tween.EaseType.Out);
            tween.Parallel().TweenProperty(TextureRect, "scale", new Vector2(1.1f, 1.1f), 0.1);
            tween.Parallel().TweenProperty(TextureRect, "pivot_offset", new Vector2(32, 32), 0.1);

            BackgroundTextureRect.SelfModulate = new Color(1, 1, 0.643f);
        }

        public virtual void Empty()
        {
            Amount = 0;
            ItemDatabaseRow = null;
        }
        public void InventorySlot_MouseExited()
        {
            InventorySlot_Exit();

            if (itemDatabaseRow == null)
                return;

            if (itemTooltip != null)
            {
                itemTooltip.HideTooltip();

                itemTooltip = null;
            }
        }
        public void InventorySlot_MouseEntered()
        {
            InventorySlot_Enter();
            if (itemDatabaseRow == null)
                return;

            itemTooltip = Item.GetTooltipSceneByType(ItemDatabaseRow.ItemType);
            Vector2 globalMousePosition = GetViewport().GetMousePosition();

            AddChild(itemTooltip);

            itemTooltip.TopLevel = true;

            itemTooltip.ShowTooltipDbRow(ItemDatabaseRow);
            itemTooltip.AdjustControlInViewport(globalMousePosition);
            itemTooltip.PostInit();
        }
        public override void _Ready()
        {
            MouseEntered += InventorySlot_MouseEntered;
            MouseExited += InventorySlot_MouseExited;

            BackgroundTextureRect = GetNode<TextureRect>("Background");
        }
        public virtual void Init(ItemDatabaseRow item, int amountToSet)
        {
            ItemDatabaseRow = item;
            Amount = amountToSet;
        }
        public class Comparers
        {
            public class DefaultAsc : IComparer<BaseSlot>
            {
                public int Compare(BaseSlot x, BaseSlot y)
                {
                    return 0;
                }
            }
            public class DefaultDesc : IComparer<BaseSlot>
            {
                public int Compare(BaseSlot x, BaseSlot y)
                {
                    return 0;
                }
            }
            public class AmountAsc : IComparer<BaseSlot>
            {
                public int Compare(BaseSlot x, BaseSlot y)
                {
                    if (x.Amount > y.Amount)
                    {
                        return 1;
                    }
                    if (x.Amount < y.Amount)
                    {
                        return -1;
                    }

                    return 0;
                }
            }
            public class AmountDesc : IComparer<BaseSlot>
            {
                public int Compare(BaseSlot x, BaseSlot y)
                {
                    if (x.Amount > y.Amount)
                    {
                        return -1;
                    }
                    if (x.Amount < y.Amount)
                    {
                        return 1;
                    }

                    return 0;
                }
            }
            public class PriceAsc : IComparer<BaseSlot>
            {
                public int Compare(BaseSlot x, BaseSlot y)
                {
                    if(x.IsEmpty && y.IsEmpty) { return 0; }
                    if (x.IsEmpty)
                    {
                        return -1;
                    }
                    if (y.IsEmpty)
                    {
                        return 1;
                    }
                    if (x.ItemDatabaseRow.SellPrice > y.ItemDatabaseRow.SellPrice)
                    {
                        return 1;
                    }
                    if (x.ItemDatabaseRow.SellPrice < y.ItemDatabaseRow.SellPrice)
                    {
                        return -1;
                    }

                    return 0;
                }
            }
            public class PriceDesc : IComparer<BaseSlot>
            {
                public int Compare(BaseSlot x, BaseSlot y)
                {
                    if (x.IsEmpty && y.IsEmpty) { return 0; }
                    if (x.IsEmpty)
                    {
                        return -1;
                    }
                    if (y.IsEmpty)
                    {
                        return 1;
                    }
                    if (x.ItemDatabaseRow.SellPrice > y.ItemDatabaseRow.SellPrice)
                    {
                        return -1;
                    }
                    if (x.ItemDatabaseRow.SellPrice < y.ItemDatabaseRow.SellPrice)
                    {
                        return 1;
                    }

                    return 0;
                }
            }
            public class TypeAsc : IComparer<BaseSlot>
            {
                public int Compare(BaseSlot x, BaseSlot y)
                {
                    if (x.IsEmpty && y.IsEmpty) { return 0; }
                    if (x.IsEmpty)
                    {
                        return -1;
                    }
                    if (y.IsEmpty)
                    {
                        return 1;
                    }
                    if (x.ItemDatabaseRow.ItemType > y.ItemDatabaseRow.ItemType)
                    {
                        return 1;
                    }
                    if (x.ItemDatabaseRow.ItemType < y.ItemDatabaseRow.ItemType)
                    {
                        return -1;
                    }

                    return 0;
                }
            }
            public class TypeDesc : IComparer<BaseSlot>
            {
                public int Compare(BaseSlot x, BaseSlot y)
                {
                    if (x.IsEmpty && y.IsEmpty) { return 0; }
                    if (x.IsEmpty)
                    {
                        return 1;
                    }
                    if (y.IsEmpty)
                    {
                        return -1;
                    }
                    if (x.ItemDatabaseRow.ItemType < y.ItemDatabaseRow.ItemType)
                    {
                        return 1;
                    }
                    if (x.ItemDatabaseRow.ItemType > y.ItemDatabaseRow.ItemType)
                    {
                        return -1;
                    }

                    return 0;
                }
            }           
        }
    }
}

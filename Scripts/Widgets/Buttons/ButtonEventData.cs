using Enums;

namespace Widgets.Buttons
{
    public class ButtonEventData
    {
        public ItemType ItemType { get; }
        public OrderType OrderType { get; }

        public ButtonEventData(ItemType itemType)
        {
            ItemType = itemType;
        }

        public ButtonEventData(OrderType ordertype)
        {
            OrderType = ordertype;
        }
    }
}

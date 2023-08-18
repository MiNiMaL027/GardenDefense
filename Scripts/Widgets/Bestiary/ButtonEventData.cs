using Enums;

public class ButtonEventData
{
    public ItemType ItemType { get; }

    public ButtonEventData(ItemType itemType)
    {
        ItemType = itemType;
    }
}
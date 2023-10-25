using Godot;
using System;
using System.Collections.Generic;

public partial class LaboratoryWindow : Control
{
	private GridContainer SlotContainer { get; set; }
	public List<LaboratorySlot> Slots = new List<LaboratorySlot>();

	private TextureButton CloseButton { get; set; }

	public override void _Ready()
	{
		SlotContainer = GetNode<GridContainer>("MarginContainer/Panel/MarginContainer/VBoxContainer/VScrollBar/GridContainer");
		CloseButton = GetNode<TextureButton>("TextureButton");

        CloseButton.Pressed += CloseButton_Pressed;

		InitSlots();
	}

    private void CloseButton_Pressed()
    {
        this.GetPlayerController().Hud.CloseLaboratory();
    }

    private void InitSlots()
	{
		for (int i = 0; i < SlotContainer.GetChildCount(); i++)
		{
			var slot = SlotContainer.GetChild<LaboratorySlot>(i);
            Slots.Add(slot);
			slot.BuyButton.Pressed += VisualRefresh;
		}
	}

	public void VisualRefresh()
	{
		for(int i = 0;i < Slots.Count; i++)
		{
			if (Slots[i].IsActive)
				Slots[i].ButtonInit();
		}
	}
}

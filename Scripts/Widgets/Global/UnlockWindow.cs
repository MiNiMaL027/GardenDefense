using Godot;
using System;

public partial class UnlockWindow : Control
{
	AnimationPlayer Animation;
	TextureRect textureRect;
	Label nameItemLabel;
	public override void _Ready()
	{
		Animation = GetNode<AnimationPlayer>("Panel/AnimationPlayer");
		textureRect = GetNode<TextureRect>("Panel/UnlockedItemTexture");
		nameItemLabel = GetNode<Label>("Panel/Label");
	}

	public void Init(int unlockItemId)
	{
		var itemData = DbService.GetItemDataById(unlockItemId);

		textureRect.Texture = itemData.texture;
		nameItemLabel.Text = itemData.itemName;

		Animation.Play("UnLock");
	}

	private void Delete()
	{
		QueueFree();
	}
}

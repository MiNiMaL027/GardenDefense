using Controllers;
using Godot;
using Pawns.BattlePlants;

namespace Widgets.ContextMenu
{
    public partial class BattlePlantContextMenu : ItemContextMenu
    {
        BaseBattlePlant menuOwner;
        public void Init(BaseBattlePlant plant, PlayerController playerController)
        {
            menuOwner = plant;

            this.playerController = playerController;

            AddButton(Scenes.Widgets.ContextMenu.TextureButtonTimeShader(), "Delete", "res://raw assets/Images/ToolsButton/Deletel.png", Delete_ButtonDown, Delete_ButtonUp);

            AddButton(initContentInButton(), "Upgrade", "res://raw assets/Images/ItemSprites/mut.png", UpgradeButtonPressed);
        }

        public override void Delete_Pressed_Confirm_Timeout()
        {
            Container.GetNode<TextureButtonTimeShader>("Delete").Material = null;

            menuOwner.QueueFree();

            playerController.RemoveOpenedContextMenu();
            timerConfirm.Timeout -= Delete_Pressed_Confirm_Timeout;
        }    
        
        public void UpgradeButtonPressed()
        {
            if(playerController.Mutagen < menuOwner.SkillRequiredMutagen)
            {
                //TODO Show message
                return;
            }

            menuOwner.MouseLeave();
            playerController.Mutagen -= menuOwner.SkillRequiredMutagen;
            menuOwner.SkillRequiredMutagen = Mathf.CeilToInt(menuOwner.SkillRequiredMutagen * 1.5f);
            GameInstance.Hud.BattlefieldWidget.OpenChooseSkillWidget(menuOwner);

            playerController.RemoveOpenedContextMenu();
        }

        protected TextureButton initContentInButton()
        {
            var button = new TextureButton();
            var hbox = new HBoxContainer();

            var label = new Label();
            label.LabelSettings = new LabelSettings() { 
                FontSize = 16,
                OutlineColor = new Color(0, 0, 0),
                OutlineSize = 6,
                FontColor = playerController.Mutagen >= menuOwner.SkillRequiredMutagen ? new Color(0, 0.621f, 0.301f) : new Color(1, 0, 0.157f)
            };
            label.Text = $"{playerController.Mutagen}/{menuOwner.SkillRequiredMutagen}";
            hbox.AddChild(label);
            button.AddChild(hbox);

            return button;
        }
    }
}

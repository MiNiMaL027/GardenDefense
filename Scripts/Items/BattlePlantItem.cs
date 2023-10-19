using Controllers;
using Godot;
using Interfaces;
using Items;

namespace Farm.Scripts.Items
{
    public partial class BattlePlantItem : Item, IPressable
    {
        public string BattlePlantScenePath { get; set; }
        public int Maxlvl { get; set; }
        public int BuyCropId { get; set; }

        public override void TryInteract(InputEventMouseButton eventMouseButton, PlayerController playerController)
        {
            Vector2 mousePosition = eventMouseButton.GlobalPosition;
            PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
            Camera3D camera = GetViewport().GetCamera3D();
            Vector3 from = camera.ProjectRayOrigin(mousePosition);
            Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 1000;
            PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(from, to);
            query.CollideWithAreas = true;
            query.CollideWithBodies = false;
            var result = spaceState.IntersectRay(query);

            if (result.Count > 0)
            {
                CollisionObject3D resultBody = result["collider"].AsGodotObject() as CollisionObject3D;
                if (resultBody is TowerDefenseAreaCell cell && cell.CanPlant() == true) //detected cell
                {
                    this.Reparent(cell);
                    this.Position = Vector3.Zero;
                    Freeze = true;
                }

            }
        }

        public override void InitializeItem(int itemId)
        {
            BattlePlantDataBaseRow i = DbService.GetItem(itemId) as BattlePlantDataBaseRow;

            InitializeItem(i);
        }

        public override void InitializeItem(Item i)
        {
            BattlePlantItem itemToCopy = i as BattlePlantItem;

            if (itemToCopy == null) { return; }

            editorItemId = itemToCopy.EditorItemId;
            ItemName = itemToCopy.ItemName;
            BuyPrice = itemToCopy.BuyPrice;
            SellPrice = itemToCopy.SellPrice;
            Description = itemToCopy.Description;
            ItemType = itemToCopy.ItemType;
            MeshPath = itemToCopy.MeshPath;
            TextureSpritePath = itemToCopy.TextureSpritePath;
            BattlePlantScenePath = itemToCopy.BattlePlantScenePath;
            Maxlvl = itemToCopy.Maxlvl;
            BuyCropId = itemToCopy.BuyCropId;

            this.InitVisual(itemToCopy);

            PostInit();
        }

        public override void InitializeItem(ItemDatabaseRow dbRow)
        {
            BattlePlantDataBaseRow i = dbRow as BattlePlantDataBaseRow;

            if(i.Id == 0) { return; }

            editorItemId = i.Id;
            ItemName = i.ItemName;
            BuyPrice = i.BuyPrice;
            SellPrice = i.SellPrice;
            Description = i.Description;
            ItemType = i.ItemType;
            MeshPath = i.MeshPath;
            TextureSpritePath = i.TextureSpritePath;
            BattlePlantScenePath = i.BattlePlantScenePath;
            Maxlvl = i.Maxlvl;
            BuyCropId = i.BuyCropId;
            PackedScene meshScene = ResourceLoader.Load<PackedScene>(MeshPath);

            this.InitVisual(meshScene);

            PostInit();
        }
        public override void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
        {
            base.LeftMouseDownListener(eventMouseButton, playerController);
            GetTree().NotifyGroup(Groups.TowerDefenseArea, Notifications.TowerDefenseArea.ITEM_BATTLEPLANT_CAPTURED);
        }
        public override void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
        {
            base.LeftMouseUpListener(eventMouseButton, playerController);
            GetTree().NotifyGroup(Groups.TowerDefenseArea, Notifications.TowerDefenseArea.ITEM_BATTLEPLANT_RELEASED);

        }
    }
}

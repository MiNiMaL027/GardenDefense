using System;
using Enums;
using Farm.Scripts.Items;
using Godot;
using Items;
using Microsoft.Data.Sqlite;

public static class DbService
{
    public static ItemDatabaseRow GetItem(int id)
    {
        using (SqliteConnection con = new SqliteConnection("Data Source = Db.db"))
        {
            con.Open();

            SqliteCommand sqliteCommand = con.CreateCommand();
            sqliteCommand.CommandText = $"SELECT * FROM Items WHERE Id = {id}";

            using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    ItemType itemType = (ItemType)Convert.ToInt32(reader["ItemType"]);

                    switch (itemType)
                    {
                        case ItemType.Harvestable:

                        case ItemType.Misc:
                            ItemDatabaseRow i = new ItemDatabaseRow();
                            i.Id = Convert.ToInt32(reader["Id"]);
                            i.ItemName = Convert.ToString(reader["ItemName"]);
                            i.Description = Convert.ToString(reader["Description"]);
                            i.BuyPrice = Convert.ToInt32(reader["BuyPrice"]);
                            i.SellPrice = Convert.ToInt32(reader["SellPrice"]);
                            i.TextureSpritePath = Convert.ToString(reader["TextureSpritePath"]);
                            i.ItemType = itemType;
                            i.MeshPath = Convert.ToString(reader["MeshPath"]);

                            return i;

                        case ItemType.Seed:
                            SeedDatabaseRow seed = new SeedDatabaseRow();
                            seed.Id = Convert.ToInt32(reader["Id"]);
                            seed.ItemName = Convert.ToString(reader["ItemName"]);
                            seed.Description = Convert.ToString(reader["Description"]);
                            seed.BuyPrice = Convert.ToInt32(reader["BuyPrice"]);
                            seed.SellPrice = Convert.ToInt32(reader["SellPrice"]);
                            seed.TextureSpritePath = Convert.ToString(reader["TextureSpritePath"]);
                            seed.ItemType = itemType;
                            seed.MeshPath = Convert.ToString(reader["MeshPath"]);
                            seed.SeedType = (SeedType)Convert.ToInt32(reader["Param1"]);
                            seed.StagesAmount = Convert.ToInt32(reader["Param2"]);
                            seed.MinSecondsToChangeState = Convert.ToInt32(reader["Param3"]);
                            seed.MaxSecondsToChangeState = Convert.ToInt32(reader["Param4"]);
                            seed.GrowUpId = Convert.ToInt32(reader["Param5"]);
                            seed.MinCropAmount = Convert.ToInt32(reader["Param6"]);
                            seed.MaxCropAmount = Convert.ToInt32(reader["Param7"]);

                            return seed;

                        case ItemType.Fertilizer:
                            FertilizerDatabaseRow fertilizer = new FertilizerDatabaseRow();
                            fertilizer.Id = Convert.ToInt32(reader["Id"]);
                            fertilizer.ItemName = Convert.ToString(reader["ItemName"]);
                            fertilizer.Description = Convert.ToString(reader["Description"]);
                            fertilizer.BuyPrice = Convert.ToInt32(reader["BuyPrice"]);
                            fertilizer.SellPrice = Convert.ToInt32(reader["SellPrice"]);
                            fertilizer.TextureSpritePath = Convert.ToString(reader["TextureSpritePath"]);
                            fertilizer.ItemType = itemType;
                            fertilizer.MeshPath = Convert.ToString(reader["MeshPath"]);
                            fertilizer.FertilizerType = (FertilizerType)Convert.ToInt32(reader["Param1"]);
                            fertilizer.SecondsDuration = Convert.ToInt32(reader["Param2"]);

                            return fertilizer;

                        case ItemType.Pot:
                            PotDatabaseRow pot = new PotDatabaseRow();
                            pot.Id = Convert.ToInt32(reader["Id"]);
                            pot.ItemName = Convert.ToString(reader["ItemName"]);
                            pot.Description = Convert.ToString(reader["Description"]);
                            pot.BuyPrice = Convert.ToInt32(reader["BuyPrice"]);
                            pot.SellPrice = Convert.ToInt32(reader["SellPrice"]);
                            pot.TextureSpritePath = Convert.ToString(reader["TextureSpritePath"]);
                            pot.ItemType = itemType;
                            pot.MeshPath = Convert.ToString(reader["MeshPath"]);
                            pot.WaterTime = Convert.ToInt32(reader["Param1"]);
                            pot.SmallPotsAmount = Convert.ToInt32(reader["Param2"]);
                            pot.BigPotsAmount = Convert.ToInt32(reader["Param3"]);

                            return pot;

                        case ItemType.BattlePlant:
                            BattlePlantDataBaseRow plant = new BattlePlantDataBaseRow();
                            plant.Id = Convert.ToInt32(reader["Id"]);
                            plant.ItemName = Convert.ToString(reader["ItemName"]);
                            plant.Description = Convert.ToString(reader["Description"]);
                            plant.BuyPrice = Convert.ToInt32(reader["BuyPrice"]);
                            plant.SellPrice = Convert.ToInt32(reader["SellPrice"]);
                            plant.TextureSpritePath = Convert.ToString(reader["TextureSpritePath"]);
                            plant.ItemType = itemType;
                            plant.MeshPath = Convert.ToString(reader["MeshPath"]);
                            plant.BattlePlantScenePath = Convert.ToString(reader["BattlePlantScenePath"]);
                            plant.Maxlvl = Convert.ToInt32(reader["Maxlvl"]);
                            plant.BuyCropId = Convert.ToInt32(reader["ButCropId"]);

                            return plant;
                    }
                }
            }
        }

        return null;
    }
    public static ItemType GetItemType(int id)
    {
        using (SqliteConnection con = new SqliteConnection("Data Source = Db.db"))
        {
            con.Open();

            SqliteCommand sqliteCommand = con.CreateCommand();
            sqliteCommand.CommandText = $"SELECT ItemType FROM Items WHERE Id = {id}";

            using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    ItemType itemType = (ItemType)reader.GetInt32(0);

                    return itemType;
                }
            }
        }

        return ItemType.Undefined;
    }

    public static int GetItemIdByName(string itemName)
    {
        int itemId = -1; // якщо предмет не знайдено, повертаємо -1
        string query = "SELECT id FROM items WHERE name = @itemName";

        using (SqliteConnection connection = new SqliteConnection("Data Source = Db.db"))
        using (SqliteCommand command = new SqliteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@itemName", itemName);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    itemId = (int)result;
                }
            }
            catch (Exception ex)
            {
                GD.Print("Error: " + ex.Message);
            }
        }

        return itemId;
    }

    public static (string itemName, Texture2D texture) GetItemDataById(int itemId)
    {
        string query = "SELECT ItemName, TextureSpritePath FROM items WHERE Id = @itemId";

        using (SqliteConnection connection = new SqliteConnection("Data Source = Db.db"))
        using (SqliteCommand command = new SqliteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@itemId", itemId);

            try
            {
                connection.Open();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string itemName = reader.GetString(0);
                        string texturePath = reader.GetString(1);

                        Texture2D texture = ResourceLoader.Load<Texture2D>(texturePath);

                        return (itemName, texture);
                    }
                }
            }
            catch (Exception ex)
            {
                GD.Print("Error: " + ex.Message);
            }
        }

        return (null, null); // Якщо запис не знайдено
    }
}
